using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour, ICombatant
{
    public enum State { Idle, Chase, Attack, Hit, Dead }
    public State currentState = State.Idle;

    private EnemyAnimation enemyAnim;
    private Transform player;

    private float rotAnglePerSecond = 360f;
    private float moveSpeed = 1.3f;

    public GameObject weapon;
    private Weapon weaponScript;

    public LayerMask playerLayer;

    public int MonsterID;
    private MonsterLoader monsterLoader;
    private ItemLoader itemLoader;
    private Monster monsterData;

    private int monsterEXP = 50;

    public int HP
    {
        get => monsterData?.HP ?? 0;
        set
        {
            if (monsterData != null) monsterData.HP = value;
        }
    }
    public int AttackPower => monsterData?.AttackPower ?? 0;
    public int Defence => monsterData?.Defence ?? 0;

    private bool isDead = false;

    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        monsterLoader = FindObjectOfType<MonsterLoader>();
        itemLoader = FindObjectOfType<ItemLoader>();

        weaponScript = weapon.GetComponent<Weapon>();

        if (monsterLoader != null)
        {
            monsterData = monsterLoader.GetMonsterByID(MonsterID);
            if (monsterData != null)
            {
                monsterData.TakeDamage(0);
                ChangeState(State.Idle, EnemyAnimation.ANIM_IDLE);
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentState == State.Attack && other.CompareTag("PlayeBody"))
        {
            ICombatant player = other.GetComponentInParent<ICombatant>();
            if(player != null)
            {
                Debug.Log($"[EnemyFSM] ����� �÷��̾� Ÿ��: {player}");
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
        Debug.Log($"[EnemyFSM] {damage} ���� ����. ���� {Defence} ���� �� ���� ����: {calculatedDamage}");
        monsterData.TakeDamage(calculatedDamage);
        if (monsterData.HP <= 0)
        {
            Debug.Log("[EnemyFSM] ���� ���");
            ChangeState(State.Dead, EnemyAnimation.ANIM_DIE);
        }
        else
        {
            Debug.Log("[EnemyFSM] ���� �ǰ� ���·� ��ȯ");
            ChangeState(State.Hit, EnemyAnimation.ANIM_HIT);
        }
    }

    public void Attack(ICombatant target)
    {
        int damage = Mathf.Max(0, AttackPower - target.Defence);
        Debug.Log($"[EnemyFSM] ����! ���ݷ�: {AttackPower}, ���� ����: {target.Defence}, ���� ����: {damage}");
        target.TakeDamage(damage);
    }

    public void ChangeState(State newState, int animNum)
    {
        if (currentState == newState) return;

        Debug.Log($"[EnemyFSM] ���� ����: {currentState} -> {newState}");
        currentState = newState;
        enemyAnim.ChangeAnim(animNum);
    }

    private void IdleState()
    {
        if (GetDistanceFromPlayer() < 5f)
        {
            Debug.Log("[EnemyFSM] �÷��̾� ����! �߰� ���·� ��ȯ");
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }
    }

    private void ChaseState()
    {
        if(GetDistanceFromPlayer() < 2.5f)
        {
            Debug.Log("[EnemyFSM] �÷��̾� ����! ���� ���·� ��ȯ");
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
        if (GetDistanceFromPlayer() > 3f)
        {
            Debug.Log("[EnemyFSM] �÷��̾� �־���! �߰� ���·� ��ȯ");
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }

    }

    private void HitState()
    {
        Debug.Log("[EnemyFSM] �ǰ� ���� ����");
        Invoke(nameof(RecoverFromHit), 1f);
    }

    private void RecoverFromHit()
    {
        if (currentState == State.Hit)
        {
            Debug.Log("[EnemyFSM] �ǰ� ȸ��! �߰� ���·� ��ȯ");
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }
    }

    private void DeadState()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("[EnemyFSM] ���� ���� �ִϸ��̼� ����");
            enemyAnim.ChangeAnim(EnemyAnimation.ANIM_DIE);
            //GrantPlayerEXP(); //���Ŀ� ����� ������ ����.

            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2f);

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

        Destroy(gameObject);
    }

    private void GrantPlayerEXP()
    {
        PlayerFSM playerFSM = player.GetComponent<PlayerFSM>();
        if(playerFSM != null)
        {
            playerFSM.GainEXP(monsterEXP);
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
        //������ FŰ�� ������ ���� ���
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("����� : ���Ͱ� ��� ����մϴ�.");
            SimulateDeath();
        }
        UpdateState();
    }

    private void SimulateDeath()
    {
        HP = 0;
        ChangeState(State.Dead, EnemyAnimation.ANIM_DIE);
    }
}
