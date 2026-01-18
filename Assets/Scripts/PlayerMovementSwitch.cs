using UnityEngine;
using TMPro;

public enum MovementType
{
    FreeMovement,
    PointAndClick,
    FollowTarget,
    Tank
}

public class PlayerMovementSwitch : MonoBehaviour
{
    [SerializeField] private MovementType movementType = MovementType.FreeMovement;

    public void Start()
    {
        TMP_Text m_TextComponent = GetComponentInChildren<TMP_Text>();
        m_TextComponent.text = movementType.ToString();
    }

    public MovementType GetMovementType() { return movementType; }
}
