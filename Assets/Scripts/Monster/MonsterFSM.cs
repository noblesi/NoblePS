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
    private float attackRange = 2f;

    public LayerMask playerLayer;

    public int MonsterID;
    private MonsterLoader monsterLoader;
    private ItemLoader itemLoader;
    private Monster monsterData;

    private Animator animator;

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

    private void AttackPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        foreach (var hitCollider in hitColliders)
        {
            ICombatant player = hitCollider.GetComponent<ICombatant>();
            if (player != null)
            {
                int damage = CalculateDamage(AttackPower, player.Defence);
                if (damage > 0)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }

    public int CalculateDamage(int attackerPower, int defenderDefence)
    {
        return Mathf.Max(0, attackerPower - defenderDefence);
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
        if (damage > 0)
        {
            HP -= damage;

            if (HP <= 0)
            {
                ChangeState(State.Dead, MonsterAnimation.ANIM_DIE);
            }
            else
            {
                ChangeState(State.Hit, MonsterAnimation.ANIM_HIT);
            }
        }
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
            ChangeState(State.Chase, MonsterAnimation.ANIM_MOVE);
            return;
        }

        AttackPlayerInRange();
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
            ChangeState(State.Chase, MonsterAnimation.ANIM_MOVE);
        }
    }

    private void DeadState()
    {
        if (!isDead)
        {
            isDead = true;
            GrantPlayerEXP();

            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 1f);

        DropItems();

        Destroy(gameObject);
    }

    private void DropItems()
    {
        if (monsterData != null && itemLoader != null)
        {
            List<Item> droppedItems = monsterData.GetDroppedItems(itemLoader);

            foreach (var item in droppedItems)
            {
                Debug.Log($"Dropped item: {item.ItemName}");

                GameObject itemObject = item.GetPrefab();

                GameObject instantiatedItem = Instantiate(itemObject, transform.position, Quaternion.identity);

                ItemPickup itemPickup = instantiatedItem.GetComponent<ItemPickup>();

                if (itemPickup != null)
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
}
