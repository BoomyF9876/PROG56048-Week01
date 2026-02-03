using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public abstract class Projectile : MonoBehaviour, IProjectile
{
    [Header("General")]
    [Tooltip("Life span of the projectile")]
    [SerializeField] protected float lifeSpan = 2.5f;
    [Tooltip("Speed of the projectile")]
    [SerializeField] protected float speed = 50f;

    protected Rigidbody rb;
    protected AudioSource audioSource;

    private IObjectPool<Projectile> pool;
    private Coroutine coroutine;

    protected void Awake()
    {
        
    }

    protected void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        coroutine = StartCoroutine(ExecuteAfterTime(lifeSpan));
        InitMovement();
    }

    private IEnumerator ExecuteAfterTime(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        Release();
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
        StopCoroutine(coroutine);
        Release();
    }

    public abstract void Launch();

    public void SetPool(IObjectPool<Projectile> pool)
    {
        this.pool = pool;
    }

    protected void Release()
    {
        pool.Release(this);
    }

    protected abstract void HandleCollision(Collision collision);
}
