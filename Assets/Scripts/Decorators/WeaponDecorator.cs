using UnityEngine;

public abstract class WeaponDecorator: IWeapon
{
    protected IWeapon wrappedWeapon;
    protected DamageType damageType = DamageType.Physical;

    protected WeaponDecorator(IWeapon weapon)
    {
        wrappedWeapon = weapon;
    }

    public virtual Projectile Fire(Vector3 pos, Quaternion rot, ProjectilePool pool)
    {
        return wrappedWeapon.Fire(pos, rot, pool);
    }

    public virtual int GetDamage()
    {
        return wrappedWeapon.GetDamage();
    }

    public virtual DamageType GetDamageType()
    {
        return damageType;
    }

    public virtual float GetCrit()
    {
        return wrappedWeapon.GetCrit();
    }
}
