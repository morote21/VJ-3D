using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isPaused = false;
    public GameObject defeatMenuUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate()
    {
        defeatMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        GameManager.instance.playDefeatMusic();
        GameManager.instance.setGameIsPaused(isPaused);
        GameManager.instance.stopMusic();
    }

    public void retry()
    {
        Time.timeScale = 1f;
        GameManager.instance.loadLevel();
        isPaused = false;
        GameManager.instance.setGameIsPaused(isPaused);
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.loadMenu();
        GameManager.instance.setGameIsPaused(isPaused);
    }

}
