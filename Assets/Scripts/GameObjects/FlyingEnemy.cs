using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IVisitableEnemy
{
    private HealthComponent health;

    void Start()
    {
        health = GetComponent<HealthComponent>();
    }

    public void Accept(IDamageVisitor visitor, float damage)
    {
        visitor.VisitFlying(this, damage);
    }

    public void ApplyDamage(float damage, Color numberColor)
    {
        health.TakeDamage(damage);
    }
}
