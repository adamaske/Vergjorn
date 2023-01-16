using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitGame : MonoBehaviour
{
    public string mainMenuScene;
    public bool saveOnExit;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (saveOnExit)
            {
                SaveManager.Instance.OnSave();
            }
            SceneManager.LoadScene(mainMenuScene);
        }
    }

}
