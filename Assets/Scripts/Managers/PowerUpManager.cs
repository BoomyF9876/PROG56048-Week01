using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager: Singleton<PowerUpManager>
{
    IWeapon weapon;

    public Dictionary<PowerUpType, int> powerUps = new Dictionary<PowerUpType, int>();

    public override void Awake()
    {
        if (weapon == null)
        {
            weapon = new BaseWeapon(5);
        }
    }

    public void AddPowerUp(PowerUpType type)
    {
        if (powerUps.ContainsKey(type))
        {
            powerUps[type] = Mathf.Clamp(powerUps[type] + 1, 0, 5);
        }
        else
        {
            powerUps.Add(type, 1);
        }
        switch (type)
        {
            case PowerUpType.DamageBoost:
                weapon = new DamageBoostDecorator(weapon);
                break;
            case PowerUpType.Crit:
                weapon = new CritDecorator(weapon);
                break;
            case PowerUpType.Burn:
                weapon = new FireDecorator(weapon);
                break;

        }

        //Debug.Log("Damage: " + weapon.GetDamage());
        //Debug.Log("Crit: " + weapon.GetCrit());
        //Debug.Log("Type: " + weapon.GetDamageType().ToString());
        //UIManager.Instance.weaponPanel.PowerUp(type);
    }

    public IWeapon GetCurrentWeapon()
    {
        return weapon;
    }
}
