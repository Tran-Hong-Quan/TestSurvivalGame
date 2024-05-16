using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDespawnParticle : MonoBehaviour
{
    private void OnEnable()
    {
        this.DelayFunction(GetComponent<ParticleSystem>().main.duration, () =>
        {
            SimplePool.Despawn(gameObject);
        });   
    }
}
