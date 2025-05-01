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
    public float EnemyHp { get; private set; }
    public IState<EnemyControllerCore> CurrentState { get; private set; }
    public IState<EnemyControllerCore> PreviousState { get; private set; }
    // public FixedJoint fixedJoint;

    private Dictionary<EnemyState, IState<EnemyControllerCore>> _states = new Dictionary<EnemyState, IState<EnemyControllerCore>>();
    // public event Action OnInteractable;
    
    Rigidbody[] rigidbodies;
    public NavMeshAgent agent;
    public Animator animator;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // fixedJoint = GetComponent<FixedJoint>();
        if (enemyAbility != null)
        {
            Debug.Log("무야호~");
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
        ChangeState(EnemyState.Grabbing);
        Debug.Log("왜 안됨 2");
        agent.enabled = false;
        foreach (var joint in rigidbodies)
        {
            joint.isKinematic = false;
        }
        // fixedJoint.connectedBody = transform.parent.GetComponent<Rigidbody>();
        // fixedJoint.breakForce = Mathf.Infinity; // 원하는 힘으로 설정
        // fixedJoint.breakTorque = Mathf.Infinity;
    }
    public void OffCharacterJoint()
    {
        animator.enabled = true;
        ChangeState(EnemyState.Chase);
        Debug.Log("왜 안됨");
        agent.enabled = true;
        foreach (var joint in rigidbodies)
        {
            joint.isKinematic = true;
        }
        // fixedJoint.connectedBody = null;
    }
  
    private void Update()
    {
        Debug.Log($"현재 상태:{CurrentState} 이전 상태:{PreviousState}");
        CurrentState?.OperateUpdate();
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
            ChangeState(EnemyState.Damaged);
        }
        else
        {
            ChangeState(EnemyState.Die);
        }
    }

    public void HitPoint(Vector3 hitPoint)
    {
        
    }
}
