using System;
using UnityEngine;
/// <summary>
/// Camera controller
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The animator component.")]
    [SerializeField] private Animator animator;
    
    [Header("Settings")]
    [Tooltip("The camera mode.")]
    [SerializeField] private CameraMode cameraMode;

    /// <summary>
    /// The camera mode.
    /// </summary>
    public enum CameraMode {
        PointAndClick,
        FollowTarget,
        Tank,
        FreeMovement
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ChangeCameraMode(cameraMode.ToString());
    }

    private void OnEnable()
    {
        EventBus.Subscribe<MotorChangeEvent>(OnMotorChange);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<MotorChangeEvent>(OnMotorChange);
    }

    private void OnMotorChange(MotorChangeEvent data)
    {
        ChangeCameraMode(data.MotorType.ToString());
    }

    private void ChangeCameraMode(string type)
    {
        Enum.TryParse(type, out CameraMode parsed);
        cameraMode = parsed;
        animator.SetTrigger(type);
    }
}