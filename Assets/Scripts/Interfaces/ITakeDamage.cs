/// <summary>
/// Interface for objects that can take damage.
/// </summary>
public interface ITakeDamage {
    int Health { get; }
    float NormalizedHealth { get; }
    void TakeDamage(int damage);
}