   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Gård : MonoBehaviour
{
    [System.Serializable]
    public class GårdLevel
    {
        public string levelName;

        public float animalCapacity;
        public float animalsGatheredPerTurn;

        public float timeToGather;

        public float woodCostToUpgrade;
        public float metalCostToUpgrade;

        public float daysToCulling = 30;
        public float animalFoodCostPerDay = 1;
        public float cullingReturnPerSpent;
    }
    public GårdLevel currentGårdLevel;
    public GårdLevel[] gårdLevels;
    public int currentLevelIndex = 0;

    public float foodSpentForCulling;
    

    public TimeManager timeManager;
    float secondsPerDay;

    public FloatVariable woodManager;
    public FloatVariable metalManager;
    public FloatVariable foodManager;
    public float currentAnimal = 0;


    public float daysLeftToCulling;
    

    

    
    bool isAqquiring;

    bool isCulling;
    private void Start()
    {        
        
        currentGårdLevel = gårdLevels[currentLevelIndex];

        secondsPerDay = timeManager.secondsPerDay;
        
    }

    private void Update()
    {
        if(currentAnimal > 0 && !isCulling)
        {
            StartCoroutine(Culling());
        }

    }

    
    

    public void AqquireAnimals()
    {
        if (isAqquiring)
        {
            Debug.Log("Already aqquiring");
            return;
        }
        if(currentAnimal == currentGårdLevel.animalCapacity)
        {
            return;
        }


        StartCoroutine(AqquireAnimal());
    }
    #region Levels
    public void UpgradeGård()
    {
        if (CanAffordToUpgrade())
        {
            PurchaseUpgrade();
            LevelUp();
        }        
    }
    void LevelUp()
    {
        
        currentLevelIndex += 1;
        if(currentLevelIndex > gårdLevels.Length-1)
        {
            currentLevelIndex = gårdLevels.Length -1;
        }
        currentGårdLevel = gårdLevels[currentLevelIndex];
    }
    void PurchaseUpgrade()
    {
        float woodCost = currentGårdLevel.woodCostToUpgrade;
        float metalCost = currentGårdLevel.metalCostToUpgrade;

        woodManager.value -= (woodCost);
        metalManager.value -= (metalCost);
    }
    bool CanAffordToUpgrade()
    {
        float woodCost = currentGårdLevel.woodCostToUpgrade;
        float metalCost = currentGårdLevel.metalCostToUpgrade;

        float wood = woodManager.value;
        float metal = metalManager.value;

        if ((wood - woodCost) < 0)
        {
            return false;
        }
        if ((metal - metalCost) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
    


    IEnumerator AqquireAnimal()
    {
        isAqquiring = true;
        Debug.Log("Started to aquire animal");
        yield return new WaitForSeconds(currentGårdLevel.timeToGather);
        AddAnimal();

        isAqquiring = false;
        
        yield return null;
    }

    void AddAnimal()
    {
        currentAnimal += currentGårdLevel.animalsGatheredPerTurn;
        if(currentAnimal > currentGårdLevel.animalCapacity)
        {
            currentAnimal = currentGårdLevel.animalCapacity;
        }
    }

    

    IEnumerator Culling()
    {
        isCulling = true;
        daysLeftToCulling = currentGårdLevel.daysToCulling;
        
        for(int i = 0; i < currentGårdLevel.daysToCulling; i++)
        {
            DayGone();
            daysLeftToCulling -= 1;
            yield return  new WaitForSeconds(secondsPerDay);
        }

        ReturnCulling();
        isCulling = false;
    }
    void ReturnCulling()
    {
        float foodToReturn = foodSpentForCulling * currentGårdLevel.cullingReturnPerSpent;
        foodManager.value += (foodToReturn);

        currentAnimal = 0;
    }
    void DayGone()
    {
        
        
        //Spend food on animals;
        float foodToSpend = currentAnimal * currentGårdLevel.animalFoodCostPerDay;
        
        foodManager.value -= (foodToSpend);
        foodSpentForCulling += foodToSpend;
    }
}
