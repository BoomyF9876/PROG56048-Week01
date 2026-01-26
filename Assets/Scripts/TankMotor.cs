using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Tank-style movement motor
/// </summary>
public class TankMotor : CapsuleMover
{
    [Header("Movement")]
    [Tooltip("Movement speed")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("Running speed")]
    [SerializeField] private float runSpeed = 4f;
    [Tooltip("Rotation speed")]
    [SerializeField] private float rotationSpeed = 180f;

    /// <summary>
    /// Handles the movement of the bot
    /// </summary>
    override protected void HandleMovement()
    {
        Vector2 input = GetInput();

        if (input == Vector2.zero)
        {
            StopMovement();
            return;
        }

        IsRunning = Keyboard.current.shiftKey.isPressed;
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
        IsMoving = CanMove(input, transform.position, ref moveDir);
        
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

    /// <summary>
    /// Checks if the character can move in the given direction.
    /// </summary>
    /// <param name="input">Input to check.</param>
    /// <param name="position">Position to check.</param>
    /// <param name="direction">Direction to check.</param>
    /// <returns>True if the character can move in the given direction, false otherwise.</returns>
    protected bool CanMove(Vector2 input, Vector3 position, ref Vector3 direction)
    {
        // Try move in desired direction
        bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
        if (canMove) return true;

        // For tank movement, we don't usually slide sideways like WASD motors. 
        direction = Vector3.zero;
        return false;
    }
}
