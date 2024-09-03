using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public const int ANIM_IDLE = 0;
    public const int ANIM_MOVE = 1;
    public const int ANIM_ATTACK = 2;
    public const int ANIM_HIT = 3;
    public const int ANIM_DIE = 4;

    private Animator EnemyAnim;

    private void Start()
    {
        EnemyAnim = GetComponent<Animator>();
    }

    public void ChangeAnim(int animNum)
    {
        EnemyAnim.SetInteger("animName", animNum);
    }
}
