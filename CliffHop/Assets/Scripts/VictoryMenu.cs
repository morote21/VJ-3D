using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isPaused = false;
    public GameObject victoryMenuUI;

    // Update is called once per frame
    void Update()
    {

    }

    public void activate()
    {
        victoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        GameManager.instance.playVictoryMusic();
        GameManager.instance.setGameIsPaused(isPaused);
        GameManager.instance.stopMusic();
    }


    public void loadMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.loadMenu();
    }

}
