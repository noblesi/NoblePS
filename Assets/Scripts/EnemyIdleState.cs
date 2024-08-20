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
        // 플레이어와 일정 거리 이내로 접근하면 추적 상태로 전환
        // 예: Player를 탐지하는 로직 추가
    }

    public void ExitState()
    {
        Debug.Log("Enemy existing IdleState.");
    }
}
