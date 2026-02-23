using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Simple camera controller for the PCG scene.
/// Provides WASD movement, Q/E rotation, and scroll wheel zoom controls.
/// </summary>
public class CameraControllerPCG : MonoBehaviour
{
    #region Serialized Fields

    [Header("Movement Settings")]
    [Tooltip("Speed of camera movement using WASD keys.")]
    [SerializeField] private float moveSpeed = 10f;

    [Tooltip("Speed of camera rotation using Q/E keys.")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Zoom Settings")]
    [Tooltip("Speed of camera zoom using scroll wheel.")]
    [SerializeField] private float zoomSpeed = 500f;

    [Tooltip("Minimum zoom distance (closest to ground).")]
    [SerializeField] private float minZoomDistance = 2f;

    [Tooltip("Maximum zoom distance (furthest from ground).")]
    [SerializeField] private float maxZoomDistance = 15f;

    #endregion

    #region Private Fields

    private Camera mainCamera;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError($"[{nameof(CameraControllerPCG)}] No main camera found!");
            enabled = false;
        }
    }

    private void Update()
    {
        // Early exit if input system unavailable
        if (Keyboard.current == null || Mouse.current == null)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    #endregion

    #region Camera Controls

    /// <summary>
    /// Handles camera movement based on WASD input.
    /// Movement is relative to camera facing direction on the XZ plane.
    /// </summary>
    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Keyboard.current.dKey.isPressed)
        {
            moveDirection += GetCameraRelativeDirection(mainCamera.transform.right);
        }

        if (Keyboard.current.aKey.isPressed)
        {
            moveDirection += GetCameraRelativeDirection(-mainCamera.transform.right);
        }

        if (Keyboard.current.wKey.isPressed)
        {
            moveDirection += GetCameraRelativeDirection(mainCamera.transform.forward);
        }

        if (Keyboard.current.sKey.isPressed)
        {
            moveDirection += GetCameraRelativeDirection(-mainCamera.transform.forward);
        }

        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
            mainCamera.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Handles camera rotation based on Q/E input.
    /// Rotates around the world Y axis.
    /// </summary>
    private void HandleRotation()
    {
        float rotationAmount = 0f;

        if (Keyboard.current.qKey.isPressed)
        {
            rotationAmount = -1f;
        }
        else if (Keyboard.current.eKey.isPressed)
        {
            rotationAmount = 1f;
        }

        if (rotationAmount != 0f)
        {
            mainCamera.transform.Rotate(Vector3.up, rotationAmount * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Handles camera zoom based on scroll wheel input.
    /// Adjusts the camera's Y position within configured bounds.
    /// </summary>
    private void HandleZoom()
    {
        float scrollInput = Mouse.current.scroll.ReadValue().y;

        if (scrollInput != 0f)
        {
            Vector3 currentPosition = mainCamera.transform.localPosition;
            float newYPosition = currentPosition.y - scrollInput * zoomSpeed * Time.deltaTime;
            newYPosition = Mathf.Clamp(newYPosition, minZoomDistance, maxZoomDistance);

            mainCamera.transform.localPosition = new Vector3(
                currentPosition.x,
                newYPosition,
                currentPosition.z
            );
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets a flattened (Y = 0) and normalized direction vector relative to the camera.
    /// </summary>
    /// <param name="direction">The camera-relative direction vector.</param>
    /// <returns>The flattened and normalized direction for XZ plane movement.</returns>
    private Vector3 GetCameraRelativeDirection(Vector3 direction)
    {
        direction.y = 0f;
        return direction.normalized;
    }

    #endregion
}
