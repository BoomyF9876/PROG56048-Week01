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
    
    [Header("Collision")]
    [Tooltip("The radius of the capsule.")]
    [SerializeField] private float capsuleRadius = 0.35f;
    [Tooltip("The height of the capsule.")]
    [SerializeField] private float capsuleHeight = 1.8f;
    [Tooltip("The distance at which the bot stops.")]
    [SerializeField] private float collisionDistance = 0.15f;
    [Tooltip("The layer mask for the obstacles.")]
    [SerializeField] private LayerMask obstacleMask = ~0;

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handles the movement of the bot
    /// </summary>
    private void HandleMovement()
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
    /// Stops the movement.
    /// </summary>
    private void StopMovement()
    {
        IsMoving = false;
        IsRunning = false;
        Speed = 0f;
        ForwardSpeed = 0f;
        TurnSpeed = 0f;
        MoveDirection = Vector3.zero;
    }

    /// <summary>
    /// Checks if the character can move in the given direction.
    /// </summary>
    /// <param name="input">Input to check.</param>
    /// <param name="position">Position to check.</param>
    /// <param name="direction">Direction to check.</param>
    /// <returns>True if the character can move in the given direction, false otherwise.</returns>
    private bool CanMove(Vector2 input, Vector3 position, ref Vector3 direction)
    {
        // Try move in desired direction
        bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
        if (canMove) return true;

        // For tank movement, we don't usually slide sideways like WASD motors. 
        direction = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Gets the input from the keyboard
    /// </summary>
    private Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;
        input.x += Keyboard.current.aKey.isPressed ? -1 : 0;
        input.x += Keyboard.current.dKey.isPressed ? +1 : 0;
        input.y += Keyboard.current.wKey.isPressed ? +1 : 0;
        input.y += Keyboard.current.sKey.isPressed ? -1 : 0;
        return input;
    }

    /// <summary>
    /// Gets the normalized input
    /// </summary>
    private Vector2 GetInputNormalized()
    {
        return GetInput().normalized;
    }
}
