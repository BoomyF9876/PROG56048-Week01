using System;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : MonoBehaviour
{
    [SerializeField] private GameObject activeDecorators;
    [SerializeField] private GameObject stackCounts;
    [SerializeField] private GameObject weaponStats;
    [SerializeField] private GameObject powerUpHistory;

    [SerializeField] private Image activeDecoratorImage;

    void Start()
    {

    }

    public void PowerUp(PowerUpType type)
    {
        GameObject icon = Instantiate(activeDecoratorImage.gameObject);
        Sprite newSprite = Resources.Load<Sprite>(type.ToString());
        if (newSprite != null)
        {
            icon.GetComponent<Image>().sprite = newSprite;
        }
        icon.transform.SetParent(activeDecorators.transform);

        foreach (Transform child in stackCounts.transform)
        {
            if (Enum.TryParse(child.name, out PowerUpType childType) && PowerUpManager.Instance.powerUps.ContainsKey(childType))
            {
                child.GetComponent<TMP_Text>().text = child.name + ": " + PowerUpManager.Instance.powerUps[childType];
            }
        }

        foreach (Transform child in weaponStats.transform)
        {
            if (child.name == "Dmg")
            {
                child.GetComponent<TMP_Text>().text = "Total damage: " + PowerUpManager.Instance.GetCurrentWeapon().GetDamage();
            }
            if (child.name == "Crit")
            {
                child.GetComponent<TMP_Text>().text = "Crit chance: " + PowerUpManager.Instance.GetCurrentWeapon().GetCrit() * 100 + "%";
            }
            if (child.name == "Type")
            {
                child.GetComponent<TMP_Text>().text = "Damage Type: " + PowerUpManager.Instance.GetCurrentWeapon().GetDamageType().ToString();
            }
        }
    }
}
