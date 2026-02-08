using UnityEngine;

public class FireDamageVisitor : IDamageVisitor
{
    public void VisitArmored(ArmoredEnemy enemy, float damage)
    {
        float finalDamage = damage * 1.5f;
        enemy.ApplyDamage(finalDamage, new Color(1f, 0.5f, 0f));
    }

    public void VisitFlying(FlyingEnemy enemy, float damage)
    {
        enemy.ApplyDamage(damage, new Color(1f, 0.5f, 0f));
    }
}
