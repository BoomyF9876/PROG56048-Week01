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
    [SerializeField] private PlayerController player;

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
        player = GetComponent<PlayerController>();

        //idleHash = Animator.StringToHash(idle);
        //walkHash = Animator.StringToHash(walk);
        //runHash = Animator.StringToHash(run);
        turnLeftHash = Animator.StringToHash(turnLeft);
        turnRightHash = Animator.StringToHash(turnRight);
        speedHash = Animator.StringToHash(speed);
        forwardSpeedHash = Animator.StringToHash(forwardSpeed);
        turnSpeedHash = Animator.StringToHash(turnSpeed);
    }

    private void OnMotorChange(MotorChangeEvent data)
    {
        LateUpdate();
    }

    private void LateUpdate()
    {
        if (animator == null || player.motor == null) return;

        // bumped up from 0.1 to 0.5 because of the gamepad input for turning while idle
        bool isMovingLinearly = Mathf.Abs(player.motor.ForwardSpeed) > 0.5f;
        bool isTurningInPlace = !isMovingLinearly && Mathf.Abs(player.motor.TurnSpeed) > 0.01f;

        //bool isIdle = !isMovingLinearly;
        //bool isRun = isMovingLinearly && player.motor.IsRunning;
        //bool isWalk = isMovingLinearly && !player.motor.IsRunning;
        bool isTurnLeft = isTurningInPlace && player.motor.TurnSpeed < 0;
        bool isTurnRight = isTurningInPlace && player.motor.TurnSpeed > 0;

        //animator.SetBool(idleHash, isIdle);
        //animator.SetBool(walkHash, isWalk);
        //animator.SetBool(runHash, isRun);

        animator.SetFloat(speedHash, player.motor.Speed);
        animator.SetFloat(forwardSpeedHash, player.motor.ForwardSpeed);
        animator.SetFloat(turnSpeedHash, player.motor.TurnSpeed);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<MotorChangeEvent>(OnMotorChange);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<MotorChangeEvent>(OnMotorChange);
    }

}
