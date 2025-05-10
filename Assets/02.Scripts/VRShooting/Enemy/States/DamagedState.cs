using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : IState<EnemyControllerCore>
{
    private EnemyControllerCore _controllerCore;
    public void Init(EnemyControllerCore controller)
    {
        _controllerCore = controller;
    }

    public void OperateEnter()
    {
        if (_controllerCore.agent.enabled)
        {
            _controllerCore.agent.isStopped = true;
        }
        _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Attack);
    }

    public void OperateUpdate()
    {
       
    }

    public void OperateExit()
    {
       _controllerCore.enemyAnimationEventHandler.InitParameter();
    }
}
