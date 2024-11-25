using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    
    public void PlayIdleAnimation()
    {
        animator.SetTrigger(Idle);
    }
}
