using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LoggingHut : MonoBehaviour
{
    [System.Serializable]
    public class LoggingHutLevel
    {
        public string levelName;
        public float capacityBonus;

        public float woodUpgradeCost;
        public float metalUpgradeCost;
    }
    public LoggingHutLevel[] LoggingHutLevels;
    public LoggingHutLevel currentLoggingHutLevel;
    int levelIndex;
    public FloatVariable food;
    public FloatVariable metal;
    public FloatVariable wood;


    private void Start()
    {
        currentLoggingHutLevel = LoggingHutLevels[levelIndex];
      
    }

    public void IncreaseWoodCapacity()
    {
        
    }



    #region Upgrades
    public void UpgradeLoggingHut()
    {
        if (CanAffordToUpgrade())
        {
            PurchaseUpgrade();
            levelIndex += 1;

            if (levelIndex > LoggingHutLevels.Length)
            {
                levelIndex = LoggingHutLevels.Length;
            }
            currentLoggingHutLevel = LoggingHutLevels[levelIndex];
            IncreaseWoodCapacity();

        }


    }

    void PurchaseUpgrade()
    {
        wood.value -=(currentLoggingHutLevel.woodUpgradeCost);
        metal.value -=(currentLoggingHutLevel.metalUpgradeCost);
    }

    bool CanAffordToUpgrade()
    {
        float curWood = wood.value;
        float curMetal = metal.value;

        float woodCost = currentLoggingHutLevel.woodUpgradeCost;
        float metalCost = currentLoggingHutLevel.metalUpgradeCost;

        if ((curWood - woodCost) < 0)
        {
            return false;
        }
        if ((curMetal - metalCost) < 0)
        {
            return false;
        }
        return true;
    }
    #endregion
}



