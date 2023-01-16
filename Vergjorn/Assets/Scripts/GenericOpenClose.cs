using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericOpenClose : MonoBehaviour
{
    public GameObject[] uiElements;


    private GameObject currentActiveUIElement;
    public int currentIndex = 0;

    private void Start()
    {
        DisableAll();
        ActivateCurrent();
    }
    public void OpenNextUIMenu()
    {
        currentIndex += 1;
        if(currentIndex >= uiElements.Length)
        {
            currentIndex = 0;
        }
        DisableAll();
        ActivateCurrent();

    }

    void DisableAll()
    {
        for(int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(false);
        }
    }

    void ActivateCurrent()
    {
        uiElements[currentIndex].SetActive(true);
    }
}
