using UnityEngine;

public abstract class Monster : CharacterBase
{
    protected MonsterFSM fsm;

    protected virtual void Start()
    {
        fsm = new MonsterFSM(this);
    }

    private void Update()
    {
        fsm.UpdateState();
    }

    public override void Move(Vector3 direction)
    {
        // Implement monster movement logic
    }

    protected override void Die()
    {
        // Implement monster death logic
    }
}
