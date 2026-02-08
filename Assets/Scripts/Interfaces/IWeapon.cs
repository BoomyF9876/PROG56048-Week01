using UnityEngine;

public interface IWeapon
{
    Projectile Fire(Vector3 pos, Quaternion rot, ProjectilePool pool);
    int GetDamage();

    float GetCrit();

    DamageType GetDamageType();
}
