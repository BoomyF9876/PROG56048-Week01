using UnityEngine;
using UnityEngine.Pool;
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile prefab;

    private IObjectPool<Projectile> pool;

    public IObjectPool<Projectile> Pool
    {
        get
        {
            if (pool == null)
            {
                pool = new ObjectPool<Projectile>(
                    createFunc: CreateProjectile,
                    actionOnGet: OnGetFromPool,
                    actionOnRelease: OnReleaseToPool,
                    actionOnDestroy: OnDestroyPooledItem,
                    collectionCheck: true,
                    defaultCapacity: 10,
                    maxSize: 100
                );

            }    
            return pool;
        }
    }

    private Projectile CreateProjectile()
    {
        var p = Instantiate(prefab);
        p.SetPool(Pool); // Dependency Injection
        return p;
    }

    private void OnGetFromPool(Projectile bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Projectile bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPooledItem(Projectile bullet)
    {
        Destroy(bullet);
    }

    public Projectile GetBulletFromPool(Vector3 position, Quaternion rotation)
    {
        Projectile bullet = Pool.Get();

        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
        }

        return bullet;
    }
}
