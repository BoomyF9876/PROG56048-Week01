using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private float count = 0;
    private int time = 1;

    public void LoadOptionsScene()
    {
        GameManager.Instance.SwitchState(new OptionsMenu(), true);
    }

    public void LoadStartLevelScene()
    {
        GameManager.Instance.SwitchState(new NewGameState());
    }

    public void LoadNextLevelScene()
    {
        GameManager.Instance.SwitchState(new NextLevelState());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (count < time)
        {
            count += Time.deltaTime;
        }
        else
        {
            ArmoredEnemy enemy1;
            FlyingEnemy enemy2;
            enemy1 = Object.FindFirstObjectByType<ArmoredEnemy>();
            if (enemy1 != null)
            {
                enemy1.ApplyDamage(30, new Color(1f, 0.5f, 0f), true);
            }
            else
            {
                enemy2 = Object.FindFirstObjectByType<FlyingEnemy>();
                if (enemy2 != null)
                {
                    enemy2.ApplyDamage(10, new Color(1f, 0.5f, 0f), true);
                }
            }
            count = 0;
        }
    }
}
