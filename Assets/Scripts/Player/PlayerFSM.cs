using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFSM : MonoBehaviour, ICombatant
{
    public enum State
    {
        Idle, Move, Attack, AttackWait, Hit, Dead
    }

    public State currentState = State.Idle;

    private Vector3 currentTargetPos;
    private ICombatant currentEnemy;

    public float rotAnglePerSecond = 360f;
    public float moveSpeed = 2f;

    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float attackTimer = 0f;

    public Transform hitBoxSpawnPoint;
    public float hitBoxRadius = 1.5f;

    public int HP { get => playerData.Status.HP; set => playerData.Status.HP = value; }
    public int AttackPower => playerData.GetAttackPower();
    public int Defence => playerData.GetDefence();

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

    private void OnTriggerEnter(Collider other)
    {
        ICombatant enemy = other.GetComponent<ICombatant>();
        if (enemy != null && currentState == State.Attack)
        {
            Attack(enemy);
        }
    }

    public void SetInventoryPresenter(InventoryPresenter presenter)
    {
        inventoryPresenter = presenter;
    }

    public void TakeDamage(int damage)
    {
        int calculateDamage = Mathf.Max(0, damage - Defence);
        playerData.ApplyDamage(calculateDamage);
        if(playerData.Status.HP <= 0)
        {
            ChangeState(State.Dead, PlayerAnimation.ANIM_DIE);
        }
        else
        {
            ChangeState(State.Hit, PlayerAnimation.ANIM_HIT);
        }
    }

    public void Attack(ICombatant target)
    {
        int damage = Mathf.Max(0, AttackPower - target.Defence);
        target.TakeDamage(damage);
    }

    public void AttackAnimation()
    {
        playerAnim.ChangeAnim(PlayerAnimation.ANIM_ATTACK);
    }

    public void PerformAttack()
    {
        // 공격 범위 내에 있는 적을 찾는다.
        Collider[] hitEnemies = Physics.OverlapSphere(hitBoxSpawnPoint.position, hitBoxRadius, enemyLayer);
        foreach (Collider enemyCollider in hitEnemies)
        {
            ICombatant enemy = enemyCollider.GetComponent<ICombatant>();
            if (enemy != null)
            {
                // 적이 있다면 공격을 수행하고 상태를 변경한다.
                Attack(enemy);
                ChangeState(State.AttackWait, PlayerAnimation.ANIM_ATTACKIDLE);
                return;
            }
        }

        // 공격 범위 내에 적이 없을 경우 대기 상태로 전환
        ChangeState(State.AttackWait, PlayerAnimation.ANIM_ATTACKIDLE);
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

        AttackAnimation();

        PerformAttack();
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
            int nextSlot = inventoryPresenter.GetNextEmptySlot();  // 빈 슬롯 인덱스 찾기
            if (nextSlot != -1)
            {
                inventoryPresenter.AddItem(item, nextSlot);  // 아이템을 빈 슬롯에 추가
                Debug.Log($"아이템 {item.ItemName}을 획득했습니다.");
            }
            else
            {
                Debug.Log("인벤토리에 빈 슬롯이 없습니다.");
            }
        }
    }
}
