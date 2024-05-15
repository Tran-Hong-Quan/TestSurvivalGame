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
    [SerializeField] private GameObject bullet;

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
        sprRange = attackRange*attackRange;

        aimIDAttack = Animator.StringToHash("Attack");
    }

    public void TakeDamge(float damage, HealthEventHandler evt)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
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
        if((transform.position - target.position).sqrMagnitude > sprRange)
        {
            SetDesitination(target.position);
            animator.SetBool(aimIDAttack, false);
        }
        else
        {
            SetDesitination(transform.position);
            animator.SetBool(aimIDAttack, true);
        }
    }

    private void SetDesitination(Vector3 des)
    {
        agent.SetDestination(des);
    }
}
