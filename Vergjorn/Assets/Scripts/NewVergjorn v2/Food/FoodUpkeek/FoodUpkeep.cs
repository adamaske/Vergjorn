using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUpkeep : MonoBehaviour
{
    public static FoodUpkeep Instance;

    public FloatVariable currentUpkeep;

    public List<FoodUpkeepStructure> structures = new List<FoodUpkeepStructure>();

    public float foodUsedPerWorker = 1;
    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        currentUpkeep.value = CurrentFoodUsed();
        currentUpkeep.capacity = CurrentUpkeep();
    }

    public void GetStructure(FoodUpkeepStructure s)
    {
        if (!structures.Contains(s))
        {
            structures.Add(s);
        }
    }

    public void RemoveStructure(FoodUpkeepStructure s)
    {
        if (structures.Contains(s))
        {
            structures.Add(s);
        }
    }

    public float CurrentFoodUsed()
    {
        float t = 0;

        for (int i = 0; i < PopulationManager.Instance.workersInGame.Count; i++)
        {
            t += foodUsedPerWorker;
        }

        return t;
    }
    public float CurrentUpkeep()
    {
        float t = 0;
        for (int i = 0; i < structures.Count; i++)
        {
            t += structures[i].CurrentFoodGiving();
        }

        return t;
    }

    public float CurrentUpkeepCapacity()
    {
        float t = 0;
        for (int i = 0; i < structures.Count; i++)
        {
            t += structures[i].MaxFoodCanGive();
        }
        return t;
    }

    public float CurrentMaxUpkeep()
    {
        float t = 0;
        for (int i = 0; i < structures.Count; i++)
        {
            t += structures[i].MaxFoodCanGive();
        }

        return t;
    }
}
