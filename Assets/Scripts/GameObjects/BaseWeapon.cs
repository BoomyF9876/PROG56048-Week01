using UnityEngine;

public class BaseWeapon: IWeapon
{
    private int baseDamage;

    public BaseWeapon(int damage)
    {
        baseDamage = damage;
    }

    public void Fire()
    {

    }

    public int GetDamage()
    {
        return baseDamage;
    }
}
