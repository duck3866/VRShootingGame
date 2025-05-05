using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamagable
{
    public float maxHp;
    private float PlayerHp { get; set; }

    private void Start()
    {
        PlayerHp = maxHp;
    }
    public void TakeDamage(float damage)
    {
        PlayerHp -= damage;
    }

    public void HitPoint(Vector3 hitPoint)
    {
        
    }
}
