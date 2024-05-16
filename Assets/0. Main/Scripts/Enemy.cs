using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Entity, IHealth
{
    [Header("Health")]
    [SerializeField] private HealthTeamSide healthTeamSide;
    [SerializeField] private float maxHealth;
    [SerializeField] ParticleSystem dieEffect;
    [SerializeField] GameObject healthBar;
    [SerializeField] Image currentHealthBar;

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

    private int animIDAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        animIDAttack = Animator.StringToHash("Attack");
        sprRange = agent.stoppingDistance * agent.stoppingDistance;

        agent.updateRotation = false;
        Init();
    }

    public void Init()
    {
        currentHealth = maxHealth;
        currentHealthBar.fillAmount = 1;
        healthBar.SetActive(false);
    }

    Coroutine offHealthBar;
    public void TakeDamge(float damage, HealthEventHandler evt)
    {
        print("Health = " + currentHealth + " ,damage = " + damage);
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            print("Die");
            Die();
            onDie?.Invoke(this);
            return;
        }

        healthBar.gameObject.SetActive(true);
        if (offHealthBar != null) StopCoroutine(offHealthBar);
        offHealthBar = this.DelayFunction(1, () => healthBar.gameObject.SetActive(false));
        currentHealthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Die()
    {
        var e = SimplePool.Spawn(dieEffect);
        e.transform.position = transform.position + Vector3.up;
        e.Play();
        SimplePool.Despawn(gameObject);
    }

    public void Regeneration(float regeneration, HealthEventHandler evt)
    {
        currentHealth += regeneration;
        healthBar.gameObject.SetActive(true);
        if (offHealthBar != null) StopCoroutine(offHealthBar);
        offHealthBar = this.DelayFunction(1, () => healthBar.gameObject.SetActive(false));
        currentHealthBar.fillAmount = currentHealth / maxHealth;
    }

    float sprRange;
    private void Update()
    {
        if (target == null)
        {
            animator.SetBool(animIDAttack, false);
            return;
        }

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
            animator.SetBool(animIDAttack, true);
        }
        else
        {
            animator.SetBool(animIDAttack, false);
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
        if (!b) return;
        b.transform.position = firePos.position;
        b.Init(10, (target.position - transform.position).normalized, bulletDamage, 10, gameObject);
    }
}
