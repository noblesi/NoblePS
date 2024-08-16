using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : CharacterBase
{
    public int experience;
    public int level;

    private PlayerInputActions inputActions;
    private PlayerMovement playerMovement;

    [Header("Events")]
    public UnityEvent<Vector2> onMove;
    public UnityEvent onAttack;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        playerMovement = GetComponent<PlayerMovement>();

        inputActions.Player.Move.performed += context => onMove.Invoke(context.ReadValue<Vector2>());
        inputActions.Player.Attack.performed += context => { if (context.ReadValue<float>() > 0) onAttack.Invoke(); };
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        onMove.AddListener(playerMovement.MoveToPointer);
        onAttack.AddListener(playerMovement.TriggerAttack);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        playerMovement.TriggerHit();

        if (health <= 0)
        {
            playerMovement.TriggerDie();
        }
    }

    public override void Move(Vector3 direction)
    {
        // This method can be left empty or refactored if needed
    }

    protected override void Die()
    {
        playerMovement.TriggerDie();
        // Implement player death logic
    }
}
