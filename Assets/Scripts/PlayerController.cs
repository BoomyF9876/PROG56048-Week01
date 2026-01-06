using UnityEngine;
/// <summary>
/// Player controller that handles movement and input.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Show gizmos for debugging")]
    [SerializeField] private bool showGizmos = true;
    private MovementMotorBase motor;

    private void Start()
    {
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
            Debug.LogError($"[{nameof(PlayerController)}] No movement motor found");
        }
    }

    /// <summary>
    /// Draws gizmos for debugging
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (motor == null) return;

        // Determine gizmo direction (forward or backward based on ForwardSpeed)
        Vector3 dynamicDir = transform.forward;
        if (motor.ForwardSpeed < -0.01f)
        {
            dynamicDir = -transform.forward;
        }

        #region RayCast gizmo
        float gizmoLength = 0.5f;  // Length of our Raycast (maxDistance)
        float gizmoHeight = 1f;    // Height offset above the pivot
        // Change the Gizmos colour 
        Gizmos.color = Color.green;
        // Calculate start point (elevated above pivot)
        Vector3 startPoint = transform.position + Vector3.up * gizmoHeight;
        // Calculate end point (start + forward direction scaled by length)
        Vector3 endPoint = startPoint + dynamicDir * gizmoLength;
        // Draw the main line
        Gizmos.DrawLine(startPoint, endPoint);
        // Draw a small sphere at the end for an "arrowhead" effect
        Gizmos.DrawSphere(endPoint, 0.05f);  // Tiny sphere as a tip
        #endregion

        #region CapsuleCast gizmo
        float radius = 0.25f;
        float height = 1.8f;
        Gizmos.color = Color.yellow;
        float cylinderTopHeight = height - radius;
        float cylinderBottom = radius;

        // Calculate the positions of the two sphere centers, offset in the movement direction
        Vector3 topSphereCenter = transform.position + transform.up * cylinderTopHeight + dynamicDir * radius;
        Vector3 bottomSphereCenter = transform.position + transform.up * cylinderBottom + dynamicDir * radius;

        // Draw the two wire spheres at the ends of the capsule
        Gizmos.DrawWireSphere(topSphereCenter, radius);
        Gizmos.DrawWireSphere(bottomSphereCenter, radius);

        // Draw the connecting lines for the cylindrical part
        // These lines connect the edges of the spheres to form the cylinder outline
        Vector3 rightOffset = transform.right * radius;
        Vector3 orthogonalForwardOffset = Vector3.Cross(transform.up, rightOffset).normalized * radius;
        // Ensure the cylinder sides are always correctly aligned with the movement direction for visualization
        Vector3 movementOffset = dynamicDir * radius;

        // Draw the four main connecting lines
        Gizmos.DrawLine(topSphereCenter + rightOffset, bottomSphereCenter + rightOffset);
        Gizmos.DrawLine(topSphereCenter - rightOffset, bottomSphereCenter - rightOffset);
        Gizmos.DrawLine(topSphereCenter + movementOffset, bottomSphereCenter + movementOffset);
        Gizmos.DrawLine(topSphereCenter - movementOffset, bottomSphereCenter - movementOffset);
        #endregion
    }
}
