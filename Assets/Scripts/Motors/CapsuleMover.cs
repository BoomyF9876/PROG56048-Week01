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

    /// <summary>
    /// Checks if the character can move in the given direction.
    /// </summary>
    /// <param name="inputDir">Input direction.</param>
    /// <param name="position">Position to check.</param>
    /// <param name="direction">Direction to check.</param>
    /// <returns>True if the character can move in the given direction, false otherwise.</returns>
    public bool CanMove(Vector3 inputDir, Vector3 position, ref Vector3 direction)
    {
        // Try move in desired direction
        bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * capsuleHeight, capsuleRadius, direction, collisionDistance, obstacleMask);
        if (canMove) return true;

        // No Sliding for TankMotor
        if (!AllowSliding)
        {
            direction = Vector3.zero;
            return false;
        }

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

    /// <summary>
    /// Gets the input from the keyboard
    /// </summary>
    protected Vector2 GetInput()
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
    protected Vector2 GetInputNormalized()
    {
        return GetInput().normalized;
    }
}
