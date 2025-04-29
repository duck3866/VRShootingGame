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
        Die
    }
    
    public EnemyAbility enemyAbility;
    
    public GameObject player;
    public float EnemyHp { get; private set; }
    public IState<EnemyControllerCore> CurrentState { get; private set; }
    public IState<EnemyControllerCore> PreviousState { get; private set; }

    private Dictionary<EnemyState, IState<EnemyControllerCore>> _states = new Dictionary<EnemyState, IState<EnemyControllerCore>>();
    public NavMeshAgent agent;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (enemyAbility != null)
        {
            Debug.Log("무야호~");
            Initialized(enemyAbility);
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
        
        _states.Add(EnemyState.Chase, chase);
        _states.Add(EnemyState.Attack, attack);
        _states.Add(EnemyState.Damaged, damaged);
        _states.Add(EnemyState.Die,die);
        foreach (var state in _states.Values)
        {
            state.Init(this);
        }
        ChangeState(EnemyState.Chase);
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
