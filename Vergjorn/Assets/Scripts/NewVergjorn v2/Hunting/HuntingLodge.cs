using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingLodge : MonoBehaviour
{
    public List<Worker> workersAtMe = new List<Worker>();
    public Transform[] farmingStandPoints;
    public int index;
    void Start()
    {
        
    }

    public Vector3 GetHuntingStandPoint()
    {
        Vector3 pos = farmingStandPoints[Random.Range(0, farmingStandPoints.Length - 1)].position;

        
        return pos;
    }

    public HuntingArea GetCloseHutningArea(Vector3 pos)
    {
        return HuntingAreaManager.Instance.GetCloseHuntingArea(transform.position);
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
