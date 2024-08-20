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
            // �÷��̾ ���� �������� ����� Idle ���·� ��ȯ�� �� ����
            enemy.SwitchState(new EnemyIdleState());
        }
    }

    public void ExitState()
    {
        Debug.Log("Enemy exiting Chase state.");
    }
}
