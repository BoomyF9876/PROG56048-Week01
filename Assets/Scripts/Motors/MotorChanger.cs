using TMPro;
using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public enum MotorType
{
    FreeMovement,
    PointAndClick,
    FollowTarget,
    Tank
};

public struct MotorChangeEvent
{
    public MotorType MotorType;
    public MotorChangeEvent(MotorType type)
    {
        MotorType = type;
    }
}

public class MotorChanger : MonoBehaviour
{
    [SerializeField] private MotorType motorType = MotorType.FreeMovement;

    public void Start()
    {
        TMP_Text m_TextComponent = GetComponentInChildren<TMP_Text>();
        m_TextComponent.text = motorType.ToString();
    }

    public MotorType GetMovementType() { return motorType; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null &&
            other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            EventBus.Publish(new MotorChangeEvent(motorType));
        }
    }
}
