using TMPro;
using UnityEngine;

public enum PowerUpType { DamageBoost, Crit, Burn }

public class PowerUpCube : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    private void Start()
    {
        TMP_Text m_TextComponent = GetComponentInChildren<TMP_Text>();
        m_TextComponent.text = powerUpType.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUpManager manager))
        {
            manager.AddPowerUp(powerUpType);
            Destroy(gameObject);
        }
    }
}