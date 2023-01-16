using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LoggingLevel : ScriptableObject
{
    public string levelName;



    
    public float expToNextLevel;

    public float timeForDelivery;
    public float flatProductionAmount;
    public float expToGivePerDelivery;
}
