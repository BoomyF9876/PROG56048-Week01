using UnityEngine;

public class PowerUpManager: MonoBehaviour
{
    IWeapon weapon;

    public void AddPowerUp(PowerUpType type)
    {

    }

    public IWeapon GetCurrentWeapon()
    {
        return weapon;
    }
}
