using UnityEngine;
using UnityEngine.UIElements;
public class FireDecorator : WeaponDecorator
{
    public FireDecorator(IWeapon weapon) : base(weapon)
    {
        damageType = DamageType.Fire;
    }

    public override Projectile Fire(Vector3 pos, Quaternion rot, ProjectilePool pool)
    {
        Projectile bullet = base.Fire(pos, rot, pool);
        bullet.GetComponent<Bullet>().damageType = DamageType.Fire;
        return bullet;
    }
}
