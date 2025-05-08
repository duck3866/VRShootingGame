using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance; // 싱글톤 객체
   public static readonly EnemyManager EnemyManager = new(); // EnemyManager 클래스
   [Header("적 스폰포인트 리스트")]
   public GameObject[] SpawnPoints = new GameObject[4]; // EnemyManager에 전달할 스폰 포인트 리스트
   [Header("현재 턴 수")]
   public float turn = 0f; // 현재 턴 수
   [Header("보스 등장 턴 수")]
   public float bossTurn = 5f; // 보스가 등장 할 턴 수
   [Header("적 생성 시간")]
   public float createTime = 10f; // 적 생성할 시간
   [Header("보스")]
   public bool bossSpawned = false; // 보스가 소환됐는지 여부 
   public GameObject bossSpawnPoint;
   private float _currentTime = 0f; // 현재 생성 누적 시간
   public void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      InitializeManagers();
   }
   /// <summary>
   /// 매니저들 초기화 함수
   /// </summary>
   public void InitializeManagers()
   {
      EnemyManager.Init();
   }
   /// <summary>
   /// 보스 클리어 시 호출되는 함수
   /// </summary>
   public void BossClear()
   {
      Debug.Log("BossClear");
      bossSpawned = false;
      bossTurn += 5;
   }
   public void Update()
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
               UIManager.Instance.BossUIAppears();
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
