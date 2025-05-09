using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class EnemyColliderInfo
{
    // [Header("데미지 받을 콜라이더")]
    public string name;
    public Collider collider;
    // [Header("데미지 가중치")]
    public float weightDamage;
}
