using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public bool isDamaged = false;
    public bool isAttacking = false;
    public bool isReloading = false;
    private Collider[] _colliders = new Collider[10];
    [SerializeField] private LayerMask layerMask;
    private EnemyControllerCore _enemyControllerCore;

    private void Start()
    {
        _enemyControllerCore = GetComponent<EnemyControllerCore>();
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void StartReloading()
    {
        isReloading = true;
    }

    public void EndReloading()
    {
        isReloading = false;
    }

    public void StartDamaged()
    {
        isDamaged = true;
    }

    public void EndDamaged()
    {
        isDamaged = false;
    }
    public void InitParameter()
    {
        isDamaged = false;
        isAttacking = false;
        isReloading = false;
    }

    public void AttackChecking()
    {
        Vector3 startPosition = transform.forward + new Vector3(0f, 0f, 1f);
        _colliders = Physics.OverlapSphere(startPosition, 1f,layerMask);
        if (_colliders.Length > 0)
        {
            foreach (var collider in _colliders)
            {
                Debug.Log(collider.name);
                IDamagable damagable = collider.GetComponentInParent<IDamagable>();
                if (damagable != null)
                {
                    Debug.Log("주먹 공격 실행");
                    damagable.TakeDamage(5f);
                }
            }
        }
    }

    public void ShootInstancePrefab()
    {
        _enemyControllerCore.InstancePrefab();
    }
}
