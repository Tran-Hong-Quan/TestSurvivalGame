using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] Vector3 direction;
    [SerializeField] float damage;

    [SerializeField] ParticleSystem hitEff;
    [SerializeField] HealthTeamSide teamSide;
    public void Init(float speed, Vector3 direction, float damage)
    {
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Hit to " + other.name);

        var e = SimplePool.Spawn(hitEff);
        e.transform.position = transform.position;
        e.Play();
        e.gameObject.SendMessage("InitAutoDespawn", e.main.duration);
        SimplePool.Despawn(gameObject);

        if (other.TryGetComponent(out IHealth enemy))
        {
            if (enemy.HealthTeamSide == teamSide) return;
            enemy.TakeDamge(damage, new HealthEventHandler(gameObject, teamSide));
        }
    }
}
