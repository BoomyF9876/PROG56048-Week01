using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_Text m_TextComponent;
    [SerializeField] private Image m_Image;
    public PowerUpPanel panel;
    public int enemiesExisted = 2;

    override public void Awake()
    {
        ChangeUIText(MotorType.FreeMovement);
        ChangeUIImage(MotorType.FreeMovement);
    }

    private void Update()
    {
        if (enemiesExisted <= 0)
        {
            GameManager.Instance.SwitchState(new NextLevelState());
        }
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
        if (m_TextComponent == null) return;
        m_TextComponent.text = "Motor: " + type.ToString();
    }

    private void ChangeUIImage(MotorType type)
    {
        if (m_Image == null) return;
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
