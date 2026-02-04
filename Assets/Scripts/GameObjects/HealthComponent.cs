using UnityEngine;

public class HealthComponent: MonoBehaviour, IHealth
{
    private int health;
    private int maxHealth;

    public int CurrentHealth => health;
    public int MaxHealth => maxHealth;
    public void TakeDamage(int damage)
    {

    }

    public void Heal()
    {

    }

    public void GetHealthPercentage()
    {

    }
}