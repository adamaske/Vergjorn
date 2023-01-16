using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLimiter : MonoBehaviour
{
    public FloatVariable food;
   
    void Update()
    {
        if(food.value > food.capacity)
        {
            food.value = food.capacity;
        }
    }
}
