using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider enemyHpEffectBar;
    [SerializeField] private Slider enemyHpBar; // 적의 체력 Slider
    [SerializeField] private EnemyControllerCore enemyControllerCore; // 적의 콘트롤러 클래스
    
    public IEnumerator EnemyHpEffect(float hp, float originalHp,float maxHp)
    {
        enemyHpBar.value = hp / maxHp;
        float currentTime = 0f;
        float finishTime = 1.5f;
        float elapsedRate = currentTime / finishTime;
        while (elapsedRate < 1)
        {
            currentTime += Time.unscaledDeltaTime;
            elapsedRate = currentTime / finishTime;
            enemyHpEffectBar.value = Mathf.Lerp(originalHp, hp, elapsedRate) / maxHp;
            yield return null;
        }

        enemyHpEffectBar.value = hp/maxHp;
    }
    private void LateUpdate()
    {
        Aiming();
    }

    public IEnumerator DieUIAction()
    {
        yield return new WaitForSecondsRealtime(3f);
        gameObject.SetActive(false);
    }
    private void Aiming()
    {
        Vector3 targetPosition = (enemyControllerCore.player.transform.position - enemyControllerCore.transform.position);
        // Vector3 direction = new Vector3(targetPosition.y, 0, 0);
        Quaternion rotation = Quaternion.LookRotation(targetPosition.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
}
