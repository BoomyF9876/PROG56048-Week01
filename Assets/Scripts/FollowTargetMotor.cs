using UnityEngine;
/// <summary>
/// Follows a target Transform or position.
/// </summary>
public class FollowTargetMotor : MovementMotorBase
{
    [Header("Target")]
    [Tooltip("The target Transform.")]
    [SerializeField] private Transform targetTransform;
    [Tooltip("The target position.")]
    [SerializeField] private Vector3 target = new Vector3(0f, 0f, 0f);
    [Tooltip("Use the target Transform position as the target.")]
    [SerializeField] private bool useTransformTarget = true;

    [Header("Movement")]
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

    [Header("Collision")]
    [Tooltip("The radius of the capsule.")]
    [SerializeField] private float capsuleRadius = 0.35f;
    [Tooltip("The height of the capsule.")]
    [SerializeField] private float capsuleHeight = 1.8f;
    [Tooltip("The distance at which the bot stops.")]
    [SerializeField] private float collisionDistance = 0.15f;
    [Tooltip("The layer mask for the obstacles.")]
    [SerializeField] private LayerMask obstacleMask = ~0;

    private void Awake()
    {
        target = useTransformTarget && targetTransform != null ? targetTransform.position : target;
        StopMovement();
    }

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handles the movement logic.
    /// </summary>
    private void HandleMovement()
    {
        target = useTransformTarget && targetTransform != null ? targetTransform.position : target;

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
    private void StopMovement()
    {
        IsMoving = false;
        IsRunning = false;
        Speed = 0f;
        ForwardSpeed = 0f;
        TurnSpeed = 0f;
        MoveDirection = Vector3.zero;
        // Unlike PointAndClick, we don't reset target here because follow target is passive
    }

    /// <summary>
    /// Checks if the character can move in the given direction.
    /// </summary>
    /// <param name="inputDir">Input direction.</param>
    /// <param name="position">Position to check.</param>
    /// <param name="direction">Direction to check.</param>
    /// <returns>True if the character can move in the given direction, false otherwise.</returns>
    private bool CanMove(Vector3 inputDir, Vector3 position, ref Vector3 direction)
    {
        // Try move in desired direction
        bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
        if (canMove) return true;

        // Try sliding along world axes towards the target
        // Sliding on X
        direction = new Vector3(Mathf.Sign(inputDir.x), 0, 0);
        if (Mathf.Abs(inputDir.x) > 0.01f)
        {
            canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
            if (canMove) return true;
        }

        // Sliding on Z
        direction = new Vector3(0, 0, Mathf.Sign(inputDir.z));
        if (Mathf.Abs(inputDir.z) > 0.01f)
        {
            canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
            if (canMove) return true;
        }

        direction = Vector3.zero;
        return false;
    }
}
