using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class NewBuild : MonoBehaviour
{
    public string saveFileName;

    private void Awake()
    {
        FileInfo file = new FileInfo(Application.persistentDataPath + "/saves/" + saveFileName + ".save");
        if (!file.Exists)
        {
            string path = Application.persistentDataPath + "/saves/";
            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Delete();

            Debug.Log("Found no " + saveFileName + ", deleted /saves/ and created new");
            Directory.CreateDirectory(path);


            NewGameSave n = new NewGameSave();
            SerializationManager.Save(saveFileName, n);
        }
        else
        {
            Debug.Log("Found save file, so the game has already been reset for this build");
            Destroy(this);
        }

        


        
    }
}

[System.Serializable]
public class NewGameSave
{

}
