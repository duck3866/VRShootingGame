using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    enum DroneState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }
    
    private DroneState state = DroneState.Idle;
    public float idleDelayTime = 2f;
    private float currentTime;
    public float moveSpeed = 1f;
    private Transform tower;
    private NavMeshAgent agent;
    public float attackRange = 3f;
    public float attackDelayTime = 2f;
    [SerializeField] private int hp = 3;
    private Transform explosion;
    private ParticleSystem expEffect;
    private AudioSource expAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("Tower").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.speed = moveSpeed;

        explosion = GameObject.Find("Explosion").transform;
        expEffect = explosion.GetComponent<ParticleSystem>();
        expAudio = expEffect.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Current State: {state}");
        switch (state)
        {
            case DroneState.Idle:
                Idle();
                break;
            case DroneState.Move:
                Move();
                break;
            case DroneState.Attack:
                Attack();
                break;
            case DroneState.Damage:
                // Damage();
                // break;
            case DroneState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        currentTime += Time.deltaTime;
        if (currentTime > idleDelayTime)
        {
            state = DroneState.Move;
            agent.enabled = true;
        }
    }

    private void Move()
    {
        if (!Tower.Instance) return;
        agent.SetDestination(tower.position);
        if (Vector3.Distance(transform.position, tower.position) < attackRange)
        {
            state = DroneState.Attack;
            agent.enabled = false;
        }
    }
    
    private void Attack()
    {
        currentTime += Time.deltaTime;
        if (currentTime > attackDelayTime)
        {
            Tower.Instance.HP--;
            currentTime = 0f;
        }
    }

    public void OnDamageProcess()
    {
        hp--;
        if (hp > 0)
        {
            state = DroneState.Damage;
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
        else
        {
            explosion.position = transform.position;
            expEffect.Play();
            expAudio.Play();
            Destroy(gameObject);
        }
    }

    IEnumerator Damage()
    {
        agent.enabled = false;
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        Color originalColor = mat.color;
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mat.color = originalColor;
        state = DroneState.Idle;
        currentTime = 0f;
    }

    private void Die()
    {
        
    }
}
