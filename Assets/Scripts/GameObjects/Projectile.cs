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
    private TrailRenderer trailRenderer;

    protected void Awake()
    {
        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }
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
        gameObject.SetActive(true);
        rb.linearVelocity = transform.forward * speed;
        audioSource.Play();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        audioSource.Stop();
        Release();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        HandleCollision(collision);
    }

    public virtual void Launch()
    {

    }

    public void Fire(Vector3 pos, Quaternion rot)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        transform.position = pos;
        transform.rotation = rot;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        trailRenderer?.Clear();

        InitMovement();

        coroutine = StartCoroutine(ExecuteAfterTime(lifeSpan));
    }

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
