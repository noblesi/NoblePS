using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyController enemy;

    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        Debug.Log("Enemy entered Chase state.");
    }

    public void UpdateState()
    {
        if (enemy.IsPlayerInRange())
        {
            enemy.MoveTo(enemy.Player.transform.position);

            if (enemy.IsPlayerInRange())
            {
                enemy.SwitchState(new EnemyAttackState());
            }
        }
        else
        {
            // 플레이어가 인지 범위에서 벗어나면 Idle 상태로 전환할 수 있음
            enemy.SwitchState(new EnemyIdleState());
        }
    }

    public void ExitState()
    {
        Debug.Log("Enemy exiting Chase state.");
    }
}
