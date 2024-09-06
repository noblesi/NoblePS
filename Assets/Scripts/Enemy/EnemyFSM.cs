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

    public Transform hitBoxSpawnPoint;
    public float hitBoxRadius = 1.5f;
    public int damage = 15;

    public LayerMask playerLayer;

    public int monsterEXP = 50;

    public int MonsterID;
    private MonsterLoader monsterLoader;
    private ItemLoader itemLoader;
    private Monster monsterData;

    private bool isDead = false;

    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        ChangeState(State.Idle, EnemyAnimation.ANIM_IDLE);
        player = GameObject.FindGameObjectWithTag("Player").transform;

        monsterLoader = FindObjectOfType<MonsterLoader>();
        itemLoader = FindObjectOfType<ItemLoader>();

        if (monsterLoader != null)
        {
            monsterData = monsterLoader.GetMonsterByID(MonsterID);
            if (monsterData != null)
            {
                enemyHP = monsterData.HP; // 몬스터 HP 설정
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
            if (IsPlayerInRange())
            {
                ChangeState(State.Attack, EnemyAnimation.ANIM_ATTACK);
            }
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

    private bool IsPlayerInRange()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(hitBoxSpawnPoint.position, hitBoxRadius, playerLayer);
        return hitPlayers.Length > 0;
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

    private void DeadState()
    {
        if (!isDead)
        {
            isDead = true;
            enemyAnim.ChangeAnim(EnemyAnimation.ANIM_DIE);
            //GrantPlayerEXP(); //추후에 제대로 구현할 예정.

            StartCoroutine(HandleDeath());
        }




    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathDelay);

        if (monsterData != null && itemLoader != null)
        {
            List<Item> droppedItems = monsterData.GetDroppedItems(itemLoader);

            foreach (var item in droppedItems)
            {
                Debug.Log($"Dropped item: {item.ItemName}");

                GameObject itemObject = item.GetPrefab();

                Instantiate(itemObject, transform.position, Quaternion.identity);
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
        //디버깅용 F키를 누르면 몬스터 사망
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("디버깅 : 몬스터가 즉시 사망합니다.");
            SimulateDeath();
        }


        UpdateState();
    }

    private void OnDrawGizmosSelected()
    {
        if(hitBoxSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBoxSpawnPoint.position, hitBoxRadius);
        }
    }

    private void SimulateDeath()
    {
        enemyHP = 0;
        ChangeState(State.Dead, EnemyAnimation.ANIM_DIE);
    }
}
