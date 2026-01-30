using UnityEngine;

public class ShootCommand: ICommand
{
    private Shooter shooter;

    public ShootCommand(Shooter _shooter)
    {
        shooter = _shooter;
    }

    public void Execute()
    {
        shooter.Shoot();
    }
    
    public void Undo()
    {

    }
}
