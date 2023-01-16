using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStorage : MonoBehaviour
{
    private StructureManager structureManager;

    public Transform[] standPoints;
    public int index;
    ProgressBar bar;

    public float thisStorageCapacity;

    void Start()
    {
        structureManager = StructureManager.Instance;

        structureManager.GetFoodStorage(this);
    }

    public Vector3 GetStandPoint()
    {
        Vector3 p = standPoints[index].position;

       
        index += 1;
        if(index >= standPoints.Length)
        {
            index = 0;
        }

        return p;
    }
    
    
}
