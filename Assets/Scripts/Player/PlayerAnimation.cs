using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum PlayerAnimationState
    {
        Idle,
        Move,
        Attack,
        AttackIdle,
        Hit,
        Die
    }

    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnim(PlayerAnimationState animState)
    {
        anim.SetInteger("animName", (int)animState);
    }
}
