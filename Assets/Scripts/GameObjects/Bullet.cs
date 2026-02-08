using UnityEngine;

public enum DamageType
{
    Physical,
    Fire
}

public class Bullet : Projectile, IDamageProvider
{
    [Tooltip("Damage of the bullet")]
    [SerializeField] private int damage = 25;
    [SerializeField] private ImpactHandlerSO impactHandler;
    public int Damage => damage;
    public float crit = 0;
    public DamageType damageType = DamageType.Physical;

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    override protected void HandleCollision(Collision collision)
    {
        if (collision.transform.parent.gameObject.TryGetComponent(out IVisitableEnemy enemy))
        {
            IDamageVisitor visitor = damageType switch
            {
                DamageType.Physical => new PhysicalDamageVisitor(),
                DamageType.Fire => new FireDamageVisitor(),
                _ => new PhysicalDamageVisitor()
            };

            if (Random.Range(0.0f, 1.0f) < crit)
            {
                enemy.Accept(visitor, damage * 1.5f);
            }
            else
            {
                enemy.Accept(visitor, damage);
            }
        }
    }

    public void Fire(Vector3 pos, Quaternion rot, int damage)
    {
        Fire(pos, rot);
        SetDamage(damage);
    }
}
