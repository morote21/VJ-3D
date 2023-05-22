using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        currentPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPage() {

        Button prevButton = transform.GetChild(1).GetComponent<Button>(); // QUIZÁS PONER COMO VAR GLOBALES?
        Button nextButton = transform.GetChild(2).GetComponent<Button>();

        Transform pages = transform.GetChild(3);


        pages.GetChild(currentPage).gameObject.SetActive(false);
        ++currentPage;
        pages.GetChild(currentPage).gameObject.SetActive(true);

        if (pages.childCount == currentPage+1)
            nextButton.interactable = false;

        prevButton.interactable = true;
    }

    public void PreviousPage() {

        Button prevButton = transform.GetChild(1).GetComponent<Button>(); // QUIZÁS PONER COMO VAR GLOBALES?
        Button nextButton = transform.GetChild(2).GetComponent<Button>();

        Transform pages = transform.GetChild(3);


        pages.GetChild(currentPage).gameObject.SetActive(false);
        --currentPage;
        pages.GetChild(currentPage).gameObject.SetActive(true);

        if (currentPage == 0)
            prevButton.interactable = false;

        nextButton.interactable = true;
    }

    public void GoToMenu(GameObject menu)
    {
        Button prevButton = transform.GetChild(1).GetComponent<Button>();
        Button nextButton = transform.GetChild(2).GetComponent<Button>();

        Transform pages = transform.GetChild(3);

        if (pages.childCount > 1 && currentPage != 0) {

            pages.GetChild(currentPage).gameObject.SetActive(false);
            pages.GetChild(0).gameObject.SetActive(true);

            prevButton.interactable = false;
            nextButton.interactable = true;
            currentPage = 0;    
        }

        menu.SetActive(true);
        gameObject.SetActive(false);

    }
}
