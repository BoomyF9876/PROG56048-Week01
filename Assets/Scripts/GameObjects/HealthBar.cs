using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    public TMP_Text damageNumber;

    private float speed = 5f;
    private float targetFillAmount = 1f;
    private Coroutine coroutine;

    private void Start()
    {
        if (damageNumber == null) damageNumber = GetComponentInChildren<TMP_Text>();
    }
    private IEnumerator RunFunctionForDuration(float duration)
    {
        float elapsed = 0f;
        damageNumber.fontSize = 0.2f;

        while (elapsed < duration)
        {
            damageNumber.fontSize += 0.0005f;

            elapsed += Time.deltaTime;
            yield return null;
        }

        damageNumber.gameObject.SetActive(false);
    }

    public void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if (healthFillImage.fillAmount != targetFillAmount)
        {
            healthFillImage.fillAmount = Mathf.Lerp(healthFillImage.fillAmount, targetFillAmount, speed * Time.deltaTime);
        }
    }

    public void UpdateHealth(float percentage, float damage)
    {
        targetFillAmount = percentage;

        damageNumber.gameObject.SetActive(true);
        damageNumber.text = ((int)damage).ToString();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(RunFunctionForDuration(2.0f));
    }
}
