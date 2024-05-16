using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    public void InitAutoDespawn(float despawnDelay = 2)
    {
        StartCoroutine(Despawn(despawnDelay));
    }

    IEnumerator Despawn(float despawnDelay = 2)
    {
        yield return new WaitForSeconds(despawnDelay);
        SimplePool.Despawn(gameObject);
    }
}
