using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Stabburd : MonoBehaviour
{
    [System.Serializable]
    public class StabburdLevel
    {
        public string levelName;
        public float capacityBonus;

        public float woodUpgradeCost;
        public float metalUpgradeCost;
    }
    public StabburdLevel[] stabburdLevels;
    public StabburdLevel currentStabburdLevel;
    int levelIndex;
    public FloatVariable food;
    public FloatVariable metal;
    public FloatVariable wood;
    

    private void Start()
    {
        currentStabburdLevel = stabburdLevels[levelIndex];
        IncreaseFoodCapacity();
    }

    public void IncreaseFoodCapacity()
    {
        food.capacity += (currentStabburdLevel.capacityBonus);
    }



#region Upgrades
    public void UpgradeStabburd()
    {
        if (CanAffordToUpgrade())
        {
            PurchaseUpgrade();
            levelIndex += 1;
            
            if(levelIndex > stabburdLevels.Length)
            {
                levelIndex = stabburdLevels.Length;
            }
            currentStabburdLevel = stabburdLevels[levelIndex];
            IncreaseFoodCapacity();

        }


    }

    void PurchaseUpgrade()
    {
        wood.value -=(currentStabburdLevel.woodUpgradeCost);
        metal.value -=(currentStabburdLevel.metalUpgradeCost);
    }

    bool CanAffordToUpgrade()
    {
        float curWood = wood.value;
        float curMetal = metal.value;

        float woodCost = currentStabburdLevel.woodUpgradeCost;
        float metalCost = currentStabburdLevel.metalUpgradeCost;

        if((curWood - woodCost) < 0)
        {
            return false;
        }
        if((curMetal - metalCost) < 0)
        {
            return false;
        }
        return true;
    }
    #endregion
}
