using UnityEngine;
/// <summary>
/// Manages animator states based on movement motor conditions.
/// </summary>
public class LocomotionAnimator : MonoBehaviour
{
    [Header("Requirements")]
    [Tooltip("The animator component to control.")]
    [SerializeField] private Animator animator;
    [Tooltip("The movement motor to track.")]
    [SerializeField] private MovementMotorBase motor;

    [Header("Animator Params")]
    [Tooltip("The name of the idle state.")]
    [SerializeField] private string idle = "Idle";
    [Tooltip("The name of the walk state.")]
    [SerializeField] private string walk = "Walking";
    [Tooltip("The name of the run state.")]
    [SerializeField] private string run = "Running";
    [Tooltip("The name of the turn left state.")]
    [SerializeField] private string turnLeft = "TurnLeft";
    [Tooltip("The name of the turn right state.")]
    [SerializeField] private string turnRight = "TurnRight";
    [Tooltip("The name of the speed parameter.")]
    [SerializeField] private string speed = "Speed";
    [Tooltip("The name of the forward speed parameter.")]
    [SerializeField] private string forwardSpeed = "ForwardSpeed";
    [Tooltip("The name of the turn speed parameter.")]
    [SerializeField] private string turnSpeed = "TurnSpeed";

    private int idleHash, walkHash, runHash, turnLeftHash, turnRightHash;
    private int speedHash, forwardSpeedHash, turnSpeedHash;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        MovementMotorBase[] motors = GetComponents<MovementMotorBase>();
        foreach (MovementMotorBase motor in motors)
        {
            if(motor.enabled)
            {
                this.motor = motor;
                break;
            }
        }
        if(motor == null)
        {
            Debug.LogError($"[{nameof(LocomotionAnimator)}] No movement motor found");
        }

        idleHash = Animator.StringToHash(idle);
        walkHash = Animator.StringToHash(walk);
        runHash = Animator.StringToHash(run);
        turnLeftHash = Animator.StringToHash(turnLeft);
        turnRightHash = Animator.StringToHash(turnRight);
        speedHash = Animator.StringToHash(speed);
        forwardSpeedHash = Animator.StringToHash(forwardSpeed);
        turnSpeedHash = Animator.StringToHash(turnSpeed);
    }

    private void LateUpdate()
    {
        if (animator == null || motor == null) return;

        bool isMovingLinearly = Mathf.Abs(motor.ForwardSpeed) > 0.01f;
        bool isTurningInPlace = !isMovingLinearly && Mathf.Abs(motor.TurnSpeed) > 0.01f;

        bool isIdle = motor.ForwardSpeed == 0f;
        bool isRun = isMovingLinearly && motor.IsRunning;
        bool isWalk = isMovingLinearly && !motor.IsRunning;
        bool isTurnLeft = isTurningInPlace && motor.TurnSpeed < 0;
        bool isTurnRight = isTurningInPlace && motor.TurnSpeed > 0;

        animator.SetBool(idleHash, isIdle);
        animator.SetBool(walkHash, isWalk);
        animator.SetBool(runHash, isRun);

        animator.SetFloat(speedHash, motor.Speed);
        animator.SetFloat(forwardSpeedHash, motor.ForwardSpeed);
        animator.SetFloat(turnSpeedHash, motor.TurnSpeed);
    }
}
