using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        Debug.Log("Enemy entered IdleState.");
    }

    public void UpdateState()
    {
        // �÷��̾�� ���� �Ÿ� �̳��� �����ϸ� ���� ���·� ��ȯ
        // ��: Player�� Ž���ϴ� ���� �߰�
    }

    public void ExitState()
    {
        Debug.Log("Enemy existing IdleState.");
    }
}
