using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<bool> SprintEvent;
    public event Action<bool> JumpEvent;
    public event Action AttackEvent;
    public event Action RightClick;

    private InputSystem_Actions inputActions;

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.SetCallbacks(this);
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();
            inputActions.UI.Disable();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Broadcast the value!
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            AttackEvent?.Invoke(); // Fire!
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnSprint(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    public void OnPrevious(InputAction.CallbackContext context)
    {

    }

    public void OnNext(InputAction.CallbackContext context)
    {

    }

    public void OnPause(InputAction.CallbackContext context)
    {

    }
}
