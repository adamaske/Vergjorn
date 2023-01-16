using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class LoadScenes : MonoBehaviour
{
    public WhatToDoOnStart wtdos;

    public string sceneName;

    public string newBuildSaveName;
    public void LoadCurrentSave()
    {
        
        wtdos.loadCurrentSave = true;
        wtdos.loadNewGameSave = false;




        SceneLoader.Instance.LoadScene(sceneName);
    }

    public void LoadNewGame()
    {
        wtdos.loadCurrentSave = false;
        wtdos.loadNewGameSave = true;

        SerializationManager.DeleteAllSaves();

        SceneLoader.Instance.LoadScene(sceneName);
       
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class NewBuildSaveFile
{

}
