using UnityEngine;

public class FireDamageVisitor : IDamageVisitor
{
    public void VisitArmored(ArmoredEnemy enemy, int damage)
    {
        int finalDamage = (int)(damage * 1.5f);
        enemy.ApplyDamage(finalDamage, new Color(1f, 0.5f, 0f));
    }

    public void VisitFlying(FlyingEnemy enemy, int damage)
    {
        enemy.ApplyDamage(damage, new Color(1f, 0.5f, 0f));
    }
}
