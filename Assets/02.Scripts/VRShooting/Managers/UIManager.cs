using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // 싱글톤 객체
    [Header("손에 뭐있는지 텍스트")]
    [Header("오른손")]
    public GameObject rightHand;
    public TextMeshProUGUI  rightHandText;
    public TextMeshProUGUI  rightHandInfoText;
    [Header("왼손")]
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
    /// <summary>
    /// UI 정보를 초기화 하는 함수
    /// </summary>
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
    /// <summary>
    /// 플레이어 체력 UI 변경 함수
    /// </summary>
    /// <param name="hp">플레이어의 현재 체력</param>
    /// <param name="maxHp">플레이어의 최대체력</param>
    public void PlayerHPUpdate(float hp, float maxHp)
    {
        PlayerHpImage.fillAmount = hp/maxHp;
    }
    /// <summary>
    /// 오른손이 오브젝트를 잡았을때/놓았을때 정보 변경 함수
    /// </summary>
    /// <param name="text">잡은 오브젝트 이름</param>
    /// <param name="textInfo">잡은 오브젝트의 정보(총알, 상태 등)</param>
    public void RightHandInfoUpdate(string text,string textInfo)
    {
        rightHandText.text = text;
        rightHandInfoText.text = textInfo;
    }
    /// <summary>
    /// 왼손이 오브젝트를 잡았을때/놓았을때 정보 변경 함수
    /// </summary>
    /// <param name="text">잡은 오브젝트 이름</param>
    /// <param name="textInfo">잡은 오브젝트의 정보(총알, 상태 등)</param>
    public void LeftHandInfoUpdate(string text,string textInfo)
    {
        leftHandText.text = text;
        leftHandInfoText.text = textInfo;
    }
    /// <summary>
    /// 보스 UI를 활성화하는 함수
    /// </summary>
    public void BossUIAppears()
    {
        bossStats.SetActive(true);
    }
    /// <summary>
    /// 보스 체력 UI를 변경하는 함수
    /// </summary>
    /// <param name="hp">보스 감소된 체력</param>
    /// <param name="originalHp">보스 감소되기 전 체력</param>
    /// <param name="maxHp">보스 최대 체력</param>
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
