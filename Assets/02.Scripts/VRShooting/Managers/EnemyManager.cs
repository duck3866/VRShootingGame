using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : IManagable
{
    public enum EnemyType
    {
        Light,
        Heavy,
        Champion
    }
    // public EnemyAbility[] EnemyAbilities = new EnemyAbility[10];
    public GameObject[] EnemyObjects = new GameObject[5];
    
    public void Init()
    {
        EnemyObjects = Resources.LoadAll<GameObject>("Enemys");
        // EnemyAbilities = Resources.LoadAll<EnemyAbility>("Ability");
    }

    public void SpawnEnemy(GameObject spawnPoint)
    {
        GameObject enemy = EnemyObjects[Random.Range(0, EnemyObjects.Length)];
        GameObject.Instantiate(enemy);
        enemy.transform.position = spawnPoint.transform.position;
    }

    public void SpawnBoss()
    {
        
    }
}
