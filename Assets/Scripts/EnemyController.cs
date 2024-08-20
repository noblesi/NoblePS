using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public Enemy enemy;
    private IEnemyState currentState;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public LayerMask playerLayerMask; // �÷��̾ Ž���ϱ� ���� LayerMask
    public float detectionRadius = 10f; // ���� ����

    private PlayerController player;

    public PlayerController Player // public ������Ƽ�� ����
    {
        get { return player; }
        private set { player = value; }
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // �ʱ� ���� ����
        SwitchState(new EnemyIdleState());
    }

    private void Update()
    {
        if (enemy.IsDead()) return;
        currentState?.UpdateState();
    }

    public void SwitchState(IEnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState(this);
    }

    public void TakeDamage(int amount)
    {
        enemy.TakeDamage(amount);
        if (enemy.IsDead())
        {
            animator.SetTrigger("Die");
            navMeshAgent.isStopped = true;
            // ��� �� ó�� ���� �߰�
        }
    }

    public void MoveTo(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    public bool IsPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayerMask);
        if (hitColliders.Length > 0)
        {
            player = hitColliders[0].GetComponent<PlayerController>();
            return true;
        }
        return false;
    }

    public void Attack()
    {
        if (IsPlayerInRange())
        {
            animator.SetTrigger("Attack");
            player.TakeDamage(enemy.attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
