using UnityEngine;

public class JumpCommand : ICommand
{
    private MovementMotorBase motor;
    public JumpCommand(MovementMotorBase motor)
    {
        this.motor = motor;
    }

    public void Execute()
    {

    }

    public void Undo()
    {

    }
}
