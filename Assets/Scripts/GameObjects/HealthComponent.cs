using UnityEngine;

public class HealthComponent: MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth;
    private float health;
    private HealthBar healthBar;

    public float CurrentHealth => health;
    public int MaxHealth => maxHealth;

    public void Start()
    {
        health = maxHealth;

        if (healthBar == null) healthBar = GetComponentInChildren<HealthBar>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log(health);

        healthBar.UpdateHealth(GetHealthPercentage(), damage);

        if (health <= 0) Die();
    }

    public void Heal()
    {

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }

    public void ShowDamageNumber(float damage, Vector3 numberColor)
    {

    }
}