using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Utility class to track mouse position in 3D space and manage a visual pointer.
/// </summary>
[DisallowMultipleComponent]
public class MouseUtil : MonoBehaviour
{
    [Header("Requirements")]
    [Tooltip("The visual object representing the mouse pointer in the world.")]
    [SerializeField] private GameObject mousePointer;
    [Tooltip("Layers to include in the mouse raycast.")]
    [SerializeField] private LayerMask layerMask;

    [Header("Settings")]
    [Tooltip("The maximum distance for the mouse raycast.")]
    [SerializeField] private float maxDistance = 1000f;
    [Tooltip("Whether the pointer should be visible and tracking initially.")]
    [SerializeField] private bool showPointer = true;

    private Vector3 _lastMousePosition;
    private Camera _mainCamera;

    /// <summary>
    /// Gets whether the mouse pointer tracking is currently enabled.
    /// </summary>
    public bool IsEnabled => showPointer;

    /// <summary>
    /// Singleton instance of MouseUtil.
    /// </summary>
    public static MouseUtil Instance { get; private set; }

    private void Awake()
    {
        InitializeSingleton();

        _mainCamera = Camera.main;
        _lastMousePosition = transform.position;

        if (mousePointer == null && transform.childCount > 0)
        {
            mousePointer = transform.GetChild(0).gameObject;
        }

        ValidateComponents();
    }

    private void Update()
    {
        // Toggle tracking with right mouse button
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            TogglePointer();
        }

        if (!showPointer) return;

        UpdateMousePosition();
    }

/// <summary>
/// Initializes the singleton instance of MouseUtil.
/// </summary>
    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Validates the components of the MouseUtil.
    /// </summary>
    private void ValidateComponents()
    {
        if (mousePointer == null)
        {
            Debug.LogError($"[{nameof(MouseUtil)}] Mouse pointer not found! Please assign one in the inspector.", this);
        }

        if (layerMask == 0)
        {
            Debug.LogWarning($"[{nameof(MouseUtil)}] Layer mask is not set. Raycast may not hit anything.", this);
        }
    }

    /// <summary>
    /// Updates the mouse position based on the raycast.
    /// </summary>
    private void UpdateMousePosition()
    {
        if (_mainCamera == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
        {
            transform.position = hit.point;
            _lastMousePosition = hit.point;
        }
    }

    /// <summary>
    /// Gets the last recorded world position of the mouse.
    /// </summary>
    /// <returns>The world position Vector3.</returns>
    public Vector3 GetMousePosition() => _lastMousePosition;

    /// <summary>
    /// Tries to get the current mouse position if tracking is enabled.
    /// </summary>
    /// <param name="position">The output world position.</param>
    /// <returns>True if tracking is enabled, false otherwise.</returns>
    public bool TryGetMousePosition(out Vector3 position)
    {
        position = _lastMousePosition;
        return showPointer;
    }

    /// <summary>
    /// Toggles the mouse pointer tracking and visibility.
    /// </summary>
    public void TogglePointer()
    {
        showPointer = !showPointer;
        if (mousePointer != null)
        {
            mousePointer.SetActive(showPointer);
        }
    }
}
