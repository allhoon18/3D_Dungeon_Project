using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public enum AnimationStatus
{
    Walk,
    Jump
}

public class AnimationHandeler : MonoBehaviour
{
    Animator animator;

    Action AnimationAction;
    Action<float> AnimationActionWithInput;

    Dictionary<AnimationStatus, Action> animationHandlersWithoutInput;
    Dictionary<AnimationStatus, Action<float>> animationHandlersWithInput;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animationHandlersWithoutInput = new Dictionary<AnimationStatus, Action>()
        {
            { AnimationStatus.Jump, ActiveJumpAnimation}
        };

        animationHandlersWithInput = new Dictionary<AnimationStatus, Action<float>>()
        {
            { AnimationStatus.Walk, ActiveWalkAnimation }
        };
    }

    void ActiveWalkAnimation(float speed)
    {
        animator.SetBool("Move", speed > 0);
    }

    void ActiveJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    public void ActiveAnimation(AnimationStatus animationStatus, float input = 0)
    {
        if(animationHandlersWithoutInput.TryGetValue(animationStatus, out AnimationAction))
        {
            AnimationAction?.Invoke();
        }
        else if ( animationHandlersWithInput.TryGetValue(animationStatus, out AnimationActionWithInput))
        {
            AnimationActionWithInput?.Invoke(input);
        }

    }
}
