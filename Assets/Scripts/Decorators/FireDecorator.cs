using UnityEngine;
public class FireDecorator : WeaponDecorator
{
    public FireDecorator(IWeapon weapon) : base(weapon)
    {
    }

    public override void Fire()
    {
        //TODO: WeaponType = fire;
        wrappedWeapon.Fire();
    }

    public override int GetDamage()
    {
        return base.GetDamage();
    }
}
