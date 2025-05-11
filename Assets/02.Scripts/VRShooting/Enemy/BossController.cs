using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyControllerCore
{
   public override void Start()
   {
      IsTharwing = false;
      _enemyAnimationSoundEventHandler = GetComponent<EnemyAnimationSoundEventHandler>();
      OriginalMaterials = skinnedMeshRenderer.materials;
      enemyAnimationEventHandler = GetComponent<EnemyAnimationEventHandler>();
      CanInteractablePoint = GetComponentInChildren<CanInteractablePoint>();
      agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
      animator = GetComponent<Animator>();
      

      // fixedJoint = GetComponent<FixedJoint>();
      if (enemyAbility != null)
      {
         Debug.Log("적 Ability 초기화");
         Initialized(enemyAbility);
      }

      Rigidbodies = GetComponentsInChildren<Rigidbody>();
      OffCharacterJoint();
      CanInteractablePoint[] interactablePoints = GetComponentsInChildren<CanInteractablePoint>();
      foreach (var point in interactablePoints)
      {
         point.Init(this);
      }
      UIManager.Instance.BossHpUpdate(EnemyHp,EnemyHp,enemyAbility.MaxHp);
   }
   public override void TakeDamage(float damage)
   {
      // Debug.Log("Boss TakeDamage");
      if (!isDie)
      {
         UIManager.Instance.BossHpUpdate(EnemyHp - (damage * PowerDamage),EnemyHp,enemyAbility.MaxHp);
         EnemyHp -= (damage * PowerDamage);
      
         if (EnemyHp > 0)
         {
            StartCoroutine(WaitForDamaged());
         }
         else
         {
            GameManager.Instance.BossClear();
            ChangeState(EnemyState.Die);
         }  
      }
   }
}
