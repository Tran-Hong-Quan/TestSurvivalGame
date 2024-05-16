using QuanUtilities;
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

    GameObject caller;

    public void Init(float speed, Vector3 direction, float damage, float timeToLive = 10, GameObject caller = null,float callerRemvomeDelay = 1)
    {
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;

        this.DelayFunction(timeToLive, () => SimplePool.Despawn(gameObject));
        this.caller = caller;
        this.DelayFunction(callerRemvomeDelay, () => caller = null);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckHit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckHit(collision.collider);
    }

    private void CheckHit(Collider collider)
    {
        print("Hit to" + collider.name);

        if (collider.gameObject == caller) return;
        var e = SimplePool.Spawn(hitEff);
        e.transform.position = transform.position;
        e.Play();

        if (collider.TryGetComponent(out IHealth enemy))
        {
            if (enemy.HealthTeamSide != teamSide || teamSide == HealthTeamSide.None)
            {
                enemy.TakeDamge(damage, new HealthEventHandler(gameObject, teamSide));
            }
        }
        SimplePool.Despawn(gameObject);
    }
}
