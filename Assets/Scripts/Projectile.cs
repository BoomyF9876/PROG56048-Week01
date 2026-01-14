using UnityEngine;

public abstract class Projectile : MonoBehaviour, IProjectile
{
    [Header("General")]
    [Tooltip("Life span of the projectile")]
    [SerializeField] protected float lifeSpan = 2.5f;
    [Tooltip("Speed of the projectile")]
    [SerializeField] protected float speed = 50f;

    protected Rigidbody rb;
    protected AudioSource audioSource;

    protected void Awake()
    {
        
    }

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, lifeSpan);
        InitMovement();
    }

    /// <summary>
    /// Initializes the bullet
    /// </summary>
    protected void InitMovement()
    {
        rb.linearVelocity = transform.forward * speed;
        audioSource.Play();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
        audioSource.Stop();
        Destroy(gameObject);
    }

    public abstract void Launch();

    protected abstract void HandleCollision(Collision collision);
}
