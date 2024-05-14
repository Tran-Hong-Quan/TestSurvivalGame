using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsHandler : MonoBehaviour
{
    public UnityEvent onMeleeAttack;
    private void OnAnimationMeleeAttack()
    {
        onMeleeAttack?.Invoke();
    }
}
