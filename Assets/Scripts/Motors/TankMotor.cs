using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Tank-style movement motor
/// </summary>
public class TankMotor : MovementMotorBase
{
    [Header("Movement")]
    [Tooltip("Movement speed")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("Running speed")]
    [SerializeField] private float runSpeed = 4f;
    [Tooltip("Rotation speed")]
    [SerializeField] private float rotationSpeed = 180f;

    override public void Start()
    {
        base.Start();
        capsuleMover.AllowSliding = false;
    }

    /// <summary>
    /// Handles the movement of the bot
    /// </summary>
    override protected void HandleMovement()
    {
        if (input == Vector2.zero)
        {
            StopMovement();
            return;
        }

        Speed = IsRunning ? runSpeed : moveSpeed;
        
        // Rotation in place (independent of collision for now)
        float yaw = input.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f, Space.Self);
        TurnSpeed = input.x * rotationSpeed;

        if (Mathf.Abs(input.y) < 0.01f)
        {
            ForwardSpeed = 0f;
            IsMoving = false;
            MoveDirection = Vector3.zero;
            return;
        }

        // Determine move direction
        Vector3 moveDir = transform.forward * Mathf.Sign(input.y);
        
        // Collision check
        IsMoving = capsuleMover.CanMove(input, transform.position, ref moveDir);
        
        if (IsMoving)
        {
            ForwardSpeed = input.y * Speed;
            Vector3 moveVector = moveDir * (Mathf.Abs(input.y) * Speed * Time.deltaTime);
            transform.position += moveVector;
            MoveDirection = moveDir;
        }
        else
        {
            ForwardSpeed = 0f;
            MoveDirection = Vector3.zero;
        }
    }

    protected void StopMovement()
    {
        IsMoving = false;
        Speed = 0f;
        ForwardSpeed = 0f;
        TurnSpeed = 0f;
        MoveDirection = Vector3.zero;
    }
}
