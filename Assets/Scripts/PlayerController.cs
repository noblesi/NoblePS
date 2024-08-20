using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    public Player player;
    public float attackRange = 2f;
    public int attackDamage = 20;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            navMeshAgent.SetDestination(hit.point);
            animator.SetFloat("Speed", 1f);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Attack");

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        player.TakeDamage(amount);
        if (player.Health <= 0)
        {
            // 플레이어 사망 처리
            Debug.Log("Player is Dead");
        }
    }

    private void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            animator.SetFloat("Speed", 0f); // 이동 중이 아니면 애니메이션을 멈춤
        }
    }
}
