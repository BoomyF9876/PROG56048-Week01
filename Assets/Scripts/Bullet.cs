using UnityEngine;
/// <summary>
/// Bullet that can do damage
/// </summary>
public class Bullet : Projectile, IDoDamage
{
    [Tooltip("Damage of the bullet")]
    [SerializeField] private int damage = 25;
    private ImpactHandlerSO impactHandler;
    public int Damage => damage;
    public GameObject ImpactEffect => impactHandler.ImpactEffect;

    private void Awake()
    {
        impactHandler = ScriptableObject.CreateInstance<ImpactHandlerSO>();
    }

    public void ApplyImpactEffect(GameObject target, Vector3 hitPoint, Vector3 hitNormal)
    {
        impactHandler.ApplyImpactEffect(target, hitPoint, hitNormal);
    }

    new private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out ITakeDamage target))
        {
            target.TakeDamage(Damage);
            ApplyImpactEffect(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
        }
        base.OnCollisionEnter(collision);
    }
}
