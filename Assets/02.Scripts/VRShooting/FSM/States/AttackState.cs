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
        if (!_controllerCore.agent.isStopped)
        {
            _controllerCore.agent.isStopped = true;
        }
        if (!_controllerCore.iHaveGun)
        {
            _controllerCore.AttackDistance = 2.5f;
            _controllerCore.animator.SetTrigger("toIdle");
        }
        else
        {
            _controllerCore.animator.SetTrigger("toShootingIdle");
        }
    }

    public void OperateUpdate()
    {
        isAttacking = _controllerCore.enemyAnimationEventHandler.isAttacking;
        isReloading = _controllerCore.enemyAnimationEventHandler.isReloading;
        if (isReloading || isAttacking) return;
        Aiming();
        if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) > _controllerCore.AttackDistance)
        {
            _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
        }
        else if (Vector3.Distance(_controllerCore.transform.position, _controllerCore.player.transform.position) <
                 _controllerCore.AttackDistance)
        {
            if (_controllerCore.iHaveGun)
            {
                if (currentBollet > 0)
                {
                    if (_attackTime <= _currentTime)
                    {
                        if (_controllerCore.iHaveGun)
                        {
                            _controllerCore.StartCoroutine(ShootAction());
                            _currentTime = 0; 
                        }
                    }
                    else
                    {
                        _currentTime += Time.deltaTime;
                    }
                }
                else
                {
                    if (_controllerCore.iHaveGun)
                    {
                        _controllerCore.StartCoroutine(Reloading());
                    }
                }   
            }
            else
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
            }
        }
    }

    public void OperateExit()
    {
        _controllerCore.enemyAnimationEventHandler.InitParameter();
    }

    private void Aiming()
    {
        Vector3 targetPosition = _controllerCore.player.transform.position - _controllerCore.transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetPosition);
        _controllerCore.transform.rotation = Quaternion.Slerp(_controllerCore.transform.rotation, rotation, Time.deltaTime * 5f);
    }
    private IEnumerator AttackAction()
    {
        Debug.Log("공격 액션 실행");
        _controllerCore.animator.SetTrigger("toAttack");
        // isAttacking = true;
        yield return null;
        // _controllerCore.animator.SetTrigger("toIdle");
        // isAttacking = false;
    }
    private IEnumerator ShootAction()
    {
        _controllerCore.animator.SetTrigger("toShoot");
        // isAttacking = true;
        currentBollet -= 1;
        Debug.Log("적 공격!");
        yield return null;
        // _controllerCore.animator.SetTrigger("toShootingIdle");
        // isAttacking = false;
    }

    private IEnumerator Reloading()
    {
        _controllerCore.animator.SetTrigger("toReload");
        // isReloading = true;
        yield return null;
        // isReloading = false;
        currentBollet = maxBollet;
    }
}
