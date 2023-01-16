using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrmalm : MonoBehaviour
{
    public List<Worker> workersOnMe = new List<Worker>();

    public Transform[] standPoints;
    int index;

    public Myr myMyr;
    public void GetWorker(Worker w)
    {
        Debug.Log("Got worker");
        if (!workersOnMe.Contains(w))
        {
            workersOnMe.Add(w);
        }
    }
    public void RemoveWorker(Worker w)
    {
        Debug.Log("Removed worker");
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
