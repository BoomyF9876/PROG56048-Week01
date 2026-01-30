public interface IHealth
{
    void TakeDamage(int amount);

    int CurrentHealth {  get; }

    int MaxHealth { get; }
}
