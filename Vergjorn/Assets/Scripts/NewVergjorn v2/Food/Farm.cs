using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public List<Worker> workersAtMe = new List<Worker>();

    public Transform[] farmingStandPoints;
    public int index;
    void Start()
    {
        
    }

    public Vector3 GetFarmingStandPoint()
    {
        Vector3 pos = farmingStandPoints[index].position;

        index += 1;
        if(index >= farmingStandPoints.Length)
        {
            index = 0;
        }

        return pos;
    }

    public void GetWorker(Worker w)
    {
        if (!workersAtMe.Contains(w))
        {
            workersAtMe.Add(w);
        }

    }
    public void RemvoeWorker(Worker w)
    {
        if (workersAtMe.Contains(w))
        {
            workersAtMe.Remove(w);
        }
    }
}
