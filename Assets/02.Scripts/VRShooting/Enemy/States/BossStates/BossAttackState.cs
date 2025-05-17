using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAttackState : AttackState
{
    public enum BossPattern
    {
        Dash,
        RecallEnemy,
        Fire
    }

    private BossPattern _currentBossPattern;
    private bool _patternRunning = false;

    private float _dashDistance = 15f;
    private float _dashDuration = 1f;

    // 패턴별 쿨타임 상태 딕셔너리
    private Dictionary<BossPattern, bool> _patternCooldown = new Dictionary<BossPattern, bool>();

    public override void Init(EnemyControllerCore controller)
    {
        controllerCore = controller;
        AttackTime = 5f;
        CurrentTime = 0f;
        CurrentBullet = MaxBullet;
        _patternRunning = false;

        // 초기화 (쿨타임이 아님 = false)
        foreach (BossPattern pattern in System.Enum.GetValues(typeof(BossPattern)))
        {
            Debug.Log(pattern+" ???");
            _patternCooldown[pattern] = false;
        }
    }

    public override void OperateEnter()
    {
        Debug.Log("보스 공격 입장");
        if (controllerCore.agent.enabled)
        {
            controllerCore.agent.isStopped = true;
        }

        if (!controllerCore.iHaveGun)
        {
            controllerCore.AttackDistance = 2.5f;
            controllerCore.animator.SetTrigger("toIdle");
        }
        else
        {
            controllerCore.animator.SetTrigger("toShootingIdle");
        }
    }

    public override void OperateUpdate()
    {
        if (!_patternRunning)
        {
            Aiming();
            if (Vector3.Distance(controllerCore.transform.position, controllerCore.player.transform.position) >=
                controllerCore.AttackDistance)
            {
                controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
            }
            else
            {
                ChangePattern();
            }
        }
    }

    public override void OperateExit()
    {
        Debug.Log("보스 공격 퇴장");
        controllerCore.enemyAnimationEventHandler.InitParameter();
    }

    private IEnumerator Fire()
    {
        _patternRunning = true;
        controllerCore.agent.isStopped = true;
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Fire");
        int bulletCount = 0;
        while (bulletCount < 8)
        {
            controllerCore.InstancePrefab();
            bulletCount++;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(5f);

        EnterCooldown(BossPattern.Fire, 5f); // 예시로 쿨타임 5초
        _patternRunning = false;
        controllerCore.agent.isStopped = false;
        yield return null;
    }

    private IEnumerator RecallEnemy()
    {
        _patternRunning = true;
        Debug.Log("Recall Enemy");
        GameManager.EnemyManager.SpawnEnemy(controllerCore.gameObject);
        yield return new WaitForSeconds(3f);

        EnterCooldown(BossPattern.RecallEnemy, 10f); // 예시로 쿨타임 10초
        _patternRunning = false;

        yield return null;
    }

    private IEnumerator Rush()
    {
        _patternRunning = true;
        // Debug.Log("거울 공격");

        Vector3 start = controllerCore.transform.position;
        Vector3 end = start + controllerCore.transform.forward * _dashDistance;
        float elapsed = 0f;

        while (elapsed < _dashDuration)
        {
            controllerCore.transform.position = Vector3.Lerp(start, end, elapsed / _dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        controllerCore.transform.position = end;

        EnterCooldown(BossPattern.Dash, 10f); // 예시로 쿨타임 15초
        _patternRunning = false;

        yield return null;
    }

    private void EnterCooldown(BossPattern pattern, float duration)
    {
        _patternCooldown[pattern] = true;
        controllerCore.StartCoroutine(CooldownRoutine(pattern, duration));
    }

    private IEnumerator CooldownRoutine(BossPattern pattern, float time)
    {
        yield return new WaitForSeconds(time);
        _patternCooldown[pattern] = false;
    }

    private void ChangePattern()
    {
        if (_patternRunning)
        {
            return;
        }   
        AimingNow();
        List<BossPattern> available = _patternCooldown
            .Where(pair => !pair.Value)
            .Select(pair => pair.Key)
            .ToList();
        Debug.Log($"상태 전환 호출 : {available.Count}");
        if (available.Count <= 0)
        {
            // 모든 패턴이 쿨타임이면 조금 기다린 후 다시 시도
            // Debug.Log("이거 왜 안됨");
            controllerCore.StartCoroutine(WaitAndRetryPattern(10f));
            return;
        }
        _currentBossPattern = available[Random.Range(0, available.Count)];
        switch (_currentBossPattern)
        {
            case BossPattern.Dash:
                controllerCore.StartCoroutine(Rush());
                break;
            case BossPattern.RecallEnemy:
                controllerCore.StartCoroutine(RecallEnemy());
                break;
            case BossPattern.Fire:
                controllerCore.StartCoroutine(Fire());
                break;
        }
    }

    private IEnumerator WaitAndRetryPattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangePattern();
    }
}
