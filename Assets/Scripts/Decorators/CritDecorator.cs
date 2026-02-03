using UnityEngine;
public class CritDecorator: WeaponDecorator
{
    protected int critChance = 10;

    public CritDecorator(IWeapon weapon): base(weapon)
    {
        //TODO: Check how many layers of crit
        critChance = 10;
    }

    public override void Fire()
    {
        int critIndex = Random.Range(0, 99);
        bool isCrit = critIndex % (critChance > 2 ? critChance : 2) == 0;

        wrappedWeapon.Fire();
    }

    public override int GetDamage()
    {
        return base.GetDamage();
    }
}
