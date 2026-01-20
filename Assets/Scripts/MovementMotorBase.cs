using UnityEngine;
/// <summary>
/// Base contract for movement motors.
/// Motors do movement ONLY. No animation, no shooting, no camera.
/// </summary>
public abstract class MovementMotorBase : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] protected InputReader inputReader;

    protected virtual void OnEnable()
    {
        if (inputReader != null)
        {
            //inputReader.MoveEvent += OnMove;
        }
    }

    /// <summary>True if the player is moving (based on motor logic).</summary>
    public bool IsMoving { get; protected set; }

    /// <summary>True if the motor considers the player "running".</summary>
    public bool IsRunning { get; protected set; }

    /// <summary>Current speed in units/sec. Used later for blend trees.</summary>
    public float Speed { get; protected set; }

    /// <summary>Forward/backward speed (can be negative for reverse).</summary>
    public float ForwardSpeed { get; protected set; }

    /// <summary>Rotation speed (can be negative for left turn).</summary>
    public float TurnSpeed { get; protected set; }

    /// <summary>World-space movement direction (normalized when moving).</summary>
    public Vector3 MoveDirection { get; protected set; }
}
