using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioSource gameMusic;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameMusic.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetAll();
        }
        else if (Input.GetKeyDown(KeyCode.Q))   // recargamos toda la escena 
        {
            loadLevel();
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
    }
}
