using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public bool isAttacking = false;
    public bool isReloading = false;
    private Collider[] _colliders = new Collider[10];
    [SerializeField] private LayerMask layerMask;
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

    public void InitParameter()
    {
        isAttacking = false;
        isReloading = false;
    }

    public void AttackChecking()
    {
        Vector3 startPosition = transform.forward + new Vector3(0f, 0f, 1f);
        Physics.OverlapSphereNonAlloc(startPosition, 1f, _colliders,layerMask);
        if (_colliders.Length > 0)
        {
            foreach (var collider in _colliders)
            {
                collider.TryGetComponent<IDamagable>(out IDamagable damagable);
                damagable.TakeDamage(5f);
            }
        }
        
    }
}
