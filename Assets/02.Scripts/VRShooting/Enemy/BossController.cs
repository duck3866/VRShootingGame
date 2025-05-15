using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyControllerCore
{
   public LineRenderer lineRenderer;
   public override void Start()
   {
      IsTharwing = false;
      lineRenderer = GetComponentInChildren<LineRenderer>();
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
   public override void Initialized(EnemyAbility ability)
   {
      enemyAbility = ability;
      EnemyHp = ability.MaxHp;
      agent.speed = ability.MoveSpeed;
      this.AttackDistance = ability.AttackDistance;
      player = GameObject.FindWithTag("Player");

      IState<EnemyControllerCore> chase = new ChaseState();
      IState<EnemyControllerCore> attack = new BossAttackState();
      IState<EnemyControllerCore> damaged = new DamagedState();
      IState<EnemyControllerCore> die = new DieState();
      IState<EnemyControllerCore> grabbing = new GrabbingState();

      _states.Add(EnemyState.Chase, chase);
      _states.Add(EnemyState.Attack, attack);
      _states.Add(EnemyState.Damaged, damaged);
      _states.Add(EnemyState.Die, die);
      _states.Add(EnemyState.Grabbing, grabbing);
      foreach (var state in _states.Values)
      {
         state.Init(this);
      }

      ChangeState(EnemyState.Chase);
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
   public override void OnTriggerEnter(Collider other)
       {
           // Debug.Log($"뭔가 닿았는데 이거 뭐임: {other.gameObject.name}");
           if (IsTharwing)
           {
               // Debug.Log("바닥");
               if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
               {
                   IDamagable damagable = other.GetComponentInChildren<IDamagable>();
                   if (damagable != null)
                   {
                       UIManager.Instance.AddPointText($"+적 충돌 {50f}");
                       GameManager.Instance.AddPoint(50f);
                       damagable.TakeDamage(10f);
                   }
                   UIManager.Instance.AddPointText($"+적 던져짐 {50f}");
                   GameManager.Instance.AddPoint(50f);
                   _enemyAnimationSoundEventHandler.ThrowingSoundPlay();
                   TakeDamage(5f);
               }
           }
       }
}
