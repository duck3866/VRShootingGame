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
        _controllerCore.agent.isStopped = false;
    }

    public void OperateUpdate()
    {
        if (_controllerCore.agent != null)
        {
            if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) > _controllerCore.enemyAbility.AttackDistance)
            {
                _controllerCore.agent.SetDestination(_controllerCore.player.transform.position);
            }
            else if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) < _controllerCore.enemyAbility.AttackDistance)
            {
                _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Attack);
            }
        }
    }

    public void OperateExit()
    {
        _controllerCore.agent.isStopped = true;
    }
}
