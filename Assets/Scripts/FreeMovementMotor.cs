using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Free movement motor that allows WASD movement in world/relative space 
/// with collision sliding.
/// </summary>
public class FreeMovementMotor : CapsuleMover
{
    [Header("Movement")]
    [Tooltip("The speed at which the bot moves.")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("The speed at which the bot runs.")]
    [SerializeField] private float runMultiplier = 2f;
    [Tooltip("The speed at which the bot rotates.")]
    [SerializeField] private float turnSpeed = 10f;

    private float currentSpeed;
    private Vector3 moveDir;

    /// <summary>
    /// Handles movement logic.
    /// </summary>
    override protected void HandleMovement()
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
    new private void StopMovement()
    {
        base.StopMovement();
        moveDir = Vector3.zero;
    }
}
