using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingArea : MonoBehaviour
{
    public List<Worker> workersAtMe = new List<Worker>();

    public Transform[] huntingStandPoints;

    private void Start()
    {
        HuntingAreaManager.Instance.GetHuntingArea(this);
    }
    public Vector3 GetHuntingStandPoint()
    {
        Vector3 pos = huntingStandPoints[Random.Range(0, huntingStandPoints.Length - 1)].position;


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
