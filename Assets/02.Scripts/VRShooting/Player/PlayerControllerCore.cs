using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCore : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] private GameObject VRIKObject;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject lookCube;
    [Header("Trail Settings")]
    public float trailDuration = 2.0f; // 잔상 효과가 지속되는 시간
    public bool trailActive = true;    // 잔상 효과 활성화 여부
    
    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 0.2f;
    
    [Header("Shader Related")] 
    public Material mat;

    public GameObject PlayerUI;
    private Queue<GameObject> ghostGameObjects = new Queue<GameObject>();
    private SkinnedMeshRenderer _skinnedRenderer;
    private GameObject _spawnedVRIK;
    private VRHandIK _vrHandIK;
    private bool _isVRIKReady = false;
    
    private Animator animator;
    private Animator IKAnimator;
    
    private bool isInitialized = false;
    private GameObject _player;
    private void Start()
    {
        StartCoroutine(SpawnVRIK());
        // ------- 촬영용 ------
        // PlayerUI.transform.localPosition = new Vector3(-0.09f, 0.02f, 0f);
        // lookCube.transform.localPosition = new Vector3(0, -0.3f, 0.3f);
    }

    private void Update()
    {
        if (animator != null)
        {
            float h = ARAVRInput.GetAxisLeft("Horizontal");
            float v = ARAVRInput.GetAxisLeft("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            if (dir.magnitude > 0)
            {
                animator.SetTrigger("toRun");
            }
            else
            {
                animator.SetTrigger("toIdle");
            }   
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.PlayerGameOver();
            // GameManager.Instance.TimeStop(true);
        }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     GameManager.Instance.TimeStop(false);
        // }
    }

    private IEnumerator SpawnVRIK()
    {
        yield return new WaitForSecondsRealtime(3f);
        
        // VRIK 생성
        _spawnedVRIK = Instantiate(VRIKObject, PlayerUI.transform);
        _player = _spawnedVRIK;
        _spawnedVRIK.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _spawnedVRIK.transform.localPosition = new Vector3(0f, -1.6f, 0f);
        // _spawnedVRIK.transform.forward = transform.forward;
        Animator[] animators = _spawnedVRIK.GetComponentsInChildren<Animator>();
        animator = animators[0];
        // Debug.Log(animator+ "????/");
        IKAnimator = animators[1];
        
        SkinnedMeshRenderer[] skinnedMeshRenderers = _spawnedVRIK.GetComponentsInChildren<SkinnedMeshRenderer>();
        _skinnedRenderer = skinnedMeshRenderers[3];
        Renderer[] materials = _spawnedVRIK.GetComponentsInChildren<Renderer>();
        mat = materials[1].material;
        GameManager.Instance.AddEvent(StartTimeStop,true);
        GameManager.Instance.AddEvent(EndTimeStop,false);

        PlayerVRIK playerVRIK = _spawnedVRIK.GetComponentInChildren<PlayerVRIK>();
        // GameObject UIManagerObject = GetComponentInChildren<UIManager>().gameObject;
        playerVRIK.Initialized();
        // if (lookCube != null)
        // {
        //     playerVRIK.LookTarget = lookCube.transform;
        // }
       
    }

    public void StartTimeStop()
    {
        trailActive = true;
        StartCoroutine(CreateTrailEffect());
    }

    public void EndTimeStop()
    {
        trailActive = false;
        StartCoroutine(DeleteTrailEffect());
    }
    private IEnumerator CreateTrailEffect()
    {
        // VRIK가 준비되지 않았다면 종료
        if (_skinnedRenderer == null)
        {
            Debug.LogError("잔상 효과를 시작할 수 없습니다. VRIK가 준비되지 않았습니다.");
            yield break;
        }
        
        Debug.Log("잔상 효과 시작");
        
        while (trailActive) // 무한 루프 대신 bool 값으로 제어
        {
            // 고스트 메시 생성
            GameObject ghost = new GameObject("GhostMesh");
            // ghost.transform.position = _spawnedVRIK.transform.position + (-gameObject.transform.forward * 0.2f) + new Vector3(0,1f,0);
            ghost.transform.position = _spawnedVRIK.transform.position + new Vector3(0,1f,0);
            ghost.transform.rotation = _spawnedVRIK.transform.rotation;
            ghost.transform.localScale = _spawnedVRIK.transform.localScale;
            
            // 현재 포즈의 메시 베이킹
            Mesh bakedMesh = new Mesh();
            _skinnedRenderer.BakeMesh(bakedMesh);
            
            // 메시 필터 추가
            MeshFilter mf = ghost.AddComponent<MeshFilter>();
            mf.mesh = bakedMesh;

            // 렌더러 및 머티리얼 적용
            MeshRenderer mr = ghost.AddComponent<MeshRenderer>();
            Material ghostMat = new Material(mat);
            ghostMat.color = GetNextGhostColor();
            ghostColorStep = (ghostColorStep + 1) % 300;
            // Renderer[] materials = _spawnedVRIK.GetComponentsInChildren<Renderer>();
            // mat = materials[1].material;
            mr.material = ghostMat;
            // mat = _skinnedRenderer.material;
            // mr.material = new Material(mat);
            ghostGameObjects.Enqueue(ghost);
            // 페이드 아웃 및 제거 코루틴 시작
            // StartCoroutine(FadeAndDestroy(ghost, ghostMat));
            
            // 다음 잔상까지 대기
            yield return new WaitForSecondsRealtime(meshRefreshRate);
        }
    }

    public IEnumerator DeleteTrailEffect()
    {
        // trailActive = false;
        foreach (var ghost in ghostGameObjects)
        {
            Destroy(ghost);
            yield return null;
            // yield return new WaitForSecondsRealtime(0.001f);
        }
    }

    private int ghostColorStep = 0;

    private Color GetNextGhostColor()
    {
        float t = (ghostColorStep % 100) / 100f;
        if (ghostColorStep < 100)
        {
            return Color.Lerp(Color.green, Color.cyan, t);
        }
        else if (ghostColorStep < 200)
        {
            return Color.Lerp(Color.blue, Color.red, t);
        }
        else
        {
            return Color.Lerp(Color.red, Color.green, t);
        }
    }
    private IEnumerator FadeAndDestroy(GameObject obj, Material mat)
    {
        float timer = 0f;
        Color startColor = mat.color;

        while (timer < meshDestroyDelay)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / meshDestroyDelay);
            mat.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        
        Destroy(obj);
    }

    // 에디터에서 테스트를 위한 메서드
    public void ToggleTrail()
    {
        trailActive = !trailActive;
        
        if (trailActive && _isVRIKReady)
        {
            StartCoroutine(CreateTrailEffect());
        }
    }
}