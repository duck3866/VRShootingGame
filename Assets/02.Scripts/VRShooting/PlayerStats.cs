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
        UIManager.Instance.PlayerHPUpdate(PlayerHp, maxHp);
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage");
        PlayerHp -= damage;
        UIManager.Instance.PlayerHPUpdate(PlayerHp, maxHp);
    }

    public void HitPoint(Vector3 hitPoint)
    {
        
    }
}
