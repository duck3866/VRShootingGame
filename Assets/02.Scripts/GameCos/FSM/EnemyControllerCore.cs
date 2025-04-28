using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerCore : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Attack,
        Damaged,
        Die
    }
    public IState<EnemyControllerCore> CurrentState { get; private set; }
    public IState<EnemyControllerCore> PreviousState { get; private set; }
    
    private Dictionary<EnemyState, IState<EnemyControllerCore>> _states;
    private void Start()
    {
        _states.Add(EnemyState.Chase, new ChaseState());
        _states.Add(EnemyState.Attack, new AttackState());
        _states.Add(EnemyState.Damaged, new DamagedState());
        _states.Add(EnemyState.Die, new DieState());
        foreach (var state in _states.Values)
        {
            state.Init(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        CurrentState?.OperateUpdate();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState?.OperateExit();
        PreviousState = CurrentState;
        CurrentState = _states[newState];
        CurrentState?.OperateEnter();
    }
}
