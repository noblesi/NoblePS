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
    private Vector3 respawnPosition;

    public float rotAnglePerSecond = 360f;
    public float moveSpeed = 2f;

    private float attackDelay = 2f;
    private float attackTimer = 0f;
    private float respawnDelay = 5f;

    public int HP { get => playerData.Status.HP; set => playerData.Status.HP = value; }
    public int AttackPower => playerData.GetAttackPower();
    public int Defence => playerData.GetDefence();

    public LayerMask enemyLayer;

    private PlayerAnimation playerAnim;
    private float hitRecoveryTime = 1f;
    private float attackRange = 2f;

    private PlayerData playerData;
    private InventoryPresenter inventoryPresenter;
    private StatusPresenter statusPresenter;
    private Animator animator;

    private PlayerHUD playerHUD;

    private bool hasDealtDamage = false;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerData = new PlayerData();

        playerHUD = FindObjectOfType<PlayerHUD>();
        if(playerHUD != null)
        {
            playerHUD.Initialize(playerData.Status);
        }

        respawnPosition = transform.position;

        ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
    }

    private void AttackEnemiesInRange()
    {
        if (!hasDealtDamage)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
            Debug.Log("1234");
            foreach (var hitCollider in hitColliders)
            {
                ICombatant enemy = hitCollider.GetComponent<ICombatant>();
                if (enemy != null)
                {
                    int damage = CalculateDamage(AttackPower, enemy.Defence);

                    if (damage > 0)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
            hasDealtDamage = true;
        }
    }

    public int CalculateDamage(int attackerPower, int defenderDefence)
    {
        return Mathf.Max(0, attackerPower - defenderDefence); 
    }

    public void SetInventoryPresenter(InventoryPresenter presenter) => inventoryPresenter = presenter;
    public void SetStatusPresenter(StatusPresenter presenter) => statusPresenter = presenter;

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            playerData.Status.TakeDamage(damage);

            playerHUD?.Initialize(playerData.Status);

            if (playerData.Status.HP <= 0)
            {
                ChangeState(State.Dead, PlayerAnimation.ANIM_DIE);
            }
            else
            {
                ChangeState(State.Hit, PlayerAnimation.ANIM_HIT);
            }
        }
    }

    public void PerformAttack()
    {
        if (currentState == State.Attack) return;

        hasDealtDamage = false;
        ChangeState(State.Attack, PlayerAnimation.ANIM_ATTACK);
    }

    private void ChangeState(State newState, int animNum)
    {
        if (currentState == newState) return;

        playerAnim.ChangeAnim(animNum);
        currentState = newState;
    }

    private void UpdateState()
    {
        if (currentState == State.Dead) return;

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

        AttackEnemiesInRange();
        
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
        playerData.Status.SetCurrentExp(0);

        respawnPosition = transform.position;

        Invoke(nameof(RespawnPlayer), respawnDelay);
    }

    private void RespawnPlayer()
    {
        transform.position = respawnPosition;
        playerData.Status.HP = playerData.Status.MaxHP;
        playerData.Status.MP = playerData.Status.MaxMP;

        ChangeState(State.Idle, PlayerAnimation.ANIM_IDLE);
    }


    public void MoveTo(Vector3 targetPos)
    {
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

    public void GainEXP(int exp) => statusPresenter.GainExperience(exp);
    public void SetPlayerData(PlayerData data) => playerData = data;

    public void PickupItem(Item item)
    {
        if (inventoryPresenter != null)
        {
            int nextSlot = inventoryPresenter.GetNextEmptySlot();  
            if (nextSlot != -1)
            {
                inventoryPresenter.AddItem(item, nextSlot); 
            }
        }
    }

    public void HealPlayer(int amount)
    {
        playerData.Status.Heal(amount);
    }

    public void RestoreMana(int amount)
    {
        playerData.Status.RestoreMana(amount);
    }
}
