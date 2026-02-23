using UnityEngine;

public class DamageBoostDecorator: WeaponDecorator
{
    private int bonusDamage = 5;

    public DamageBoostDecorator(IWeapon _weapon): base(_weapon)
    {
    }

    public override Projectile Fire(Vector3 pos, Quaternion rot, ProjectilePool pool)
    {
        Projectile bullet = base.Fire(pos, rot, pool);
        bullet.GetComponent<Bullet>().SetDamage(GetDamage());
        return bullet;
    }

    public override int GetDamage()
    {
        return base.GetDamage() + bonusDamage;
    }
}