using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyController enemy;

    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        Debug.Log("Enemy entered Attack state.");
    }

    public void UpdateState()
    {
        enemy.Attack();

        if (!enemy.IsPlayerInRange())
        {
            enemy.SwitchState(new EnemyChaseState());
        }
    }

    public void ExitState()
    {
        Debug.Log("Enemy exiting Attack state.");
    }
}
