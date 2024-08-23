using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Hit,
        Dead
    }

    public State currentState = State.Idle;

    private EnemyAnimation enemyAnim;
    private Transform player;

    private float chaseDistance = 5f;
    private float attackDistance = 2.5f;
    private float reChaseDistance = 3f;
    private float rotAnglePerSecond = 360f;
    private float moveSpeed = 1.3f;
    private float attackDelay = 2f;
    private float attackTimer = 0f;

    private int enemyHP = 100;
    private float deathDelay = 2f;

    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        ChangeState(State.Idle, EnemyAnimation.ANIM_IDLE);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Hit:
                HitState();
                break;
            case State.Dead:
                DeadState();
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState != State.Dead)
        {
            enemyHP -= damage;

            if (enemyHP > 0)
            {
                ChangeState(State.Hit, EnemyAnimation.ANIM_HIT);
            }
            else
            {
                ChangeState(State.Dead, EnemyAnimation.ANIM_DIE);
            }
        }
    }

    public void ChangeState(State newState, int animNum)
    {
        if (currentState == newState) return;

        currentState = newState;
        enemyAnim.ChangeAnim(animNum);
    }

    private void IdleState()
    {
        if (GetDistanceFromPlayer() < chaseDistance)
        {
            ChangeState(State.Chase, EnemyAnimation.ANIM_IDLE);
        }
    }

    private void ChaseState()
    {
        if(GetDistanceFromPlayer() < attackDistance)
        {
            ChangeState(State.Attack, EnemyAnimation.ANIM_ATTACK);
        }
        else
        {
            TurnToDestination();
            MoveToDestination();
        }
    }

    private void AttackState()
    {
        if(GetDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0f;
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }
        else
        {
            if(attackTimer > attackDelay)
            {
                transform.LookAt(player.position);
                enemyAnim.ChangeAnim(EnemyAnimation.ANIM_ATTACK);

                attackTimer = 0f;
            }

            attackTimer += Time.deltaTime;
        }
    }

    private void HitState()
    {
        Invoke(nameof(RecoverFromHit), 1f);
    }

    private void RecoverFromHit()
    {
        if (currentState == State.Hit)
        {
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }
    }

    private void ReturnToChaseState()
    {
        if(currentState == State.Hit)
        {
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }
    }

    private void DeadState()
    {
        enemyAnim.ChangeAnim(EnemyAnimation.ANIM_DIE);

        Destroy(gameObject, deathDelay);
    }

    private void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    private void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    private void Update()
    {
        UpdateState();
    }
}
