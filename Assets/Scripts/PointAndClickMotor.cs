using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Point and click movement motor.
/// </summary>
public class PointAndClickMotor : CapsuleMover
{
    [Header("Settings")]
    [Tooltip("The speed at which the bot moves.")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("The speed at which the bot runs.")]
    [SerializeField] private float runSpeed = 4f;
    [Tooltip("The speed at which the bot rotates.")]
    [SerializeField] private float rotationSpeed = 10f;
    [Tooltip("The distance at which the bot stops.")]
    [SerializeField] private float stopDistance = 0.25f;
    [Tooltip("The distance at which the bot runs.")]
    [SerializeField] private float runningThreshold = 4f;

    [Header("State")]
    [Tooltip("The target position.")]
    [SerializeField] private Vector3 target;

    private void Awake()
    {
        target = transform.position;
        StopMovement();
    }

    /// <summary>
    /// Handles the movement logic.
    /// </summary>
    override protected void HandleMovement()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (!MouseUtil.Instance.TryGetMousePosition(out Vector3 mousePos)) return;
            target = mousePos;
        }

        Vector3 toTarget = target - transform.position;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;
        if (distance <= stopDistance)
        {
            StopMovement();
            return;
        }

        IsRunning = distance > runningThreshold;
        Speed = IsRunning ? runSpeed : moveSpeed;
        
        Vector3 moveDir = toTarget.normalized;
        
        // Collision check and slide
        IsMoving = CanMove(moveDir, transform.position, ref moveDir);
        MoveDirection = moveDir;

        if (IsMoving)
        {
            ForwardSpeed = Speed;
            transform.position += moveDir * Speed * Time.deltaTime;

            // Rotate toward movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
        else
        {
            ForwardSpeed = 0f;
        }
    }

    /// <summary>
    /// Stops the movement.
    /// </summary>
    new private void StopMovement()
    {
        base.StopMovement();
        target = transform.position; // Reset target to current position to stop trying to move
    }

}
