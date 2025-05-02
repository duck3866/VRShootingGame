using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<EnemyControllerCore>
{
    private float _attackTime = 2f;
    private float _currentTime = 0f;
    private bool isAttacking = false;
    private float currentBollet = 0f;
    private float maxBollet = 5f;
    private bool isReloading = false;
    
    private EnemyControllerCore _controllerCore;
    public void Init(EnemyControllerCore controller)
    {
        _attackTime = 5f;
        _currentTime = 0f;
        currentBollet = maxBollet;
        _controllerCore = controller;
    }

    public void OperateEnter()
    {
        _controllerCore.animator.SetTrigger("toIdle");
    }

    public void OperateUpdate()
    {
        Debug.Log($"장탄 수:{currentBollet}");
        _controllerCore.transform.forward  = -_controllerCore.player.transform.forward;
        if (isReloading || isAttacking)
        {
            Debug.Log("장탄 이게 뭐지");
            return;
        }
        if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) > _controllerCore.enemyAbility.AttackDistance)
        {
            _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
        }
        else if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) <
                 _controllerCore.enemyAbility.AttackDistance)
        {
            if (currentBollet > 0)
            {
                if (_attackTime <= _currentTime)
                {
                    _controllerCore.StartCoroutine(AttackAction());
                    _currentTime = 0;
                }
                else
                {
                    _currentTime += Time.deltaTime;
                }
            }
            else
            {
                _controllerCore.StartCoroutine(Reloading());
            }
        }
    }

    public void OperateExit()
    {
     
    }

    private IEnumerator AttackAction()
    {
        _controllerCore.animator.SetTrigger("toAttack");
        isAttacking = true;
        currentBollet -= 1;
        Debug.Log("적 공격!");
        yield return new WaitForSeconds(1.2f);
        _controllerCore.animator.SetTrigger("toIdle");
        isAttacking = false;
    }

    private IEnumerator Reloading()
    {
        _controllerCore.animator.SetTrigger("toReload");
        isReloading = true;
        yield return new WaitForSeconds(3f);
        isReloading = false;
        currentBollet = maxBollet;
    }
}
