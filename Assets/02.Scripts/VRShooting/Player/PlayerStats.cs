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
        if (PlayerHp <= 0)
        {
            GameManager.Instance.PlayerGameOver();
        }
        UIManager.Instance.PlayerHPUpdate(PlayerHp, maxHp);
        UIManager.Instance.EffectUIUpdate(true,true);
        StartCoroutine(WaitForDamage());
    }

    private IEnumerator WaitForDamage()
    {
        yield return new WaitForSecondsRealtime(1f);
        UIManager.Instance.EffectUIUpdate(false,true);
    }
    public void HitPoint(Vector3 hitPoint)
    {
        
    }
}
