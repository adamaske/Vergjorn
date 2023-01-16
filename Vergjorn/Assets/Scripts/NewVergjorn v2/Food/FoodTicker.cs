using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTicker : MonoBehaviour
{
    public static FoodTicker Instance;

    public FloatVariable food;

    public float foodNeededPerDay = 1f;

    public string lowFoodNotificationText = "You are low on food, gather more so your people dont starve.";
    public float warningNotificationLength = 5f;
    public Sprite notificationSprite;
    public float warnWhenBelow = 10f;

    public bool takeFoodPerDay;
    private void Awake()
    {
        Instance = this;
    }


    public void NewDay()
    {
        if(food.value <= warnWhenBelow) 
        {
            NotificationManager.Instance.OpenNotification("Low Food", lowFoodNotificationText, warningNotificationLength, notificationSprite);
        }

        if (takeFoodPerDay)
        {
            float t = food.value;
            float k = 0;
            if(FoodTooTake() > t)
            {
                k = t;

            }
            else
            {
                k = FoodTooTake();
            }

            food.value -= k;
        }
    }

    public float FoodTooTake()
    {
        float t = PopulationManager.Instance.workersInGame.Count;



        return t;
    }
}
