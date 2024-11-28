using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Run = Animator.StringToHash("Run");
    
    public void PlayIdleAnimation()
    {
        animator.SetBool(Idle, true);
        animator.SetBool(Run, false);
    }

    public void PlayRunAnimation()
    {
        animator.SetBool(Idle, false);
        animator.SetBool(Run, true);
    }
}
