using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IHealth
{
    [Header("Health")]
    [SerializeField] private HealthTeamSide healthTeamSide;
    [SerializeField] private float maxHealth;

    private float currentHealth;
    public GameObject GameObject => gameObject;

    public HealthTeamSide HealthTeamSide => healthTeamSide;

    public float MaxHealth => maxHealth;

    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        maxHealth = currentHealth;
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
}
