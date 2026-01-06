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
    }
  
    private void OnEnable()
    {
        animator.SetTrigger(cameraMode.ToString());
    }
}