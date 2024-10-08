using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : MonoBehaviour, ICombatant
{
    public enum State { Idle, Chase, Attack, Hit, Dead }
    public State currentState = State.Idle;

    private MonsterAnimation enemyAnim;
    private Transform player;

    private float rotAnglePerSecond = 360f;
    private float moveSpeed = 1.3f;

    public LayerMask playerLayer;

    public int MonsterID;
    private MonsterLoader monsterLoader;
    private ItemLoader itemLoader;
    private Monster monsterData;

    private Animator animator;
    private readonly string attackAnimName = "Attack";

    public int HP
    {
        get => monsterData?.HP ?? 0;
        set
        {
            if (monsterData != null)
            {
                monsterData.HP = value;
                OnHealthChanged?.Invoke(monsterData.HP, monsterData.MaxHP);
            }
        }
    }
    public int AttackPower => monsterData?.AttackPower ?? 0;
    public int Defence => monsterData?.Defence ?? 0;

    private bool isDead = false;

    public event Action<int, int> OnHealthChanged;


    private void Start()
    {
        enemyAnim = GetComponent<MonsterAnimation>();
        animator = GetComponent<Animator>();
        monsterLoader = FindObjectOfType<MonsterLoader>();
        itemLoader = FindObjectOfType<ItemLoader>();

        MonsterHpBar hpBar = GetComponent<MonsterHpBar>();

        if (monsterLoader != null)
        {
            monsterData = monsterLoader.GetMonsterByID(MonsterID);
            if (monsterData != null)
            {
                ChangeState(State.Idle, MonsterAnimation.ANIM_IDLE);
                player = GameObject.FindGameObjectWithTag("Player").transform;

                OnHealthChanged?.Invoke(monsterData.HP, monsterData.MaxHP);
            }
        }

        if(hpBar != null)
        {
            hpBar.Initialize(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentState == State.Attack && other.CompareTag("PlayerBody"))
        {
            ICombatant player = other.GetComponentInParent<ICombatant>();
            if(player != null)
            {
                Debug.Log($"[EnemyFSM] 무기로 플레이어 타격: {player}");
                Attack(player);
            }
        }
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
        int calculatedDamage = Mathf.Max(0, damage - Defence);
        Debug.Log($"[EnemyFSM] {damage} 피해 입음. 방어력 {Defence} 적용 후 최종 피해: {calculatedDamage}");
        HP -= calculatedDamage;

        if (HP <= 0)
        {
            ChangeState(State.Dead, MonsterAnimation.ANIM_DIE);
        }
        else
        {
            ChangeState(State.Hit, MonsterAnimation.ANIM_HIT);
        }
    }

    public void Attack(ICombatant target)
    {
        int damage = Mathf.Max(0, AttackPower - target.Defence);
        Debug.Log($"[EnemyFSM] 공격! 공격력: {AttackPower}, 상대방 방어력: {target.Defence}, 최종 피해: {damage}");
        target.TakeDamage(damage);
    }

    public void ChangeState(State newState, int animNum)
    {
        if (currentState == newState) return;

        if (currentState == State.Dead) return;

        currentState = newState;
        enemyAnim.ChangeAnim(animNum);
    }

    private void IdleState()
    {
        if (GetDistanceFromPlayer() < 5f)
        {
            ChangeState(State.Chase, MonsterAnimation.ANIM_MOVE);
        }
    }

    private void ChaseState()
    {
        if(GetDistanceFromPlayer() < 2.5f)
        {
            Debug.Log("[EnemyFSM] 플레이어 접근! 공격 상태로 전환");
            ChangeState(State.Attack, MonsterAnimation.ANIM_ATTACK);
        }
        else
        {
            TurnToDestination();
            MoveToDestination();
        }
    }

    private void AttackState()
    {
        if (GetDistanceFromPlayer() > 3f)
        {
            Debug.Log("[EnemyFSM] 플레이어 멀어짐! 추격 상태로 전환");
            ChangeState(State.Chase, MonsterAnimation.ANIM_MOVE);
        }


        if (!IsAnimationPlaying(attackAnimName))
        {
            ChangeState(State.Idle, MonsterAnimation.ANIM_IDLE);
        }
    }

    private void HitState()
    {
        if(HP <= 0)
        {
            ChangeState(State.Dead, MonsterAnimation.ANIM_DIE);
        }

        Invoke(nameof(RecoverFromHit), 1f);
    }

    private void RecoverFromHit()
    {
        if (currentState == State.Hit)
        {
            Debug.Log("[EnemyFSM] 피격 회복! 추격 상태로 전환");
            ChangeState(State.Chase, MonsterAnimation.ANIM_MOVE);
        }
    }

    private void DeadState()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Monster is dead");

            if(player != null) GrantPlayerEXP();

            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);

        if (monsterData != null && itemLoader != null)
        {
            List<Item> droppedItems = monsterData.GetDroppedItems(itemLoader);

            foreach (var item in droppedItems)
            {
                Debug.Log($"Dropped item: {item.ItemName}");

                GameObject itemObject = item.GetPrefab();

                GameObject instantiatedItem = Instantiate(itemObject, transform.position, Quaternion.identity);

                ItemPickup itemPickup = instantiatedItem.GetComponent<ItemPickup>();

                if(itemPickup != null)
                {
                    itemPickup.Initialize(item);
                }
                else
                {
                    Debug.LogError("ItemPickup component not found on instantiated item.");
                }
            }
        }
    }

    private void GrantPlayerEXP()
    {
        PlayerFSM playerFSM = player.GetComponent<PlayerFSM>();
        if(playerFSM != null)
        {
            playerFSM.GainEXP(monsterData.ExperienceReward);
            Debug.Log($"[EnemyFSM] 플레이어에게 {monsterData.ExperienceReward} 경험치 지급");
        }
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
        //디버깅용 F키를 누르면 몬스터 사망
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("디버깅 : 몬스터가 즉시 사망합니다.");
            SimulateDeath();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("디버깅 : 몬스터에게 데미지를 줍니다.");
            SimulateDamage();
        }

        UpdateState();
    }

    private void SimulateDeath()
    {
        HP = 0;
        ChangeState(State.Dead, MonsterAnimation.ANIM_DIE);
    }

    private void SimulateDamage()
    {
        HP -= 10;
    }

    private bool IsAnimationPlaying(string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        return false;
    }
}
