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

    public GameObject weapon;
    private Weapon weaponScript;

    public int HP { get => playerData.Status.HP; set => playerData.Status.HP = value; }
    public int AttackPower => playerData.GetAttackPower();
    public int Defence => playerData.GetDefence();

    public LayerMask enemyLayer;

    private PlayerAnimation playerAnim;
    private float hitRecoveryTime = 1f;

    private PlayerData playerData;
    private InventoryPresenter inventoryPresenter;

    private Animator animator;
    private readonly string attackAnimName = "Attack";
    private bool isAttackActive = false;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerData = new PlayerData();

        weaponScript = weapon.GetComponent<Weapon>();
        ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentState == State.Attack && other.CompareTag("Enemy"))
        {
            ICombatant enemy = other.GetComponentInParent<ICombatant>();
            if (enemy != null)
            {
                Debug.Log($"[PlayerFSM] ����� �� Ÿ��: {enemy}");
                Attack(enemy);
            }
        }
    }

    public void SetInventoryPresenter(InventoryPresenter presenter)
    {
        inventoryPresenter = presenter;
    }

    public void TakeDamage(int damage)
    {
        int calculateDamage = Mathf.Max(0, damage - Defence);
        Debug.Log($"[PlayerFSM] {damage} ���� ����. ���� {Defence} ���� �� ���� ����: {calculateDamage}");
        playerData.ApplyDamage(calculateDamage);
        if(playerData.Status.HP <= 0)
        {
            Debug.Log("[PlayerFSM] �÷��̾� ���");
            ChangeState(State.Dead, PlayerAnimation.ANIM_DIE);
        }
        else
        {
            Debug.Log("[PlayerFSM] �÷��̾� �ǰ� ���·� ��ȯ");
            ChangeState(State.Hit, PlayerAnimation.ANIM_HIT);
        }
    }

    public void Attack(ICombatant target)
    {
        int damage = Mathf.Max(0, AttackPower - target.Defence);
        Debug.Log($"[PlayerFSM] ����! ���ݷ�: {AttackPower}, ���� ����: {target.Defence}, ���� ����: {damage}");
        target.TakeDamage(damage);
    }

    public void AttackAnimation()
    {
        Debug.Log("[PlayerFSM] ���� �ִϸ��̼� ���");
        playerAnim.ChangeAnim(PlayerAnimation.ANIM_ATTACK);
    }

    public void PerformAttack()
    {
        Debug.Log("[PlayerFSM] ���� ����");
        AttackAnimation();
        ChangeState(State.Attack, PlayerAnimation.ANIM_ATTACK);
    }

    private void ChangeState(State newState, int animNum)
    {
        if (currentState == newState) return;

        Debug.Log($"[PlayerFSM] ���� ����: {currentState} -> {newState}");
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

        if (weaponScript != null)
        {
            weaponScript.OnWeaponEnable();
        }

        if (!IsAnimationPlaying(attackAnimName))
        {
            ChangeState(State.AttackWait, PlayerAnimation.ANIM_ATTACKIDLE);

            if (weaponScript != null)
            {
                weaponScript.OnWeaponDisable();
            }
        }
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
            int nextSlot = inventoryPresenter.GetNextEmptySlot();  // �� ���� �ε��� ã��
            if (nextSlot != -1)
            {
                inventoryPresenter.AddItem(item, nextSlot);  // �������� �� ���Կ� �߰�
                Debug.Log($"������ {item.ItemName}�� ȹ���߽��ϴ�.");
            }
            else
            {
                Debug.Log("�κ��丮�� �� ������ �����ϴ�.");
            }
        }
    }

    private bool IsAnimationPlaying(string animationName)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        return false;
    }
}
