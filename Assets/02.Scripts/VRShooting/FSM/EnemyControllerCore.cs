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

    public EnemyAbility enemyAbility;

    public GameObject player;
    public float AttackDistance { get; set; }
    public float EnemyHp { get; private set; }
    public IState<EnemyControllerCore> CurrentState { get; private set; }
    public IState<EnemyControllerCore> PreviousState { get; private set; }
    // public FixedJoint fixedJoint;

    private Dictionary<EnemyState, IState<EnemyControllerCore>> _states =
        new Dictionary<EnemyState, IState<EnemyControllerCore>>();
    // public event Action OnInteractable;

    Rigidbody[] rigidbodies;
    public NavMeshAgent agent;
    public Animator animator;

    public bool iHaveGun;
    public GameObject haveGunPosition;
    [HideInInspector] public EnemyAnimationEventHandler enemyAnimationEventHandler;
    [SerializeField] private EnemyColliderInfo[] _colliderInfos = new EnemyColliderInfo[10];
    public bool isDie = false;
    public GameObject bulletPrefab;
    public GameObject firePosition;
    private CanInteractablePoint _canInteractablePoint;
    // private float _hitDamage;
    private float _powerDamage = 1;
    public Rigidbody hips;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public bool IsTharwing = false;
    public Material hitMaterial; // 빨간색 머티리얼
    private Material[] _originalMaterials;

    public void Start()
    {
        IsTharwing = false;
        _originalMaterials = skinnedMeshRenderer.materials;
        enemyAnimationEventHandler = GetComponent<EnemyAnimationEventHandler>();
        _canInteractablePoint = GetComponentInChildren<CanInteractablePoint>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // fixedJoint = GetComponent<FixedJoint>();
        if (enemyAbility != null)
        {
            Debug.Log("적 Ability 초기화");
            Initialized(enemyAbility);
        }

        rigidbodies = GetComponentsInChildren<Rigidbody>();
        OffCharacterJoint();
        CanInteractablePoint[] interactablePoints = GetComponentsInChildren<CanInteractablePoint>();
        foreach (var point in interactablePoints)
        {
            point.Init(this);
        }
    }

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

    public void OnCharacterJoint()
    {
        animator.enabled = false;
        agent.enabled = false;
        // if (!isDie)
        // {
        //     ChangeState(EnemyState.Grabbing);
        // }
        foreach (var joint in rigidbodies)
        {
            joint.isKinematic = false;
        }
    }

    public void OffCharacterJoint()
    {
        if (!isDie)
        {
            animator.enabled = true;
            // ChangeState(EnemyState.Chase);
            agent.enabled = true;
            foreach (var joint in rigidbodies)
            {
                joint.isKinematic = true;
            }
            // fixedJoint.connectedBody = null;
        }
    }

    private void Update()
    {
        agent.stoppingDistance = AttackDistance;
        if (!isDie)
        {
            iHaveGun = haveGunPosition.transform.childCount != 0;
            // Debug.Log($"현재 상태:{CurrentState} 이전 상태:{PreviousState}");
            CurrentState?.OperateUpdate();
        }
    }

    public void ChangeState(EnemyState newState)
    {
        // Debug.Log($"상태 전환 이전:{CurrentState} 이후:{newState}");
        CurrentState?.OperateExit();
        PreviousState = CurrentState;
        CurrentState = _states[newState];
        CurrentState?.OperateEnter();
    }

    public void TakeDamage(float damage)
    {
        EnemyHp -= (damage * _powerDamage);
        if (EnemyHp > 0)
        {
            StartCoroutine(WaitForDamaged());
        }
        else
        {
            ChangeState(EnemyState.Die);
        }
    }

    public void HitPoint(Vector3 hitPoint)
    {
        foreach (var colliderInfo in _colliderInfos)
        {
            if (colliderInfo.collider.bounds.Contains(hitPoint))
            {
                Debug.Log($"맞은 부위: {colliderInfo.collider.name}, {_powerDamage * colliderInfo.weightDamage}");
                _powerDamage *= colliderInfo.weightDamage;
            }
        }
    }
    private IEnumerator WaitForDamaged()
    {
        Material[] hitMaterials = new Material[_originalMaterials.Length];
        for (int i = 0; i < hitMaterials.Length; i++)
        {
            hitMaterials[i] = hitMaterial;
        }

        skinnedMeshRenderer.materials = hitMaterials;
        animator.SetTrigger("toDamaged");
        ChangeState(EnemyState.Damaged);
        yield return new WaitForSeconds(1f);
        // ChangeState(EnemyState.Damaged);
        _powerDamage = 1f;
        skinnedMeshRenderer.materials = _originalMaterials;
    }

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
            Debug.Log("바닥");
            if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
            {
                IDamagable damagable = other.GetComponentInChildren<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(5f);
                }
                TakeDamage(5f);
            }
            // if (other.gameObject.CompareTag("Enemy"))
            // {
            //     Debug.Log("적과 부딪힘");
            //     if (other.gameObject.TryGetComponent(out IDamagable damagable))
            //     {
            //         damagable.TakeDamage(5f);
            //     }
            //     TakeDamage(5f);
            // }
        }
    }
    public void DieAction()
    {
        isDie = true;
        if (_canInteractablePoint.fixedJoint != null) _canInteractablePoint.fixedJoint.connectedBody = null;
        // _canInteractablePoint.ExitGrabbing();
        _canInteractablePoint.Grabbed = true;
        

        agent.isStopped = true;
        agent.enabled = false;
        animator.applyRootMotion = false;
        animator.enabled = false;

        FixedJoint[] fixedJoints = GetComponentsInChildren<FixedJoint>();
        foreach (var fixedJoint in fixedJoints)
        {
            Destroy(fixedJoint);
        }

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        Material[] hitMaterials = new Material[_originalMaterials.Length];
        for (int i = 0; i < hitMaterials.Length; i++)
        {
            hitMaterials[i] = hitMaterial;
        }

        skinnedMeshRenderer.materials = hitMaterials;
    }
}