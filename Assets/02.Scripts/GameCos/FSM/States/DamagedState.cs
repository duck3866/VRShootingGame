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
        _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
    }

    public void OperateUpdate()
    {
       
    }

    public void OperateExit()
    {
       
    }
}
