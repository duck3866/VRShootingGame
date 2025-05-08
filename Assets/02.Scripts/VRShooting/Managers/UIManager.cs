using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("손에 뭐있는지 텍스트")]
    public GameObject rightHand;
    public TextMeshProUGUI  rightHandText;
    public TextMeshProUGUI  rightHandInfoText;
    public GameObject leftHand;
    public TextMeshProUGUI  leftHandText;
    public TextMeshProUGUI  leftHandInfoText;
    [Header("플레이어 정보 UI")]
    public GameObject playerStats;
    public Image PlayerHpImage;
    // public TextMeshProUGUI  playerHpText;
    [Header("보스 정보 UI")]
    public GameObject bossStats;

    public Slider bossHpSlider;
    public Slider bossHpEffectSlider;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        transform.localPosition = Vector3.zero;
        Init();
    }

    public void Init()
    {
        rightHand.transform.localPosition = new Vector3(0.23f, 0, 0.5f);
        rightHandInfoText.transform.localPosition = new Vector3(0, -0.3f, 0);
        leftHand.transform.localPosition = new Vector3(-0.23f, 0, 0.5f);
        leftHandInfoText.transform.localPosition = new Vector3(0, -0.3f, 0);
        playerStats.transform.localPosition = new Vector3(0, -0.33f, 0.6f);
        bossStats.transform.localPosition = new Vector3(0, 0.1f, 0.3f);
        bossStats.SetActive(false);
    }

    public void PlayerHPUpdate(float hp, float maxHp)
    {
        PlayerHpImage.fillAmount = hp/maxHp;
    }
    public void RightHandInfoUpdate(string text,string textInfo)
    {
        rightHandText.text = text;
        rightHandInfoText.text = textInfo;
    }
    public void LeftHandInfoUpdate(string text,string textInfo)
    {
        leftHandText.text = text;
        leftHandInfoText.text = textInfo;
    }

    public void BossAppears()
    {
        bossStats.SetActive(true);
    }

    public void BossHpUpdate(float hp, float originalHp,float maxHp)
    {
        bossHpSlider.value = hp/maxHp;
        StartCoroutine(BossHpEffect(hp, originalHp,maxHp));
    }

    private IEnumerator BossHpEffect(float hp, float originalHp,float maxHp)
    {
        float currentTime = 0f;
        float finishTime = 1.5f;
        float elapsedRate = currentTime / finishTime;
        while (elapsedRate < 1)
        {
            currentTime += Time.unscaledDeltaTime;
            elapsedRate = currentTime / finishTime;
            bossHpEffectSlider.value = Mathf.Lerp(originalHp, hp, elapsedRate) / maxHp;
            yield return null;
        }

        bossHpEffectSlider.value = hp/maxHp;
    }
}
