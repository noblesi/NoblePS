using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum State
    {
        Idle, Move, Attack, AttackWait, Hit, Dead
    }

    public State currentState = State.Idle;

    private Vector3 currentTargetPos;

    private GameObject currentEnemy;

    public float rotAnglePerSecond = 360f;

    public float moveSpeed = 2f;

    [SerializeField] private float attackDelay = 2f;

    [SerializeField] private float attackTimer = 0f;

    public GameObject hitBoxPrefsb;
    public Transform hitBoxSpawnPoint;

    private float attackDistance = 1.5f;

    private PlayerAnimation playerAnim;

    private int playerHP = 100;
    private float hitRecoveryTime = 1f;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();

        ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
    }

    public void AttackEnemy(GameObject enemy)
    {
        currentEnemy = enemy;
        currentTargetPos = currentEnemy ? currentEnemy.transform.position : transform.position;
        ChangeState(State.Attack, PlayerAnimation.ANIM_ATTACK);
    }

    public void TakeDamage(int damage)
    {
        if(currentState != State.Dead)
        {
            playerHP -= damage;
            if(playerHP <= 0)
            {
                ChangeState(State.Hit, PlayerAnimation.ANIM_HIT);
            }
            else
            {
                ChangeState(State.Dead, PlayerAnimation.ANIM_DIE);
            }
        }
    }

    private void ChangeState(State newState, int animNum)
    {
        if (currentState == newState) return;

        playerAnim.ChangeAnim(animNum);
        currentState = newState;
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Move:
                MoveState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.AttackWait:
                AttackWaitState();
                break;
            case State.Hit:
                HitState();
                break;
            case State.Dead:
                DeadState();
                break;
            default:
                break;
        }
    }

    private void IdleState()
    {

    }

    private void MoveState()
    {
        TurnToDestination();
        MoveToDestination();
    }

    private void AttackState()
    {
        attackTimer = 0f;

        transform.LookAt(currentTargetPos);

        if(hitBoxPrefsb != null && hitBoxSpawnPoint != null)
        {
            Instantiate(hitBoxSpawnPoint, hitBoxSpawnPoint.position, hitBoxSpawnPoint.rotation);
        }

        ChangeState(State.AttackWait, PlayerAnimation.ANIM_ATTACKIDLE);
    }

    private void AttackWaitState()
    {
        if (attackTimer > attackDelay)
        {
            ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
        }

        attackTimer += Time.deltaTime;
    }

    private void HitState()
    {
        Invoke(nameof(RecoverFromHit), hitRecoveryTime);
    }

    private void RecoverFromHit()
    {
        if(currentState == State.Hit)
        {
            ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
        }
    }

    private void DeadState()
    {

    }


    public void MoveTo(Vector3 targetPos)
    {
        currentEnemy = null;
        currentTargetPos = targetPos;
        ChangeState(State.Move, PlayerAnimation.ANIM_MOVE);
    }

    private void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(currentTargetPos - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    private void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPos, moveSpeed * Time.deltaTime);

        if(transform.position == currentTargetPos)
        {
            ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
        }
    }

    private void Update()
    {
        UpdateState();
    }
}
