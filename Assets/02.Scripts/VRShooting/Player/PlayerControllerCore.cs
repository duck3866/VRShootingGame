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

    [Header("Trail Settings")]
    public float trailDuration = 2.0f; // 잔상 효과가 지속되는 시간
    public bool trailActive = true;    // 잔상 효과 활성화 여부
    
    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 0.2f;
    
    [Header("Shader Related")] 
    public Material mat;

    private Queue<GameObject> ghostGameObjects = new Queue<GameObject>();
    private SkinnedMeshRenderer _skinnedRenderer;
    private GameObject _spawnedVRIK;
    private VRHandIK _vrHandIK;
    private bool _isVRIKReady = false;
    
    private Animator animator;
    
    private bool isInitialized = false;

    private void Start()
    {
        
        StartCoroutine(SpawnVRIK());
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

        // if (Input.GetMouseButtonDown(0))
        // {
        //     GameManager.Instance.TimeStop(true);
        // }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     GameManager.Instance.TimeStop(false);
        // }
    }

    private IEnumerator SpawnVRIK()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        // VRIK 생성
        _spawnedVRIK = Instantiate(VRIKObject, transform);
        _spawnedVRIK.transform.localPosition = new Vector3(0f, -1f, -0.3f);
        animator = _spawnedVRIK.GetComponent<Animator>();
        
        SkinnedMeshRenderer[] skinnedMeshRenderers = _spawnedVRIK.GetComponentsInChildren<SkinnedMeshRenderer>();
        _skinnedRenderer = skinnedMeshRenderers[1];
        GameManager.Instance.AddEvent(StartTimeStop,true);
        GameManager.Instance.AddEvent(EndTimeStop,false);
        //
        // // VRIK 컴포넌트 가져오기
        // _vrHandIK = _spawnedVRIK.GetComponent<VRHandIK>();
        // if (_vrHandIK == null)
        // {
        //     Debug.LogError("VRHandIK 컴포넌트를 찾을 수 없습니다!");
        //     yield break;
        // }
        //
        // // 타겟 설정
        // _vrHandIK.leftHandTarget = LeftHand.transform;
        // _vrHandIK.rightHandTarget = RightHand.transform;

        // SkinnedMeshRenderer 찾기 - VRIK 오브젝트에서 찾아야 함
        // if (_skinnedRenderer == null)
        // {
        //     Debug.LogError("SkinnedMeshRenderer를 찾을 수 없습니다!");
        //     yield break;
        // }
        //
        // Debug.Log("VRIK 초기화 완료: " + _skinnedRenderer.name);
        // _isVRIKReady = true;
        //
        // // 잔상 효과 시작
        // if (trailActive)
        // {
        //     StartCoroutine(CreateTrailEffect());
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
            ghost.transform.position = _spawnedVRIK.transform.position + (-gameObject.transform.forward * 0.5f);
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
            mr.material = ghostMat;
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