using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private TMP_Text m_TextComponent;

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        m_TextComponent.text = MotorType.FreeMovement.ToString();
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
        ChangeUIText(data.MotorType);
    }

    private void ChangeUIText(MotorType type)
    {
        m_TextComponent.text = type.ToString();
    }
}
