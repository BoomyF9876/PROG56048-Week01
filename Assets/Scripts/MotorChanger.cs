using TMPro;
using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public struct MotorChangeEvent
{
    public MotorChanger.MotorType MotorType;
    public MotorChangeEvent(MotorChanger.MotorType type)
    {
        MotorType = type;
    }
}

public class MotorChanger : MonoBehaviour
{
    public enum MotorType
    {
        FreeMovement,
        PointAndClick,
        FollowTarget,
        Tank
    };

    [SerializeField] private MotorType motorType = MotorType.FreeMovement;

    public void Start()
    {
        TMP_Text m_TextComponent = GetComponentInChildren<TMP_Text>();
        m_TextComponent.text = motorType.ToString();
    }

    public MotorType GetMovementType() { return motorType; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null &&
            other.transform.parent.TryGetComponent(out PlayerController playerController))
        {
            EventBus.Publish(new MotorChangeEvent(motorType));
        }
    }
}
