using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUpkeepStructure : MonoBehaviour
{
    public List<Worker> workersOnMe = new List<Worker>();

    [System.Serializable]
    public class UpkeepLevel
    {
        public float maxUpkeep;

        public bool useBaseUpkeep;
        public float baseUpkeep = 5f;
        public float perWorkerMultiplier = 1.1f;
        
        [Space]
        public float maxWorkers;
        public bool usePerWorker;
        public float basePerWorkerValue;
        public float perWorkerValue;
        [Space]
        public float goldUpgradeCost;
        public float woodUpgradeCost;
        public float metalUpgradeCost;
        [Space]
        public bool useStaticValue;
        public float staticValue;
        public float maxStaticValue;
    }
    public UpkeepLevel[] upkeepLevels;
    public UpkeepLevel currentUpkeepLevel;
    public int upkeepLevelIndex;

    private Structure structure;


    public bool useLevels = true;
    private void Start()
    {
        FoodUpkeep.Instance.GetStructure(this);
        structure = GetComponent<Structure>();
        if (useLevels)
        {
            SetLevel();
        }
        else
        {
            currentUpkeepLevel = upkeepLevels[0];
        }
        
    }

    void SetLevel()
    {
        upkeepLevelIndex = structure.myData.upkeepLevel;
        currentUpkeepLevel = upkeepLevels[upkeepLevelIndex];
    }

    public void LevelUp()
    {
        upkeepLevelIndex += 1;
        if(upkeepLevelIndex >= upkeepLevels.Length)
        {
            upkeepLevelIndex = upkeepLevelIndex - 1;
        }

        structure.myData.upkeepLevel = upkeepLevelIndex;
        currentUpkeepLevel = upkeepLevels[upkeepLevelIndex];
    }

    public void GetWorker(Worker worker)
    {
        if (!workersOnMe.Contains(worker))
        {
            workersOnMe.Add(worker);
        }
    }
    public void RemoveWorker(Worker worker)
    {
        if (workersOnMe.Contains(worker))
        {
            workersOnMe.Remove(worker);
        }
    }
    private void OnDestroy()
    {
        FoodUpkeep.Instance.RemoveStructure(this);
    }

    public float CurrentFoodGiving()
    {
        if (currentUpkeepLevel.useStaticValue)
        {
            return currentUpkeepLevel.staticValue;
        }
        float t = 0;
        if (currentUpkeepLevel.useBaseUpkeep)
        {
            t = currentUpkeepLevel.baseUpkeep;
            for (int i = 0; i < workersOnMe.Count; i++)
            {
                t *= currentUpkeepLevel.perWorkerMultiplier;
            }

            return t;
        }
        float k = 0;
        if (currentUpkeepLevel.usePerWorker)
        {
            k = currentUpkeepLevel.basePerWorkerValue;
            for (int i = 0; i < workersOnMe.Count; i++)
            {
                k += currentUpkeepLevel.perWorkerValue;
            }

            return k;
        }

        return t;
    }

    public float MaxFoodCanGive()
    {
        if (currentUpkeepLevel.useStaticValue)
        {
            return currentUpkeepLevel.maxStaticValue;
        }
        float t = 0;
        if (currentUpkeepLevel.useBaseUpkeep)
        {
            t = currentUpkeepLevel.baseUpkeep;
            for (int i = 0; i < currentUpkeepLevel.maxWorkers; i++)
            {
                t *= currentUpkeepLevel.perWorkerMultiplier;
            }

            return t;
        }

        float k = 0;
        if (currentUpkeepLevel.usePerWorker)
        {
            for (int i = 0; i < currentUpkeepLevel.maxWorkers; i++)
            {
                k += currentUpkeepLevel.perWorkerValue;
            }

            return k;
        }

        return t;
    }


}
