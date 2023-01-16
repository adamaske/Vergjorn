using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCredits : MonoBehaviour
{
    public GameObject creditGO;

    private void Start()
    {
        creditGO.SetActive(false);
    }
    public void OpenCloseCredits()
    {
        if (creditGO.activeSelf)
        {
            creditGO.SetActive(false);
        }
        else
        {
            creditGO.SetActive(true);
        }
    }
}
