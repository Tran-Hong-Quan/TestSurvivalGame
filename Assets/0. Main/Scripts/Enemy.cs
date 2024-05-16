using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IHealth
{
    [Header("Health")]
    [SerializeField] private HealthTeamSide healthTeamSide;
    [SerializeField] private float maxHealth;

    [Header("Attack")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private Transform target;
    [SerializeField] private Bullet bullet;
    [SerializeField] private float bulletDamage = 10;
    [SerializeField] private Transform firePos;

    [SerializeField] private float rotateSpeed = 20;

    private Animator animator;
    private NavMeshAgent agent;

    //Health
    private float currentHealth;
    public GameObject GameObject => gameObject;
    public HealthTeamSide HealthTeamSide => healthTeamSide;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private int aimIDAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        maxHealth = currentHealth;
        sprRange = agent.stoppingDistance * agent.stoppingDistance;

        aimIDAttack = Animator.StringToHash("Attack");

        agent.updateRotation = false;
    }

    public void TakeDamge(float damage, HealthEventHandler evt)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            print("Die");
            onDie?.Invoke(this);
        }
    }

    public void Regeneration(float regeneration, HealthEventHandler evt)
    {
        currentHealth += regeneration;
    }

    float sprRange;
    private void Update()
    {
        SetDesitination(target.position);

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0,
                Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y, rotateSpeed * Time.deltaTime),
                0);
        }

        if ((target.position - transform.position).sqrMagnitude < sprRange)
        {
            animator.SetBool(aimIDAttack, true);
        }
        else
        {
            animator.SetBool(aimIDAttack, false);
        }

        //print(agent.isStopped);
    }

    private void SetDesitination(Vector3 des)
    {
        agent.SetDestination(des);
    }

    private void OnAnimationRangeAttack()
    {
        var b = SimplePool.Spawn(bullet);
        b.transform.position = firePos.position;
        b.Init(10, (target.position - transform.position).normalized, bulletDamage);
    }
}
