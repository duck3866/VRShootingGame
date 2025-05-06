using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public static readonly EnemyManager EnemyManager = new();
   public GameObject[] SpawnPoints = new GameObject[4];

   public float turn = 0f;
      
   public float currentTime = 0f;
   public float createTime = 10f;
   public void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      InitializeManagers();
   }

   public void InitializeManagers()
   {
      EnemyManager.Init();
   }

   public void Update()
   {
      if (currentTime >= createTime)
      {
         GameObject SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
         EnemyManager.SpawnEnemy(SpawnPoint);
         currentTime = 0f;
      }
      else
      {
         currentTime += Time.deltaTime;
      }
   }
}
