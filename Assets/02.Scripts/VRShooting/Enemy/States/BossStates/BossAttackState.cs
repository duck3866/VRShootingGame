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
    private BossController _bossController;
    // 패턴별 쿨타임 상태 딕셔너리
    private Dictionary<BossPattern, bool> _patternCooldown = new Dictionary<BossPattern, bool>();

    public override void Init(EnemyControllerCore controller)
    {
        controllerCore = controller;
        _bossController = controllerCore as BossController;
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
        if (_bossController.agent.enabled)
        {
            _bossController.agent.isStopped = true;
        }
        //
        // if (!_bossController.iHaveGun)
        // {
        //     _bossController.AttackDistance = 2.5f;
        //     _bossController.animator.SetTrigger("toIdle");
        // }
        // else
        // {
            _bossController.animator.SetTrigger("toShootingIdle");
        //}
    }

    public override void OperateUpdate()
    {
        if (!_patternRunning)
        {
            Aiming();
            if (Vector3.Distance(_bossController.transform.position, _bossController.player.transform.position) >=
                _bossController.AttackDistance)
            {
                _bossController.ChangeState(EnemyControllerCore.EnemyState.Chase);
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
        _bossController.enemyAnimationEventHandler.InitParameter();
    }

    private IEnumerator Fire()
    {
        _patternRunning = true;
        _bossController.agent.isStopped = true;
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Fire");
        int bulletCount = 0;
        while (bulletCount < 8)
        {
            _bossController.InstancePrefab();
            bulletCount++;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(5f);

        EnterCooldown(BossPattern.Fire, 5f); // 예시로 쿨타임 5초
        _patternRunning = false;
        _bossController.agent.isStopped = false;
        yield return null;
    }

    private IEnumerator RecallEnemy()
    {
        _patternRunning = true;
        controllerCore.animator.SetTrigger("toSpwan");
        yield return new WaitForSeconds(1f);
        Debug.Log("Recall Enemy");
        GameManager.EnemyManager.SpawnEnemy(_bossController.gameObject);
        yield return new WaitForSeconds(3f);

        EnterCooldown(BossPattern.RecallEnemy, 10f); // 예시로 쿨타임 10초
        _patternRunning = false;

        yield return null;
    }

    private IEnumerator Rush()
    {
        _patternRunning = true;
        _bossController.animator.SetTrigger("toDashing");
        _bossController.isDashing = true; // 이거 왜 안됨
        // Debug.Log("거울 공격");

        Vector3 start = _bossController.transform.position;
        Vector3 end = start + _bossController.transform.forward * _dashDistance;
        float elapsed = 0f;

        while (elapsed < _dashDuration)
        {
            _bossController.transform.position = Vector3.Lerp(start, end, elapsed / _dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _bossController.transform.position = end;

        EnterCooldown(BossPattern.Dash, 10f); // 예시로 쿨타임 15초
        _patternRunning = false;
        _bossController.isDashing = false; 
        _bossController.animator.SetTrigger("toShootingIdle");
        yield return null;
    }

    private void EnterCooldown(BossPattern pattern, float duration)
    {
        _patternCooldown[pattern] = true;
        _bossController.StartCoroutine(CooldownRoutine(pattern, duration));
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
            _bossController.StartCoroutine(WaitAndRetryPattern(10f));
            return;
        }
        _currentBossPattern = available[Random.Range(0, available.Count)];
        switch (_currentBossPattern)
        {
            case BossPattern.Dash:
                _bossController.StartCoroutine(Rush());
                break;
            case BossPattern.RecallEnemy:
                _bossController.StartCoroutine(RecallEnemy());
                break;
            case BossPattern.Fire:
                _bossController.StartCoroutine(Fire());
                break;
        }
    }

    private IEnumerator WaitAndRetryPattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangePattern();
    }
}
