using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMenu (GameObject menu) {

        menu.SetActive(true);
        gameObject.SetActive(false);
    
    }
}
