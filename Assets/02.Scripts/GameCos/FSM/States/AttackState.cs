using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<EnemyControllerCore>
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
        if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) > _controllerCore.enemyAbility.AttackDistance)
        {
            _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
        }
    }

    public void OperateExit()
    {
     
    }
}
