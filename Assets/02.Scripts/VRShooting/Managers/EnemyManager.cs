using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : IManagable
{
    public float enemyCount = 0;
    public float bossCount = 0;
    public enum EnemyType
    {
        Light = 1,
        Heavy = 2,
        Champion = 3
    }
    // public EnemyAbility[] EnemyAbilities = new EnemyAbility[10];
    private GameObject[] _enemyObjects = new GameObject[5]; // 적 프리팹 리스트
    private GameObject[] _bossObjects = new GameObject[5]; // 보스 프리팹 리스트
    
    /// <summary>
    /// 
    /// </summary>
    public void Init()
    {
        enemyCount = 0;
        bossCount = 0;
        _enemyObjects = Resources.LoadAll<GameObject>("Enemy");
        _bossObjects = Resources.LoadAll<GameObject>("Boss");
        // EnemyAbilities = Resources.LoadAll<EnemyAbility>("Ability");
    }

    public void DieEnemy(bool isBoss)
    {
        if (isBoss)
        {
            bossCount += 1;
        }
        else
        {
            enemyCount += 1;
        }
    }
    public void SpawnEnemy(GameObject spawnPoint)
    {
        // enemyCount += 1;
        GameObject enemy = _enemyObjects[Random.Range(0, _enemyObjects.Length)];
        GameObject.Instantiate(enemy);
        enemy.transform.position = spawnPoint.transform.position;
    }

    public void SpawnBoss(GameObject spawnPoint)
    {
        // bossCount += 1;
        GameObject boss = _bossObjects[Random.Range(0, _bossObjects.Length)];
        UIManager.Instance.BossNameUpdate(boss.name);
        GameObject.Instantiate(boss);
        boss.transform.position = spawnPoint.transform.position;
    }
}
