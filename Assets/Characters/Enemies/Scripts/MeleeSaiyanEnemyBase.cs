using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSaiyanEnemyBase : EnemyBase
{
    private Animator healthBarAnimator;

    protected override void GetComponents()
    {
        healthSystem = GetComponent<HealthSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponentInChildren<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        healthBarAnimator = transform.GetChild(1).GetComponent<Animator>();
    }
    protected override void GetMovementDirection()
    {
        MoveDestination = (Target.transform.position - transform.position).normalized;
    }

    protected override void PlayIdleAnimation()
    {
        base.PlayIdleAnimation();
        healthBarAnimator.Play("idle");
    }

    protected override void PlayWalkAnimation()
    {
        base.PlayWalkAnimation();
        healthBarAnimator.Play("walk");
    }
}
