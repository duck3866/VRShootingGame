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

    public EnemyAbility[] EnemyAbilities = new EnemyAbility[10];

    public void Init()
    {
        EnemyAbilities = Resources.LoadAll<EnemyAbility>("Ability");
        foreach (var VARIABLE in EnemyAbilities)
        {
            Debug.Log(VARIABLE);
        }
    }
}
