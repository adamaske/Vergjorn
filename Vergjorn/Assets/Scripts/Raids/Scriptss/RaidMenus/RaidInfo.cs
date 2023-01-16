using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class RaidInfo : MonoBehaviour
{
    RaidInfoManager manager;

    public bool openDespite;

    public string[] requiredCompletedRaidsSaveFileName;

    public string raidName;
    public string raidDesc;

    public float foodCost;

    public string raidSceneName;

    public string thisRaidSaveFileName;

    private void Start()
    {
        manager = RaidInfoManager.Instance;
    }
    public void RaidInfoPressed()
    {
        manager.GetRaidInfo(this);
    }

    public bool AmIUnlocked()
    {
        if (openDespite)
        {
            return true;
        }
        for (int i = 0; i < requiredCompletedRaidsSaveFileName.Length; i++)
        {
            //Find the save file and check if the raid isCompleted, if the file doesnt exits, it counts as an uncomplted raid
            RaidSave save = (RaidSave)SerializationManager.Load(Application.persistentDataPath + "/saves/" + requiredCompletedRaidsSaveFileName[i] + ".save");
            FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/" + requiredCompletedRaidsSaveFileName[i] + ".save");
            if(info.Exists)
            {
                if (save.raidComleted == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

           
        }

        return true;
    }
}
