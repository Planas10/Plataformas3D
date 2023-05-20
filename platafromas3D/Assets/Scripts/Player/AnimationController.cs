using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] private Animator animator;

    private void Update()
    {
        animator.SetBool("IsCrouching", playerController.agachado);
        animator.SetBool("IsAttached", playerController.IsAttached);
        animator.SetBool("IsMoving", playerController.IsMoving);
        animator.SetBool("IsJumping", playerController.IsJumping);
        animator.SetBool("IsFalling", playerController.IsFalling);
    }
}
