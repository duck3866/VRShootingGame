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

    private Dictionary<EnemyState, IState<EnemyControllerCore>> _states = new Dictionary<EnemyState, IState<EnemyControllerCore>>();
    // public event Action OnInteractable;
    
    Rigidbody[] rigidbodies;
    public NavMeshAgent agent;
    public Animator animator;

    public bool iHaveGun;
    public GameObject haveGunPosition;

    public bool isDie = false;
    public GameObject bulletPrefab;
    public GameObject firePosition;
    private CanInteractablePoint _canInteractablePoint;
    public void Start()
    {
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
    public virtual void Initialized(EnemyAbility  ability)
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
        _states.Add(EnemyState.Die,die);
        _states.Add(EnemyState.Grabbing,grabbing);
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
        if (!isDie)
        {
            ChangeState(EnemyState.Grabbing);
        }
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
            ChangeState(EnemyState.Chase);
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
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(1);
        }

        if (!isDie)
        {
            iHaveGun = haveGunPosition.transform.childCount != 0;
            // Debug.Log($"현재 상태:{CurrentState} 이전 상태:{PreviousState}");
            CurrentState?.OperateUpdate();  
        }
    }
    public void ChangeState(EnemyState newState)
    {
        CurrentState?.OperateExit();
        PreviousState = CurrentState;
        CurrentState = _states[newState];
        CurrentState?.OperateEnter();
    }

    public void TakeDamage(float damage)
    {
        if (EnemyHp > 0)
        {
            EnemyHp -= damage;
            StartCoroutine(WaitForDamaged());
        }
        else
        {
            ChangeState(EnemyState.Die);
        }
    }

    public void HitPoint(Vector3 hitPoint)
    {
        
    }

    private IEnumerator WaitForDamaged()
    {
        animator.SetTrigger("toDamaged");
        yield return new WaitForSeconds(1f);
        ChangeState(EnemyState.Damaged);
    }

    public void InstancePrefab()
    {
        Debug.Log("총알 생성");
        GameObject bullet =  Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bullet.transform.position = firePosition.transform.position;
        bullet.transform.forward = -firePosition.transform.up;
        bullet.SetActive(true);
    }

    public void DieAction()
    {
        _canInteractablePoint.ExitGrabbing();
        _canInteractablePoint.Grabbed = true;
        isDie = true;

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

    }
}
