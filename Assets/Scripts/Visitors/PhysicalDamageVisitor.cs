using UnityEngine;

public class PhysicalDamageVisitor: IDamageVisitor
{
    public void VisitArmored(ArmoredEnemy enemy, float damage)
    {
        float finalDamage = damage * 0.5f;
        enemy.ApplyDamage(finalDamage, Color.red);
    }

    public void VisitFlying(FlyingEnemy enemy, float damage)
    {
        enemy.ApplyDamage(damage, Color.red);
    }
}
