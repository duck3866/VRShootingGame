using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<EnemyControllerCore>
{
    protected float AttackTime = 2f;
    protected float CurrentTime = 0f;
    protected bool IsAttacking = false;
    protected float CurrentBullet = 0f;
    protected float MaxBullet = 5f;
    protected bool IsReloading = false;
    
    public EnemyControllerCore controllerCore;
    public void Init(EnemyControllerCore controller)
    {
        AttackTime = 5f;
        CurrentTime = 0f;
        CurrentBullet = MaxBullet;
        controllerCore = controller;
    }

    public void OperateEnter()
    {
        if (controllerCore.agent.enabled)
        {
            controllerCore.agent.isStopped = true;
        }
        if (!controllerCore.iHaveGun)
        {
            controllerCore.AttackDistance = 2.5f;
            controllerCore.animator.SetTrigger("toIdle");
        }
        else
        {
            controllerCore.animator.SetTrigger("toShootingIdle");
        }
    }

    public void OperateUpdate()
    {
        IsAttacking = controllerCore.enemyAnimationEventHandler.isAttacking;
        IsReloading = controllerCore.enemyAnimationEventHandler.isReloading;
        if (IsReloading || IsAttacking) return;
        Aiming();
        if (Vector3.Distance(controllerCore.transform.position, controllerCore.player.transform.position) > controllerCore.AttackDistance)
        {
            controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
        }
        else if (Vector3.Distance(controllerCore.transform.position, controllerCore.player.transform.position) <
                 controllerCore.AttackDistance)
        {
            if (controllerCore.iHaveGun)
            {
                if (CurrentBullet > 0)
                {
                    if (AttackTime <= CurrentTime)
                    {
                        if (controllerCore.iHaveGun)
                        {
                            controllerCore.StartCoroutine(ShootAction());
                            CurrentTime = 0; 
                        }
                    }
                    else
                    {
                        CurrentTime += Time.deltaTime;
                    }
                }
                else
                {
                    if (controllerCore.iHaveGun)
                    {
                        controllerCore.StartCoroutine(Reloading());
                    }
                }   
            }
            else
            {
                if (CurrentBullet > 0)
                {
                    if (AttackTime <= CurrentTime)
                    {
                        controllerCore.StartCoroutine(AttackAction());
                        CurrentTime = 0;
                    }
                    else
                    {
                        CurrentTime += Time.deltaTime;
                    }
                }
            }
        }
    }

    public void OperateExit()
    {
        controllerCore.enemyAnimationEventHandler.InitParameter();
    }

    private void Aiming()
    {
        Vector3 targetPosition = controllerCore.player.transform.position - controllerCore.transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetPosition);
        controllerCore.transform.rotation = Quaternion.Slerp(controllerCore.transform.rotation, rotation, Time.deltaTime * 5f);
    }
    private IEnumerator AttackAction()
    {
        Debug.Log("공격 액션 실행");
        controllerCore.animator.SetTrigger("toAttack");
        // IsAttacking = true;
        yield return null;
        // controllerCore.animator.SetTrigger("toIdle");
        // IsAttacking = false;
    }
    private IEnumerator ShootAction()
    {
        controllerCore.animator.SetTrigger("toShoot");
        // IsAttacking = true;
        CurrentBullet -= 1;
        Debug.Log("적 공격!");
        yield return null;
        // controllerCore.animator.SetTrigger("toShootingIdle");
        // IsAttacking = false;
    }

    private IEnumerator Reloading()
    {
        controllerCore.animator.SetTrigger("toReload");
        // IsReloading = true;
        yield return null;
        // IsReloading = false;
        CurrentBullet = MaxBullet;
    }
}
