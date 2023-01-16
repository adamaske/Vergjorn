using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    
    public FishingLevel[] fishingLevels;

    public FishingLevel currentFishingLevel;
    public int currentLevelIndex;

    bool isFishing;

    bool canWork = true;

    bool maxLeveled;
    bool stopped;
    public FloatVariable food;

    public float currentExp;
    private void Start()
    {
        currentFishingLevel = fishingLevels[currentLevelIndex];
        currentExp = 0;
        
    }
    private void Update()
    {
        if (isFishing)
        {
            if (canWork)
            {
                StartCoroutine(DoingFishing());
            }
        }
    }

    public void StartFishing()
    {
        isFishing = true;
    }

    public void EndFishing()
    {
        StopCoroutine(DoingFishing());

        isFishing = false;
        canWork = true;
    }

    IEnumerator DoingFishing()
    {
        canWork = false;

        float timeToWait = currentFishingLevel.timeForDelivery;
        for (int i = 0; i < timeToWait; i++)
        {
            

            yield return new WaitForSeconds(1);
        }

        ProduceFood();

        canWork = true;

        yield return null;
    }

    void ProduceFood()
    {
        
        float fishToDeliver = currentFishingLevel.flatProductionAmount;
        float expToGive = currentFishingLevel.expToGivePerDelivery;
        Debug.Log("Produced food: " + fishToDeliver.ToString());
        food.value += (fishToDeliver);
        currentExp += expToGive;

        if (currentExp >= currentFishingLevel.expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevelIndex += 1;
        if (currentLevelIndex >= fishingLevels.Length)
        {
            currentLevelIndex = fishingLevels.Length - 1;
        }
        currentFishingLevel = fishingLevels[currentLevelIndex];
        currentExp = 0;
    }
}

