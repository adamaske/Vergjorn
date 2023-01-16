using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public List<Worker> workersOnMe = new List<Worker>();

    public Transform[] standPoints;
    int index;

    public Forest myForest;
    public void GetWorker(Worker w)
    {
        
        if (!workersOnMe.Contains(w))
        {
            workersOnMe.Add(w);
        }
    }
    public void RemoveWorker(Worker w)
    {

        if (workersOnMe.Contains(w))
        {
            workersOnMe.Remove(w);
        }
    }

    public Vector3 GetStandPoint()
    {
        Vector3 pos = standPoints[index].position;

        return pos;
    }
}
