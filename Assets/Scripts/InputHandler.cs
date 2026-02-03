using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Shooter shooter;
    [SerializeField] private MovementMotorBase motor;

    // The "Button" slots (Commands)
    private ICommand fireCommand;
    private ICommand jumpCommand;

    private void Start()
    {
        if (shooter == null)
        {
            shooter = GetComponent<Shooter>();
        }

        fireCommand = new ShootCommand(shooter);
        //jumpCommand = new JumpCommand(motor);
    }

    private void OnEnable()
    {
        inputReader.AttackEvent += OnAttack;
        inputReader.JumpEvent += OnJump;
    }

    private void OnDisable()
    {
        inputReader.AttackEvent -= OnAttack;
        inputReader.JumpEvent -= OnJump;
    }

    private void OnAttack() => fireCommand?.Execute();
    private void OnJump(bool pressed)
    {
        if (pressed) jumpCommand?.Execute();
    }
}
