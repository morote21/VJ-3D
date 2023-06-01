using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHighScoreScript : MonoBehaviour
{

    public Text newHighScore;
    public GameObject newHSPanel;

    // Start is called before the first frame update
    void Start()
    {
        newHSPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        newHighScore.text = GameManager.instance.getHighScore().ToString();
        if (GameManager.instance.hasNewHighScore())
        {
            newHSPanel.SetActive(true);
        }
    }
}
