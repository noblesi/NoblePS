using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Camera mainCamera;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Update the animator with the current movement speed
        UpdateAnimator();
    }

    public void MoveToPointer(Vector2 pointerPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    private void UpdateAnimator()
    {
        // Get the current speed of the agent
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("speed", speed);

        // Transition between Idle and Run based on speed
        if (speed > 0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void TriggerAttack()
    {
        // Trigger the Attack animation
        animator.SetTrigger("isAttacking");
    }

    public void TriggerHit()
    {
        // Trigger the Hit animation
        animator.SetTrigger("isHit");
    }

    public void TriggerDie()
    {
        // Trigger the Die animation
        animator.SetBool("isDead", true);
        // Optionally stop the NavMeshAgent to prevent further movement
        navMeshAgent.isStopped = true;
    }
}
