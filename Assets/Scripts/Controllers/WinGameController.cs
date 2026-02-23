using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class WinGameController : MonoBehaviour
{
    [SerializeField] private GameObject winTxt;

    void Start()
    {
        StartCoroutine(WaitAndPrint());
    }

    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(2.0f);

        winTxt.SetActive(true);

        StartCoroutine(WaitAndMainMenu());
    }

    IEnumerator WaitAndMainMenu()
    {
        yield return new WaitForSeconds(1.0f);

        //GetComponentInChildren<TMP_Text>().enabled = false;

        GameManager.Instance.SwitchState(new MainMenuState());
    }
}
