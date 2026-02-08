using UnityEngine;
using UnityEngine.InputSystem;

public class CapsuleMover : MonoBehaviour
{
    [Header("Collision")]
    [Tooltip("The radius of the capsule.")]
    [SerializeField] protected float capsuleRadius = 0.35f;
    [Tooltip("The height of the capsule.")]
    [SerializeField] protected float capsuleHeight = 1.8f;
    [Tooltip("The distance at which the bot stops.")]
    [SerializeField] protected float collisionDistance = 0.15f;
    [Tooltip("The layer mask for the obstacles.")]
    [SerializeField] protected LayerMask obstacleMask = ~0;

    public bool AllowSliding = true;

    public bool TryApplyMovement(Vector3 inputDir, Vector3 position, ref Vector3 direction)
    {
        // Try move in desired direction
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * capsuleHeight, capsuleRadius, inputDir, collisionDistance, obstacleMask);
        if (canMove)
        {
            direction = inputDir;
            return true;
        }

        // No Sliding for TankMotor
        if (!AllowSliding)
        {
            direction = Vector3.zero;
            return false;
        }

        // Try sliding along world axes towards the target
        // Sliding on X
        direction = new Vector3(inputDir.x, 0, 0).normalized;
        if (direction != Vector3.zero)
        {
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
            if (canMove) return true;
        }

        // Sliding on Z
        direction = new Vector3(0, 0, inputDir.y).normalized;
        if (direction != Vector3.zero)
        {
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
            if (canMove) return true;
        }

        direction = Vector3.zero;
        return false;
    }
}
