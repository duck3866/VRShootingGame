using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public static readonly EnemyManager EnemyManager = new();
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
}
