using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool isPaused = false;
    public GameObject pauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGamePaused())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    resume();
                    GameManager.instance.setGameIsPaused(false);
                }
                else
                {
                    pause();
                    GameManager.instance.setGameIsPaused(true);
                }
            }
        }
    }

    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        GameManager.instance.resumeMusic();
        GameManager.instance.setGameIsPaused(isPaused);
    }

    public void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        GameManager.instance.pauseMusic();
        GameManager.instance.setGameIsPaused(isPaused);
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.loadMenu();
    }

}
