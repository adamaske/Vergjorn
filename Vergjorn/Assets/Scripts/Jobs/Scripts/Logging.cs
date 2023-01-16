using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Logging : MonoBehaviour
{
    
    public LoggingLevel[] loggingLevels;

    public LoggingLevel currentLoggingLevel;
    public int currentLevelIndex;

    bool isLogging;
    public  FloatVariable wood;

    bool canWork = true;

    bool maxLeveled;

    public float currentExp;
    private void Start()
    {
        canWork = true;
        currentLoggingLevel = loggingLevels[currentLevelIndex];

        
    }
    private void Update()
    {
        if (isLogging)
        {
            if (canWork)
            {
                StartCoroutine(DoingLogging());
            }
        }


    }
    public void StartLogging()
    {
        
        isLogging = true;
        
    }

    public void EndLogging()
    {
        StopCoroutine(DoingLogging());
        
        
        isLogging = false;
        canWork = true;
        
    }

    IEnumerator DoingLogging()
    {
        
        canWork = false;
        
        float timeToWait = currentLoggingLevel.timeForDelivery;
        for (int i = 0; i < timeToWait; i++)
        {
            
            yield return new WaitForSeconds(1f);
        }


        ProduceWood();

        canWork = true;

        yield return null;
    }

    void ProduceWood()
    {
        
        float woodToDeliver = currentLoggingLevel.flatProductionAmount;

        float expToGive = currentLoggingLevel.expToGivePerDelivery;

        Debug.Log("Produced wood: " + woodToDeliver.ToString());
        wood.value += woodToDeliver;
        //give exp
        currentExp += expToGive;
        //check if it should level up then level up
        if (currentExp >= currentLoggingLevel.expToNextLevel)
        {
            LevelUp();
        }

    }

    void LevelUp()
    {
        
        currentLevelIndex += 1;
        if(currentLevelIndex >= loggingLevels.Length)
        {
            currentLevelIndex = loggingLevels.Length - 1;
        }
        currentLoggingLevel = loggingLevels[currentLevelIndex];
        currentExp = 0;
        
        
        

        
    }


}
