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
        throw new System.NotImplementedException();
    }

    public void OperateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OperateExit()
    {
        throw new System.NotImplementedException();
    }
}
