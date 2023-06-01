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
    private bool gameStarted, newHighScore;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameMusic.Play();
            newHighScore = false;
            //startTimer = 0f;
            //Time.timeScale = 0f;
            //gameStarted = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        /*
        startTimer += Time.unscaledDeltaTime;
        if (startTimer > 5) Debug.Log("GO!");
        else if (startTimer > 4) Debug.Log("4!");
        else if (startTimer > 3) Debug.Log("1!");
        else if (startTimer > 2) Debug.Log("2!");
        else if (startTimer > 1) Debug.Log("3!");

        if (startTimer >= 5f && !gameStarted)   // a los 3 segundos empieza el juego
        {
            Time.timeScale = 1f;
            gameStarted = true;
        }
        */
        //if (gameStarted)
        //{
        if (Input.GetKeyDown(KeyCode.R))    // reseteamos monedas y highscore
        {
            resetAll();
        }
        else if (Input.GetKeyDown(KeyCode.Q))   // recargamos toda la escena 
        {
            loadLevel();
        }
        //}
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

    }

    public bool hasNewHighScore()
    {
        return newHighScore;
    }

}
