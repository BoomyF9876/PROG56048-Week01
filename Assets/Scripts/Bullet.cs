using UnityEngine;
/// <summary>
/// Bullet that can do damage
/// </summary>
public class Bullet : MonoBehaviour, IDoDamage
{
    [Header("General")]
    [Tooltip("Life span of the bullet")]
    [SerializeField] private float lifeSpan = 2.5f;
    [Tooltip("Speed of the bullet")]
    [SerializeField] private float speed = 50f;
    [Tooltip("Damage of the bullet")]
    [SerializeField] private int damage = 25;
    
    [Header("Impact Effect")]
    [Tooltip("Impact effect to be instantiated on collision")]
    [SerializeField] private GameObject impactEffect;
    [Tooltip("Life span of the impact effect")]
    [SerializeField] private float impactEffectLifeSpan = 10f;

    private Rigidbody rb;
    private AudioSource audioSource;

    public GameObject ImpactEffect => impactEffect;
    public int Damage => damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, lifeSpan);
        Init();
    }

    /// <summary>
    /// Initializes the bullet
    /// </summary>
    private void Init()
    {
        rb.linearVelocity = transform.forward * speed;
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out ITakeDamage target))
        {
            target.TakeDamage(Damage);
            ApplyImpactEffect(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
        }
        audioSource.Stop();
        Destroy(gameObject);
    }

    /// <summary>
    /// Applies impact effect to the target
    /// </summary>
    /// <param name="target">The target to apply the impact effect to</param>
    /// <param name="hitPoint">The point of impact</param>
    /// <param name="hitNormal">The normal of the impact</param>
    public void ApplyImpactEffect(GameObject target, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (impactEffect == null || target == null) return;
        GameObject impact = Instantiate(impactEffect, hitPoint + (hitNormal * 0.001f), Quaternion.LookRotation(hitNormal));
        impact.transform.parent = target.transform;
        Destroy(impact, impactEffectLifeSpan);
    }
}
