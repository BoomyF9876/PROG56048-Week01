using UnityEngine;

public class DamageBoostDecorator: WeaponDecorator
{
    private int bonusDamage;

    public DamageBoostDecorator(
        IWeapon _weapon,
        int _bonusDamage
    ): base(_weapon)
    {
        bonusDamage = _bonusDamage;
    }

    public override int GetDamage()
    {
        return base.GetDamage() + bonusDamage;
    }

}