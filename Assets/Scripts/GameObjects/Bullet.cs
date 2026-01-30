using UnityEngine;
/// <summary>
/// Bullet that can do damage
/// </summary>
public class Bullet : Projectile, IDamageProvider, IImpactProvider
{
    [Tooltip("Damage of the bullet")]
    [SerializeField] private int damage = 25;
    [SerializeField] private ImpactHandlerSO impactHandler;
    public int Damage => damage;

    public void ApplyImpactEffect(GameObject target, Vector3 hitPoint, Vector3 hitNormal)
    {
        impactHandler.ApplyImpactEffect(target, hitPoint, hitNormal);
    }

    override protected void HandleCollision(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ITakeDamage target))
        {
            target.TakeDamage(Damage);
            ApplyImpactEffect(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
        }
    }

    override public void Launch()
    {

    }
}
