using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private TMP_Text m_TextComponent;
    private Image m_Image;

    private void Awake()
    {
        m_TextComponent = GetComponentInChildren<TMP_Text>();
        m_Image = transform.Find("ControlImage").GetComponent<Image>();

        ChangeUIText(MotorType.FreeMovement);
        ChangeUIImage(MotorType.FreeMovement);
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
        ChangeUIImage(data.MotorType);
    }

    private void ChangeUIText(MotorType type)
    {
        m_TextComponent.text = "Motor: " + type.ToString();
    }

    private void ChangeUIImage(MotorType type)
    {
        Sprite newSprite = Resources.Load<Sprite>(type.ToString());

        if (newSprite != null)
        {
            m_Image.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Sprite not found in Resources folder: " + type.ToString());
        }
    }
}
