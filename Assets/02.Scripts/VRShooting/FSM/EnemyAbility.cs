using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "EnemyAbility",fileName = "Ability", order = 0)]
public class EnemyAbility : ScriptableObject
{
    public float MoveSpeed;
    public float MaxHp;
    public float AttackDistance;
    public EnemyManager.EnemyType Type;
}
