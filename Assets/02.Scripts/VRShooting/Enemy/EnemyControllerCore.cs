using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerCore : MonoBehaviour, IDamagable
{
    public enum EnemyState
    {
        Chase,
        Attack,
        Damaged,
        Grabbing,
        Die
    }
    public IState<EnemyControllerCore> CurrentState { get; private set; } // 현재 상태
    public IState<EnemyControllerCore> PreviousState { get; private set; } // 이전 상태
    
    [Header("적의 스텟 정보")]
    public EnemyAbility enemyAbility; // 적의 체력, 공격 범위, 이동 속도 등의 정보를 담은 클래스
    public float AttackDistance { get; set; } // 공격 범위
    public float EnemyHp { get; protected set; } // 적의 체력
    
   
    [Header("죽었는지 여부")]
    public bool isDie = false; // 적이 죽었는지 여부
    [Header("던져지고 있는지 여부")]
    public bool IsTharwing = false; // 던져지고 있는지 여부
    
    [Header("총의 위치")] // 총의 위치
    public GameObject haveGunPosition;
    [Header("소환될 총알 프리팹")]
    public GameObject bulletPrefab; //소환될 총알 프리팹
    [Header("총알이 발사될 위치")]
    public GameObject firePosition; //총알이 발사될 위치
    
    [Header("중심 리지드 바디")]
    public Rigidbody hips; // 중심 리지드 바디
    
    [Header("변경될 메시렌더러")]
    public SkinnedMeshRenderer skinnedMeshRenderer; // 변경될 메시 렌더러
    [Header("피격되었을때 변할 색")]
    public Material hitMaterial; // 피격색 머티리얼
    
    [HideInInspector] public GameObject player; // 추격할 플레이어 자동 할당됨
    [HideInInspector] public NavMeshAgent agent; // navMeshAgent
    [HideInInspector] public Animator animator; // animator
    
    [HideInInspector] public bool iHaveGun; // 총을 보유하고 있는지 여부
    [HideInInspector] public EnemyAnimationEventHandler enemyAnimationEventHandler; // 애니메이션 이벤트 처리 클래스
    
    protected float PowerDamage = 1; // 받은 데미지
    
    protected Material[] OriginalMaterials; // 원래 색 머티리얼
    protected CanInteractablePoint CanInteractablePoint; // Grab 처리 클래스
    protected Rigidbody[] Rigidbodies; // 적의 래그돌을 담을 리스트
    
    [Header("데미지 가중치 리스트"),SerializeField] private EnemyColliderInfo[] _colliderInfos = new EnemyColliderInfo[10]; // 데미지 가중치 리스트 
    // public event Action OnInteractable;
    private Dictionary<EnemyState, IState<EnemyControllerCore>> _states =
        new Dictionary<EnemyState, IState<EnemyControllerCore>>(); // 적의 상태를 담을 딕셔너리
   
  
    // private float _hitDamage;
  

    public virtual void Start()
    {
        IsTharwing = false;
        OriginalMaterials = skinnedMeshRenderer.materials;
        enemyAnimationEventHandler = GetComponent<EnemyAnimationEventHandler>();
        CanInteractablePoint = GetComponentInChildren<CanInteractablePoint>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // fixedJoint = GetComponent<FixedJoint>();
        if (enemyAbility != null)
        {
            Debug.Log("적 Ability 초기화");
            Initialized(enemyAbility);
        }

        Rigidbodies = GetComponentsInChildren<Rigidbody>();
        OffCharacterJoint();
        CanInteractablePoint[] interactablePoints = GetComponentsInChildren<CanInteractablePoint>();
        foreach (var point in interactablePoints)
        {
            point.Init(this);
        }
    }
    /// <summary>
    /// 적의 정보를 초기화 하는 클래스
    /// </summary>
    /// <param name="ability">초기화 할 어빌리티 정보</param>
    public virtual void Initialized(EnemyAbility ability)
    {
        enemyAbility = ability;
        EnemyHp = ability.MaxHp;
        agent.speed = ability.MoveSpeed;
        this.AttackDistance = ability.AttackDistance;
        player = GameObject.FindWithTag("Player");

        IState<EnemyControllerCore> chase = new ChaseState();
        IState<EnemyControllerCore> attack = new AttackState();
        IState<EnemyControllerCore> damaged = new DamagedState();
        IState<EnemyControllerCore> die = new DieState();
        IState<EnemyControllerCore> grabbing = new GrabbingState();

        _states.Add(EnemyState.Chase, chase);
        _states.Add(EnemyState.Attack, attack);
        _states.Add(EnemyState.Damaged, damaged);
        _states.Add(EnemyState.Die, die);
        _states.Add(EnemyState.Grabbing, grabbing);
        foreach (var state in _states.Values)
        {
            state.Init(this);
        }

        ChangeState(EnemyState.Chase);
    }
    /// <summary>
    /// CharacterJoint를 활성화 하는 함수
    /// </summary>
    public void OnCharacterJoint()
    {
        animator.enabled = false;
        agent.enabled = false;
        // if (!isDie)
        // {
        //     ChangeState(EnemyState.Grabbing);
        // }
        foreach (var joint in Rigidbodies)
        {
            joint.isKinematic = false;
        }
    }
    /// <summary>
    /// CharacterJoint를 비활성화 하는 함수
    /// </summary>
    public void OffCharacterJoint()
    {
        if (!isDie)
        {
            animator.enabled = true;
            // ChangeState(EnemyState.Chase);
            agent.enabled = true;
            foreach (var joint in Rigidbodies)
            {
                joint.isKinematic = true;
            }
            // fixedJoint.connectedBody = null;
        }
    }

    // private float bossHp = 20;
    // private float bossMaxHp = 20;
    // private float test = 1f;
    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //    // TakeDamage(5f);
        //    UIManager.Instance.AddPointText("+"+test.ToString());
        //    test += 1;
        // }
        agent.stoppingDistance = AttackDistance;
        if (!isDie)
        {
            iHaveGun = haveGunPosition.transform.childCount != 0;
            // Debug.Log($"현재 상태:{CurrentState} 이전 상태:{PreviousState}");
            CurrentState?.OperateUpdate();
        }
    }
    /// <summary>
    /// 적의 상태를 변화함
    /// </summary>
    /// <param name="newState">변화할 상태</param>
    public void ChangeState(EnemyState newState)
    {
        // Debug.Log($"상태 전환 이전:{CurrentState} 이후:{newState}");
        CurrentState?.OperateExit();
        PreviousState = CurrentState;
        CurrentState = _states[newState];
        CurrentState?.OperateEnter();
    }
    /// <summary>
    /// 데미지를 받음
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    public virtual void TakeDamage(float damage)
    {
        if (!isDie)
        {
            EnemyHp -= (damage * PowerDamage);
            if (EnemyHp > 0)
            {
                StartCoroutine(WaitForDamaged());
            }
            else
            {
                ChangeState(EnemyState.Die);
            } 
        }
    }
    /// <summary>
    /// 데미지를 받은 위치
    /// </summary>
    /// <param name="hitPoint">위치 좌표</param>
    public void HitPoint(Vector3 hitPoint)
    {
        Debug.Log("에ㅔㅔ");
        foreach (var colliderInfo in _colliderInfos)
        {
            if (colliderInfo.collider.bounds.Contains(hitPoint))
            {
                Debug.Log($"맞은 부위: {colliderInfo.collider.name}, {PowerDamage * colliderInfo.weightDamage}");
                PowerDamage *= colliderInfo.weightDamage;
                // int point = (int)enemyAbility.Type;
                UIManager.Instance.AddPointText($"+적 {colliderInfo.name} 공격 "+ 50f* colliderInfo.weightDamage);
                GameManager.Instance.AddPoint(50f* colliderInfo.weightDamage);
            }
        }
    }

    public void AddForceMethod(Vector3 direction)
    {
        StartCoroutine(CanInteractablePoint.ExitGrabbingAction(direction));
    }
    protected IEnumerator WaitForDamaged()
    {
        Material[] hitMaterials = new Material[OriginalMaterials.Length];
        for (int i = 0; i < hitMaterials.Length; i++)
        {
            hitMaterials[i] = hitMaterial;
        }

        skinnedMeshRenderer.materials = hitMaterials;
        animator.SetTrigger("toDamaged");
        ChangeState(EnemyState.Damaged);
        yield return new WaitForSeconds(1f);
        // ChangeState(EnemyState.Damaged);
        PowerDamage = 1f;
        skinnedMeshRenderer.materials = OriginalMaterials;
    }
    /// <summary>
    /// 총알 생성 함수
    /// </summary>
    public void InstancePrefab()
    {
        Debug.Log("총알 생성");
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bullet.transform.position = firePosition.transform.position;
        bullet.transform.forward = -firePosition.transform.up;
        bullet.SetActive(true);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"뭔가 닿았는데 이거 뭐임: {other.gameObject.name}");
        if (IsTharwing)
        {
            // Debug.Log("바닥");
            if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
            {
                IDamagable damagable = other.GetComponentInChildren<IDamagable>();
                if (damagable != null)
                {
                    UIManager.Instance.AddPointText($"+적 충돌 {50f}");
                    GameManager.Instance.AddPoint(50f);
                    damagable.TakeDamage(5f);
                }
                UIManager.Instance.AddPointText($"+적 던져짐 {50f}");
                GameManager.Instance.AddPoint(50f);
                TakeDamage(5f);
            }
        }
    }
    /// <summary>
    /// 적이 죽었을때 호출되는 함수
    /// </summary>
    public void DieAction()
    {
        isDie = true;
        UIManager.Instance.AddPointText($"+적 처치 {(int)enemyAbility.Type * 100f}");
        GameManager.Instance.AddPoint((int)enemyAbility.Type * 100f);
        if (CanInteractablePoint.fixedJoint != null) CanInteractablePoint.fixedJoint.connectedBody = null;
        // CanInteractablePoint.ExitGrabbing();
        CanInteractablePoint.Grabbed = true;
        

        // agent.isStopped = true;
        agent.enabled = false;
        animator.applyRootMotion = false;
        animator.enabled = false;

        FixedJoint[] fixedJoints = GetComponentsInChildren<FixedJoint>();
        foreach (var fixedJoint in fixedJoints)
        {
            Destroy(fixedJoint);
        }

        foreach (var rb in Rigidbodies)
        {
            rb.isKinematic = false;
        }

        Material[] hitMaterials = new Material[OriginalMaterials.Length];
        for (int i = 0; i < hitMaterials.Length; i++)
        {
            hitMaterials[i] = hitMaterial;
        }

        skinnedMeshRenderer.materials = hitMaterials;
    }
}