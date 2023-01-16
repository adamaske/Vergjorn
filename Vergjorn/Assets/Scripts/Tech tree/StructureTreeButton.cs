using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class StructureTreeButton : MonoBehaviour
{
    public StructurePrice structureInfo;

    public GameObject structurePrefab;

    public TechTreeManager treeManager;

    public bool canPurchaseDespiteElse;

    public bool requireOtherUnlockedButtons;
    public StructureTreeButton[] otherRequired;

    [Header("Raid unlocks")]
    public bool requireCompletedRaid;
    public string raidSaveFileName;

   
   
    public void StructurePressed()
    {
        treeManager.StructureSelected(this);
    }

    public bool CanPurchase()
    {
        if (canPurchaseDespiteElse)
        {
            return true;
        }

        return Unlocked();
    }
    
    public bool Unlocked()
    {
        
        if (canPurchaseDespiteElse)
        {
            return true;
        }

        if (requireCompletedRaid)
        {
            if(RequiredRaidCompleted() == false)
            {
                return false;
            }
        }

        if (requireOtherUnlockedButtons)
        {
            if(RequiredButtonsCompleted() == false)
            {
                return false;
            }
        }


        return true;
    }
    

    public bool RequiredRaidCompleted()
    {
        string path = Application.persistentDataPath + "/saves/" + raidSaveFileName + ".save";
        if (!File.Exists(path))
        {
            //File dosent exits wich means it has never been saved
            return false;
        }


        RaidSave s = (RaidSave)SerializationManager.Load(path);
        if(s.raidComleted == true)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    public bool RequiredButtonsCompleted()
    {
        //goes trough each of the required buttons and checks if its unlocked
        for (int i = 0; i < otherRequired.Length; i++)
        {
            if(otherRequired[i].Unlocked() == false)
            {
                return false;
            }
        }
        return true;
    }
}
