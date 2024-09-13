using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public Transform hitBoxSpawnPoint;
    public float hitBoxRadius = 1.5f;
    public int damage = 20;

    public LayerMask enemyLayer;

    private PlayerAnimation playerAnim;
    private float hitRecoveryTime = 1f;

    private PlayerData playerData;
    private InventoryPresenter inventoryPresenter;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        playerData = new PlayerData();
        ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
    }

    public void SetInventoryPresenter(InventoryPresenter presenter)
    {
        inventoryPresenter = presenter;
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
            playerData.Status.HP -= damage;
            if(playerData.Status.HP <= 0)
            {
                ChangeState(State.Dead, PlayerAnimation.ANIM_DIE);
            }
            else
            {
                ChangeState(State.Hit, PlayerAnimation.ANIM_HIT);
            }

            playerData.SavePlayerData();
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

        Collider[] hitEnemies = Physics.OverlapSphere(hitBoxSpawnPoint.position, hitBoxRadius, enemyLayer);
        foreach(Collider enemy in hitEnemies)
        {
            EnemyFSM enemyFSM = enemy.GetComponent<EnemyFSM>();
            if(enemyFSM != null)
            {
                enemyFSM.TakeDamage(damage);
            }
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
        if (EventSystem.current.IsPointerOverGameObject()) return;

        UpdateState();
    }

    public void GainEXP(int exp)
    {
        playerData.Status.GainExp(exp);
        playerData.SavePlayerData();
    }

    public void SetPlayerData(PlayerData data)
    {
        playerData = data;
    }

    public void PickupItem(Item item)
    {
        if (inventoryPresenter != null)
        {
            int nextSlot = inventoryPresenter.GetNextEmptySlot();  // ∫Û ΩΩ∑‘ ¿Œµ¶Ω∫ √£±‚
            if (nextSlot != -1)
            {
                inventoryPresenter.AddItem(item, nextSlot);  // æ∆¿Ã≈€¿ª ∫Û ΩΩ∑‘ø° √ﬂ∞°
                Debug.Log($"æ∆¿Ã≈€ {item.ItemName}¿ª »πµÊ«ﬂΩ¿¥œ¥Ÿ.");
            }
            else
            {
                Debug.Log("¿Œ∫•≈‰∏Æø° ∫Û ΩΩ∑‘¿Ã æ¯Ω¿¥œ¥Ÿ.");
            }
        }
    }
}
