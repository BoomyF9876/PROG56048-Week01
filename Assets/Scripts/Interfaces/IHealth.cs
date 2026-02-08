public interface IHealth
{
    void TakeDamage(float amount);

    float CurrentHealth {  get; }

    int MaxHealth { get; }
}
