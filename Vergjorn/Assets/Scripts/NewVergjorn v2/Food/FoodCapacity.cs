using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCapacity : MonoBehaviour
{
    public FloatVariable foodCount;
    public BoolVariable useCount;

    public float current;
    // Update is called once per frame
    void Update()
    {
        SetCapacity();
    }

    void SetCapacity()
    {
        float t = 0;

        for (int i = 0; i < StructureManager.Instance.foodStorages.Count; i++)
        {
            t += StructureManager.Instance.foodStorages[i].thisStorageCapacity;
        }

        foodCount.capacity = t;
    }
}
