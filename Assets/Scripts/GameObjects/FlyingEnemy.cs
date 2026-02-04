using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IVisitableEnemy
{
    private HealthComponent health;

    void Start()
    {
        health = GetComponent<HealthComponent>();
    }

    public void Accept(IDamageVisitor visitor, int damage)
    {
        visitor.VisitFlying(this, damage);
    }

    public void ApplyDamage(int damage, Color numberColor)
    {
        health.TakeDamage(damage);
        //ShowDamageNumber(damage, numberColor);
    }
}
