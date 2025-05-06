using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private EnemyControllerCore enemyControllerCore;
    public Slider healthBar;
    private void Update()
    {
        if (enemyControllerCore == null) return;
        healthBar.value = enemyControllerCore.EnemyHp / enemyControllerCore.enemyAbility.MaxHp;
        Aiming();
    }
    private void Aiming()
    {
        Vector3 targetPosition = enemyControllerCore.player.transform.position - enemyControllerCore.transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetPosition);
        enemyControllerCore.transform.rotation = Quaternion.Slerp(enemyControllerCore.transform.rotation, rotation, Time.deltaTime * 5f);
    }
}
