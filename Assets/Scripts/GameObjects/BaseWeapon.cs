using UnityEngine;

public class BaseWeapon: IWeapon
{
    private int baseDamage;
    private float crit = 0.0f;

    public BaseWeapon(int damage)
    {
        baseDamage = damage;
    }

    public Projectile Fire(Vector3 position, Quaternion rotation, ProjectilePool pool = null)
    {
        Projectile bullet = pool.Pool.Get();

        bullet.GetComponent<Bullet>().Fire(position, rotation, GetDamage());

        return bullet;
    }

    public int GetDamage()
    {
        return baseDamage;
    }

    public DamageType GetDamageType()
    {
        return DamageType.Physical;
    }

    public float GetCrit()
    {
        return crit;
    }
}
