using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class EnemyUI : MonoBehaviour
{
 
    [SerializeField] private Slider healthBar; // 적의 체력 Slider
    [SerializeField] private EnemyControllerCore enemyControllerCore; // 적의 콘트롤러 클래스
    private void LateUpdate()
    {
        if (enemyControllerCore == null) return;
        healthBar.value = enemyControllerCore.EnemyHp / enemyControllerCore.enemyAbility.MaxHp;
        Aiming();
    }
    private void Aiming()
    {
        Vector3 targetPosition = enemyControllerCore.player.transform.position - enemyControllerCore.transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
}
