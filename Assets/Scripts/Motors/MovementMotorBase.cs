using UnityEngine;
/// <summary>
/// Base contract for movement motors.
/// Motors do movement ONLY. No animation, no shooting, no camera.
/// </summary>
public abstract class MovementMotorBase : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] protected InputReader inputReader;
    protected Vector2 input = Vector2.zero;
    protected CapsuleMover capsuleMover;

    public virtual void Start()
    {
        capsuleMover = GetComponent<CapsuleMover>();
    }

    public virtual void Update()
    {
        if (GameManager.Instance.IsGamePlaying()) HandleMovement();
    }

    protected virtual void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent += OnMove;
            inputReader.SprintEvent += OnSprint;
            inputReader.RightClick += OnRightClick;
        }
    }

    protected virtual void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent -= OnMove;
            inputReader.SprintEvent -= OnSprint;
            inputReader.RightClick -= OnRightClick;
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

    public virtual void OnMove(Vector2 input)
    {
        this.input = input;
    }

    public virtual void OnSprint(bool isSprinting)
    {
        IsRunning = isSprinting;
    }

    public virtual void OnRightClick() { }

    protected virtual void HandleMovement() { }
}
