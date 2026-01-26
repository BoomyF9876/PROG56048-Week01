using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null &&
            other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            EventBus.Publish(new MotorChangeEvent(MotorType.Tank));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null &&
            other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            switch (PlayerManager.Instance.GetLastState())
            {
                case PointAndClickState:
                    EventBus.Publish(new MotorChangeEvent(MotorType.PointAndClick));
                    break;
                case FollowTargetState:
                    EventBus.Publish(new MotorChangeEvent(MotorType.FollowTarget));
                    break;
                case ExplorationState:
                    EventBus.Publish(new MotorChangeEvent(MotorType.FreeMovement));
                    break;
                default:
                    break;
            }
        }
    }
}
