using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioSource gameMusic;
    public AudioSource defeatMusic;
    public AudioSource victoryMusic;

    private float startTimer;
    private bool gameStarted, newHighScore, gameIsPaused;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameMusic.Play();
            newHighScore = false;
            gameIsPaused = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (!gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.R))    // reseteamos monedas y highscore
            {
                resetAll();
            }
            else if (Input.GetKeyDown(KeyCode.Q))   // recargamos toda la escena 
            {
                loadLevel();
            }
        }
        
    }

    public void coinPickup()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 1);
    }

    public void setHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            newHighScore = true;
            Debug.Log("High score! : " + getHighScore());
        }
    }

    public int getCoins()
    {
        return PlayerPrefs.GetInt("Coins", 0);
    }

    public int getHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public void resetAll()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("HighScore", 0);
        newHighScore = false;
    }

    public void spendCoins(int quantity)
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - quantity);
    }

    public void loadLevel()
    {
        SceneManager.LoadScene("SampleScene");
        gameMusic.Stop();
        gameMusic.Play();
        gameStarted = false;
        startTimer = 0f;
        newHighScore = false;
    }

    public void pauseMusic()
    {
        gameMusic.Pause();
    }

    public void resumeMusic()
    {
        gameMusic.Play();
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void playDefeatMusic()
    {
        defeatMusic.Play();
    }

    public void playVictoryMusic()
    {
        victoryMusic.Play();
    }

    public bool hasNewHighScore()
    {
        return newHighScore;
    }

    public bool isGamePaused()
    {
        return gameIsPaused;
    }

    public void setGameIsPaused(bool paused)
    {
        gameIsPaused = paused;
    }

    public void stopMusic()
    {
        gameMusic.Stop();
    }
}
