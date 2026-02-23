using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("Slider for master volume.")]
    [SerializeField] private Button resumeButton;
    [Tooltip("Slider for music volume.")]
    [SerializeField] private Button mainMenuButton;
    [Tooltip("Slider for SFX volume.")]
    [SerializeField] private Button optionsButton;
    [Tooltip("Button to close the options menu.")]
    [SerializeField] private Button backButton;

    public void Start()
    {
        InitializeUIComponents();
    }

    private void InitializeUIComponents()
    {
        backButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumePreviousState();
        });

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SwitchState(new PlayingState());
        });

        optionsButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SwitchState(new OptionsMenu(), true);
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SwitchState(new MainMenuState(), true);
        });
    }
}
