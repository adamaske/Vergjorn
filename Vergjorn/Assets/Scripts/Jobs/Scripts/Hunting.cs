using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunting : MonoBehaviour
{
    
    
    public HuntingLevel[] huntingLevels;

    public HuntingLevel currentHuntingLevel;
    public int currentLevelIndex;

    bool isHunting;

    bool canWork = true;
    public FloatVariable food;
    bool maxLeveled;
    bool stopped;
    public float currentExp;
    private void Start()
    {
        currentHuntingLevel = huntingLevels[currentLevelIndex];
        
    }
    private void Update()
    {
        if (isHunting)
        {
            if (canWork)
            {
                StartCoroutine(DoingHunting());
            }
        }
    }

    public void StartHUnting()
    {
        isHunting = true;
    }

    public void EndHunting()
    {
        StopCoroutine(DoingHunting());
        stopped = true;
        isHunting = false;
        canWork = true;
    }

    IEnumerator DoingHunting()
    {
        canWork = false;

        float timeToWait = currentHuntingLevel.timeForDelivery;
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
        
        float foodToDeliver = currentHuntingLevel.flatProductionAmount;
        float expToGive = currentHuntingLevel.expToGivePerDelivery;
        Debug.Log("Produced food: " + foodToDeliver.ToString());

        food.value += (foodToDeliver);
        currentExp += expToGive;
        if (currentExp >= currentHuntingLevel.expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevelIndex += 1;
        if (currentLevelIndex >= huntingLevels.Length)
        {
            currentLevelIndex = huntingLevels.Length - 1;
        }
        currentHuntingLevel = huntingLevels[currentLevelIndex];
        currentExp = 0;
    }
}


