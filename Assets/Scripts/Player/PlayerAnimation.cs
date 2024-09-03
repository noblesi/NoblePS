using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public const int ANIM_IDLE = 0;
    public const int ANIM_MOVE = 1;
    public const int ANIM_ATTACK = 2;
    public const int ANIM_ATTACKIDLE = 3;
    public const int ANIM_HIT = 4;
    public const int ANIM_DIE = 5;

    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnim(int animNum)
    {
        anim.SetInteger("animName", animNum);
    }
}
