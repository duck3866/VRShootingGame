using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState<EnemyControllerCore>
{
    private EnemyControllerCore _controllerCore;
    public void Init(EnemyControllerCore controller)
    {
        _controllerCore = controller;
    }

    public void OperateEnter()
    {
       
    }

    public void OperateUpdate()
    {
       
    }

    public void OperateExit()
    {
        
    }
}
