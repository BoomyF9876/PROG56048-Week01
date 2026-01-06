using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Free movement motor that allows WASD movement in world/relative space 
/// with collision sliding.
/// </summary>
public class FreeMovementMotor : MovementMotorBase
{
    [Header("Movement")]
    [Tooltip("The speed at which the bot moves.")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("The speed at which the bot runs.")]
    [SerializeField] private float runMultiplier = 2f;
    [Tooltip("The speed at which the bot rotates.")]
    [SerializeField] private float turnSpeed = 10f;

    [Header("Collision")]
    [Tooltip("The radius of the capsule.")]
    [SerializeField] private float capsuleRadius = 0.35f;
    [Tooltip("The height of the capsule.")]
    [SerializeField] private float capsuleHeight = 1.8f;
    [Tooltip("The distance at which the bot stops.")]
    [SerializeField] private float collisionDistance = 0.15f;
    [Tooltip("The layer mask for the obstacles.")]
    [SerializeField] private LayerMask obstacleMask = ~0;

    private float currentSpeed;
    private Vector3 moveDir;

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handles movement logic.
    /// </summary>
    private void HandleMovement()
    {
        Vector2 input = GetInput();
        Vector3 inputDir = new Vector3(input.x, 0f, input.y);
        float inputMag = Mathf.Clamp01(inputDir.magnitude);
        
        bool runHeld = Keyboard.current.shiftKey.isPressed;
        float targetSpeed = moveSpeed * (runHeld ? runMultiplier : 1f) * inputMag;

        // Apply acceleration
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, moveSpeed * runMultiplier * Time.deltaTime);

        // Movement Status
        if (inputMag < 0.01f && currentSpeed < 0.01f)
        {
            StopMovement();
            return;
        }

        // Determine desired direction
        moveDir = inputDir.normalized;
        if (moveDir == Vector3.zero) moveDir = transform.forward;

        // Collision check and slide
        IsMoving = CanMove(input, transform.position, ref moveDir) && currentSpeed > 0.01f;
        IsRunning = runHeld && IsMoving;
        Speed = currentSpeed;
        ForwardSpeed = IsMoving ? currentSpeed : 0f;

        if (IsMoving)
        {
            transform.position += moveDir * currentSpeed * Time.deltaTime;
            MoveDirection = moveDir;

            // Rotation toward movement
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            float angleBefore = transform.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            float angleAfter = transform.eulerAngles.y;
            TurnSpeed = Mathf.DeltaAngle(angleBefore, angleAfter) / Time.deltaTime;
        }
        else
        {
            TurnSpeed = 0f;
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
        moveDir = Vector3.zero;
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

        // Sliding on X
        direction = new Vector3(input.x, 0, 0).normalized;
        if (direction != Vector3.zero)
        {
            canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
            if (canMove) return true;
        }

        // Sliding on Z
        direction = new Vector3(0, 0, input.y).normalized;
        if (direction != Vector3.zero)
        {
            canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
            if (canMove) return true;
        }

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
