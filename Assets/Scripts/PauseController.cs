using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public void Start()
    {
        GameManager.Instance.SwitchState(new PlayingState());
    }

    public void ResumeGame()
    {
        GameManager.Instance.SwitchState(new PlayingState());
    }

    public void PauseGame()
    {
        GameManager.Instance.SwitchState(new PauseState());
    }

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            switch (GameManager.Instance.GetCurrentState().GetType().ToString())
            {
                case "PlayingState":
                    PauseGame();
                    break;
                case "PauseState":
                    ResumeGame();
                    break;
                default:
                    break;
            }
        }
    }
}
