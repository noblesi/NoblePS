public class MonsterFSM
{
    private Monster monster;
    private MonsterState currentState;

    public MonsterFSM(Monster monster)
    {
        this.monster = monster;
        currentState = MonsterState.Idle;
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                // Handle idle state
                break;
            case MonsterState.Chase:
                // Handle chase state
                break;
            case MonsterState.Attack:
                // Handle attack state
                //monster.Attack(/* target */);
                break;
        }
    }

    public void ChangeState(MonsterState newState)
    {
        currentState = newState;
    }
}

public enum MonsterState
{
    Idle,
    Chase,
    Attack
}
