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
    public TextMeshProUGUI  bossNameText;
    [Header("점수 정보 UI")] 
    public GameObject UIGameObject;
    public GameObject pointText;
   
    public Queue<GameObject> pointQueue = new Queue<GameObject>();

    [Header("이펙트UI")]
    // public GameObject EffectSlider;
    // public Slider EffectSliderBar;
    public Image playerImage;
    public Image effectUI;
    public Color timeStopColor;
    public Color hitColor;

    [Header("GameOver UI")] 
    public GameObject gameOverUI;
    public TextMeshProUGUI  gameOverText;
    public Slider timeValueSlider;
    // private Animator animator;
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
    }

    public void Start()
    {
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
        UIGameObject.transform.localPosition = new Vector3(0, 0, 0.3f); // -0.3f
        bossNameText.transform.localPosition = new Vector3(0, 1.6f, 0.3f);
        effectUI.transform.localPosition = new Vector3(0, 0, 0.3f);
        playerImage.transform.localPosition = new Vector3(0, -0.04f, 0.15f);
        effectUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
       
        pointQueue.Clear();
        bossStats.SetActive(false);
        timeValueSlider.transform.localPosition = new Vector3(-0.05f, -0.16f, 0.3f);
        
        GameManager.Instance.GameOverEvent += GameOverUIUpdate;
    }

    public void EffectSliderUpdate(float value, float maxValue)
    {
        timeValueSlider.value = value/maxValue;
    }
    public void EffectUIUpdate(bool active,bool isHit)
    {
        effectUI.color = isHit ? hitColor : timeStopColor;
        effectUI.gameObject.SetActive(active);
    }
    
    public void GameOverUIUpdate()
    {
        gameOverUI.gameObject.SetActive(true);
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        gameOverUI.transform.position = new Vector3(player.position.x,3f, player.position.z);
        GameInfo gameInfo = GameManager.Instance.ReturnGamePoint();
        gameOverText.text = $"Game Over \n게임 점수: {gameInfo.GamePoint}\n처치한 적의 수: {gameInfo.EnemyCount}\n처치한 보스의 수: {gameInfo.BossCount}"; 
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
    /// 점수 텍스트를 추가하는 함수
    /// </summary>
    /// <param name="text">추가할 텍스트 내용</param>
    public void AddPointText(string text)
    {
        GameObject pointMeshText = Instantiate(pointText.gameObject,UIGameObject.transform);
        pointMeshText.GetComponentInChildren<TextMeshProUGUI>().text = text;
        
        pointQueue.Enqueue(pointMeshText);
        if (pointQueue.Count > 6)
        {
            GameObject temp = pointQueue.Dequeue();
            // temp.GetComponentInChildren<Animator>().SetTrigger("toDelete");
            Destroy(temp);
        }
    }
    /// <summary>
    /// 보스 UI를 활성화/비활성화하는 함수
    /// </summary>
    public IEnumerator BossUIAppears(bool active, float duration)
    {
        yield return new WaitForSeconds(duration);
        bossStats.SetActive(active);
    }
    /// <summary>
    /// 보스 이름 UI를 업데이트 하는 함수
    /// </summary>
    /// <param name="text">변경할 이름</param>
    public void BossNameUpdate(string text)
    {
        bossNameText.text = text;
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
