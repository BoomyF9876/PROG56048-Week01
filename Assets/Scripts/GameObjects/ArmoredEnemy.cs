using UnityEngine;

public class ArmoredEnemy: MonoBehaviour, IVisitableEnemy
{
    private HealthComponent health;

    void Start()
    {
        health = GetComponent<HealthComponent>();
    }

    public void Accept(IDamageVisitor visitor, int damage)
    {
        visitor.VisitArmored(this, damage);
    }

    public void ApplyDamage(int damage, Color numberColor)
    {
        health.TakeDamage(damage);
        //ShowDamageNumber(damage, numberColor);
    }
}