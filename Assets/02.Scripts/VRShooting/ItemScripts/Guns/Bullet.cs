using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float aliveTime;
    private float _currentTime;
    private void OnEnable()
    {
        _currentTime = 0;
    }

    private void Update()
    {
        if (_currentTime >= aliveTime)
        {
           Destroy(gameObject);
        }
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        _currentTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(5f);
        }
    }
}
