using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsHandler : MonoBehaviour
{
    private void MeleeAttack()
    {
        SendMessage("OnAnimationMeleeAttack", SendMessageOptions.DontRequireReceiver);
    }

    private void FootStep()
    {
        SendMessage("OnFoodStep", SendMessageOptions.DontRequireReceiver);
    }

    private void RangeAttack()
    {
        SendMessage("OnAnimationRangeAttack", SendMessageOptions.DontRequireReceiver);
    }
}
