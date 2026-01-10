using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Life span of the projectile")]
    [SerializeField] protected float lifeSpan = 2.5f;
    [Tooltip("Speed of the projectile")]
    [SerializeField] protected float speed = 50f;

    protected Rigidbody rb;
    protected AudioSource audioSource;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, lifeSpan);
        Init();
    }

    /// <summary>
    /// Initializes the bullet
    /// </summary>
    protected void Init()
    {
        rb.linearVelocity = transform.forward * speed;
        audioSource.Play();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        audioSource.Stop();
        Destroy(gameObject);
    }

}
