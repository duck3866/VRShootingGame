using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance; // 싱글톤 객체
   public static readonly EnemyManager EnemyManager = new(); // EnemyManager 클래스
   public static readonly AudioManager AudioManager = new();
   [Header("적 스폰포인트 리스트")]
   public GameObject[] SpawnPoints = new GameObject[4]; // EnemyManager에 전달할 스폰 포인트 리스트
   [Header("현재 턴 수")]
   public float turn = 0f; // 현재 턴 수
   [Header("보스 등장 턴 수")]
   public float bossTurn = 5f; // 보스가 등장 할 턴 수
   public bool timeStop = false;
   public float GamePoint { get; private set; }

   [Header("적 생성 시간")]
   public float createTime = 10f; // 적 생성할 시간
   [Header("보스")]
   public bool bossSpawned = false; // 보스가 소환됐는지 여부 
   public GameObject bossSpawnPoint;
   private float _currentTime = 0f; // 현재 생성 누적 시간
   [SerializeField,Header("BGM")]
   private AudioClip bgmClip;
   private AudioSource _bgmSource;
   [SerializeField] private float audioVolume;
   public bool GameOver { get; private set; }
   [Header("GameOver Sound")]
   public AudioClip gameOverBGM;
   public event Action GameOverEvent;
   public event Action TimeStopStartEvent;
   public event Action TimeStopEndEvent;
   public AudioClip itemSpawnSFX;
   [Range(0,1)]
   public float itemSpawnSFXVolume;
   [Header("테스트 씬")] public bool isTest;
   public GameInfo GameInfo;
   public void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      InitializeManagers();
      GameInfo = new GameInfo();
   } 
   /// <summary>
   /// 매니저들 초기화 함수
   /// </summary>
   public void InitializeManagers()
   {
      _bgmSource = GetComponent<AudioSource>();
      
      turn = 0f;
      GamePoint = 0f;
      EnemyManager.Init();
      AudioManager.Init();
      AudioManager.SetBGMSource(_bgmSource);
      AudioManager.SetBGMVolumeRatio(audioVolume);
      AudioManager.PlayBackgroundMusic(bgmClip);
   }
   /// <summary>
   /// 보스 클리어 시 호출되는 함수
   /// </summary>
   public void BossClear()
   {
      Debug.Log("BossClear");
      EnemyManager.DieEnemy(true);
      StartCoroutine(UIManager.Instance.BossUIAppears(false,3f));
      bossSpawned = false;
      bossTurn += 5;
   }

   public void AddPoint(float addPoint)
   {
      GamePoint += addPoint;
   }
   public void Update()
   {
      if (!isTest)
      {
         if (!GameOver)
         {
            if (!bossSpawned)
            {
               if (_currentTime >= createTime)
               {
                  if (turn < bossTurn)
                  {
                     GameObject SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                     EnemyManager.SpawnEnemy(SpawnPoint);
                     turn += 1;
                     _currentTime = 0f;
                  }
                  else
                  {
                     EnemyManager.SpawnBoss(bossSpawnPoint);
                     StartCoroutine(UIManager.Instance.BossUIAppears(true, 0f));
                     bossSpawned = true;
                     _currentTime = 0f;
                  }
               }
               else
               {
                  _currentTime += Time.deltaTime;
               }
            }
         }
      }
   }

   public void TimeStop(bool timeValue)
   {
      if (!GameOver)
      {
         if (timeValue)
         {
            Time.timeScale = 0.1f;
            timeStop = true;
            TimeStopStartEvent?.Invoke();
            UIManager.Instance.EffectUIUpdate(true, false);
            AudioManager.SetBGMPitch(0.5f);
         }
         else
         {
            Time.timeScale = 1f;
            timeStop = false;
            TimeStopEndEvent?.Invoke();
            UIManager.Instance.EffectUIUpdate(false, false);
            AudioManager.SetBGMPitch(1f);
         }
      }
   }

   public void AddEvent(Action eventArgs,bool timeValue)
   {
      if (timeValue)
      {
         TimeStopStartEvent += eventArgs;
      }
      else
      {
         TimeStopEndEvent += eventArgs;
      }
   }

   public void SpawnItem(string path, GameObject spawnPoint)
   {
      GameObject itemPrefab = Resources.Load<GameObject>(path);
      GameObject item = Instantiate(itemPrefab);
      AudioManager.PlaySoundEffect(itemSpawnSFX,spawnPoint.transform.position,itemSpawnSFXVolume);
      item.transform.position = spawnPoint.transform.position;
   }
   public void PlayerGameOver()
   {
      GameOver = true;
      GameOverEvent?.Invoke();
      StartCoroutine(UIManager.Instance.BossUIAppears(false, 0));
      AudioManager.SetBGMState(true);
      AudioManager.PlaySoundEffect(gameOverBGM,transform.position,2.5f);
      GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (GameObject enemy in enemyObjects)
      {
         Destroy(enemy);
      }
   }

   public GameInfo ReturnGamePoint()
   {
      if (GameInfo != null)
      {
         GameInfo.EnemyCount = EnemyManager.enemyCount;
         GameInfo.BossCount = EnemyManager.bossCount;
         GameInfo.GamePoint = this.GamePoint;
      }
      else
      {
         GameInfo = new GameInfo();
      }
      return GameInfo;
   }
 
}

public class GameInfo
{
   public float GamePoint;
   public float EnemyCount;
   public float BossCount;
}
