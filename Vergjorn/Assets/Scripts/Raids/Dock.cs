using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{
    public List<Worker> vikingsReady = new List<Worker>();


    public Transform standPointParent;

    Transform[] standPoints;
    int standPointIndex;

    private void Start()
    {
        standPoints = standPointParent.GetComponentsInChildren<Transform>();
    }
    public void GetWorker(Worker worker)
    {
        if (!vikingsReady.Contains(worker))
        {
            vikingsReady.Add(worker);
        }
    }

    public Vector3 StandPoint()
    {
        Vector3 pos = standPoints[standPointIndex].position;

        standPointIndex += 1;
        if(standPointIndex >= standPoints.Length)
        {
            standPointIndex = 0;
        }

        return pos;
    }

    public void RemoveWorker(Worker worker)
    {
        if (vikingsReady.Contains(worker))
        {
            vikingsReady.Remove(worker);
        }
    }
}
