public interface IEnemyState
{
    void EnterState(EnemyController enemy);
    void UpdateState();
    void ExitState();
}
