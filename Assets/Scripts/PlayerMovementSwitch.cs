using UnityEngine;
using TMPro;

public class PlayerMovementSwitch : MonoBehaviour
{
    public enum MovementType
    {
        FreeMovement,
        PointAndClick,
        FollowTarget,
        Tank
    }

    [SerializeField] private MovementType movementType = MovementType.FreeMovement;

    public void Start()
    {
        TMP_Text m_TextComponent = GetComponentInChildren<TMP_Text>();
        m_TextComponent.text = movementType.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter...");
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent(out PlayerController target))
        {
            Debug.Log("Hits Player...");
            collision.gameObject.GetComponent<TankMotor>().enabled = false;
            collision.gameObject.GetComponent<PointAndClickMotor>().enabled = false;
            collision.gameObject.GetComponent<FollowTargetMotor>().enabled = false;
            collision.gameObject.GetComponent<FreeMovementMotor>().enabled = false;

            switch (movementType)
            {
                case MovementType.Tank:
                    collision.gameObject.GetComponent<TankMotor>().enabled = true;
                    break;
                case MovementType.FollowTarget:
                    collision.gameObject.GetComponent<FollowTargetMotor>().enabled = true;
                    break;
                case MovementType.FreeMovement:
                    collision.gameObject.GetComponent<FreeMovementMotor>().enabled = true;
                    break;
                case MovementType.PointAndClick:
                    collision.gameObject.GetComponent<PointAndClickMotor>().enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
