using UnityEngine;
/// <summary>
/// Follows a target Transform or position.
/// </summary>
public class FollowTargetMotor : CapsuleMover
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

    private void Awake()
    {
        target = useTransformTarget && targetTransform != null ? targetTransform.position : target;
        StopMovement();
    }

    /// <summary>
    /// Handles the movement logic.
    /// </summary>
    override protected void HandleMovement()
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
}
