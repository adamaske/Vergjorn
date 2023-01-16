using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHut : MonoBehaviour
{
    public List<Worker> workersOnMe = new List<Worker>();
    public List<Worker> workersFishing = new List<Worker>();

    public FoodUpkeepStructure foodUpkeepStructure; 
    [System.Serializable]
    public class FishingHutLevel
    {
        public float maxWorkersAtMe;

        public float goldUpgradeCost;
    }
    public FishingHutLevel currentFishingHutLevel;
    public int fishingLevelIndex;
    public FishingHutLevel[] fishingHutLevels;
    private void Start()
    {
        currentFishingHutLevel = fishingHutLevels[0];
        foodUpkeepStructure = GetComponent<FoodUpkeepStructure>();
    }

    public void GetWorker(Worker w)
    {
        if (!workersOnMe.Contains(w))
        {
            workersOnMe.Add(w);

            foodUpkeepStructure.GetWorker(w);
        }
    }
    public void RemoveWorker(Worker w)
    {
        if (workersOnMe.Contains(w))
        {
            workersOnMe.Remove(w);

            foodUpkeepStructure.RemoveWorker(w);
        }
        if (workersFishing.Contains(w))
        {
            workersFishing.Remove(w);
        }
    }

    public void WorkerStartedFishing(Worker w)
    {
        if (!workersFishing.Contains(w))
        {
            workersFishing.Add(w);
        }
        
    }

    public bool CanGetMoreWorkers()
    {
        if(workersOnMe.Count + 1 <= currentFishingHutLevel.maxWorkersAtMe)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public FishingPoint GetFishingPoint(Worker w)
    {
        FishingPoint p = WaterManager.Instance.GetClosestNotBusyFishingPoint(w);
        
        return p;
    }
}
