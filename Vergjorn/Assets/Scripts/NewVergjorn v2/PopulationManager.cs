using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance;

    public List<Worker> workersInGame = new List<Worker>();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void GetWorker(Worker worker)
    {
        workersInGame.Add(worker);
    }

    public void RemoveWorker(Worker worker)
    {
        if (workersInGame.Contains(worker))
        {
            workersInGame.Remove(worker);
        }
    }
}
