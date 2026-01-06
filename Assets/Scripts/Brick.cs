using UnityEngine;
/// <summary>
/// Brick that can be destroyed
/// </summary>
public class Brick : MonoBehaviour, ITakeDamage
{
    [Header("General")]
    [Tooltip("The health of the brick.")]
    [SerializeField] private int health = 100;
    [Tooltip("The maximum health of the brick.")]
    [SerializeField] private int maxHealth = 100;

    public int Health => health;
    public float NormalizedHealth => (float)health / maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Take damage and die if health is 0 or less
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destroy the brick
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }
}
