using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoint : MonoBehaviour
{
    public List<Worker> workersAtMe = new List<Worker>();
    public Transform workerLookPoint;
    public void GetWorker(Worker w)
    {
        if (!workersAtMe.Contains(w))
        {
            workersAtMe.Add(w);
        }
    }
    public void RemoveWorker(Worker w)
    {
        if (workersAtMe.Contains(w))
        {
            workersAtMe.Remove(w);
        }
    }
}
