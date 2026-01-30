using UnityEngine;

public enum PowerUpType { DamageBoost, Crit, Burn }

public class PowerUpCube : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUpManager manager))
        {
            manager.AddPowerUp(powerUpType);
            Destroy(gameObject);
        }
    }
}