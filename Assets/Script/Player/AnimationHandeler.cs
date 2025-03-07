using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationStatus
{
    Walk,
    Jump,
    Run
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
            { AnimationStatus.Walk, ActiveWalkAnimation },
            { AnimationStatus.Run, ActiveRunAnimation }
        };
    }

    void ActiveWalkAnimation(float speed)
    {
        animator.SetBool("Move", speed > 0.5f);
    }

    void ActiveJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    void ActiveRunAnimation(float speed)
    {
        animator.SetBool("Run", speed >= GameManager.Instance.stat.runSpeed);
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
