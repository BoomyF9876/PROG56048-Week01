using UnityEngine;

public class PhysicalDamageVisitor: IDamageVisitor
{
    public void VisitArmored(ArmoredEnemy enemy, int damage)
    {
        int finalDamage = (int)(damage * 0.5f);
        enemy.ApplyDamage(finalDamage, Color.red);
    }

    public void VisitFlying(FlyingEnemy enemy, int damage)
    {
        enemy.ApplyDamage(damage, Color.red);
    }
}
