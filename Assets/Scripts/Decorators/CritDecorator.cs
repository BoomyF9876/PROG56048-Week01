using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;
public class CritDecorator: WeaponDecorator
{
    protected float critChance = 0.1f;

    public CritDecorator(IWeapon weapon): base(weapon)
    {
    }

    public override Projectile Fire(Vector3 pos, Quaternion rot, ProjectilePool pool)
    {
        Projectile bullet = base.Fire(pos, rot, pool);
        bullet.GetComponent<Bullet>().crit += critChance;
        return bullet;
    }

    public override float GetCrit()
    {
        return base.GetCrit() + critChance;
    }
}
