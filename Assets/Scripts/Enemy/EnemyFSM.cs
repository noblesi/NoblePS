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
    private Enemy enemy;

    private float rotAnglePerSecond = 360f;

    public Transform hitBoxSpawnPoint;
    public float hitBoxRadius = 1.5f;

    public LayerMask playerLayer;

    private bool isDead = false;

    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        enemy = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeState(State.Idle, EnemyAnimation.ANIM_IDLE);
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
            enemy.TakeDamage(damage);

            if (enemy.IsAlive())
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
        if (GetDistanceFromPlayer() < enemy.MonsterData.ChaseDistance)
        {
            ChangeState(State.Chase, EnemyAnimation.ANIM_IDLE);
        }
    }

    private void ChaseState()
    {
        if(GetDistanceFromPlayer() < enemy.MonsterData.AttackDistance)
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
        if (GetDistanceFromPlayer() > enemy.MonsterData.AttackDistance)
        {
            ChangeState(State.Chase, EnemyAnimation.ANIM_MOVE);
        }
        else
        {
            enemyAnim.ChangeAnim(EnemyAnimation.ANIM_ATTACK);
            // 플레이어에게 데미지 주기 로직 추가
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
        yield return new WaitForSeconds(2f);

        var droppedItems = enemy.DropItems(FindObjectOfType<ItemLoader>());
        foreach (var item in droppedItems)
        {
            Debug.Log($"Dropped item: {item.ItemName}");

            var itemObject = item.GetPrefab();
            var instantiatedItem = Instantiate(itemObject, transform.position, Quaternion.identity);

            var itemPickup = instantiatedItem.GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                itemPickup.Initialize(item);
            }
            else
            {
                Debug.LogError("ItemPickup component not found on instantiated item.");
            }
        }

        Destroy(gameObject);
    }

    private void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    private void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemy.MonsterData.MoveSpeed * Time.deltaTime);
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
        enemy.MonsterData.HP = 0;
        ChangeState(State.Dead, EnemyAnimation.ANIM_DIE);
    }
}
