public abstract class WeaponDecorator: IWeapon
{
    protected IWeapon wrappedWeapon;

    protected WeaponDecorator(IWeapon weapon)
    {
        wrappedWeapon = weapon;
    }

    public virtual void Fire()
    {
        wrappedWeapon.Fire();
    }

    public virtual int GetDamage()
    {
        return wrappedWeapon.GetDamage();
    }
}
