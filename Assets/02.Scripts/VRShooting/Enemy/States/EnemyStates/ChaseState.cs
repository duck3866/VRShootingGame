using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState<EnemyControllerCore>
{
    private EnemyControllerCore _controllerCore;
    public void Init(EnemyControllerCore controller)
    {
        _controllerCore = controller;
    }

    public void OperateEnter()
    {
        _controllerCore.enemyAnimationEventHandler.InitParameter();
        _controllerCore.animator.SetTrigger("toRunning");
        if (_controllerCore.agent.enabled)
        {
            _controllerCore.agent.isStopped = false;
        }
    }

    public void OperateUpdate()
    {
        if (_controllerCore.agent != null)
        {
            if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) >= _controllerCore.AttackDistance)
            {
                if (_controllerCore.agent != null && _controllerCore.agent.enabled)
                {
                    _controllerCore.agent.SetDestination(_controllerCore.player.transform.position);   
                }
                // Debug.Log($"뭔데 시발{Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position)},{_controllerCore.AttackDistance}");
            }
            else if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) < _controllerCore.AttackDistance)
            {
                _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Attack);
            }
        }
    }

    public void OperateExit()
    {
        if (_controllerCore.agent.enabled)
        {
            _controllerCore.agent.isStopped = true;
        }
    }
}
