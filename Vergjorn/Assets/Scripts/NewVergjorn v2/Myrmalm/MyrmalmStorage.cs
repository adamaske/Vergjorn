using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyrmalmStorage : MonoBehaviour
{
    private StructureManager structureManager;

    public Transform[] standPoints;
    int index;
    public float myrmalmCapacityGiven;


    void Start()
    {
        structureManager = StructureManager.Instance;

        structureManager.GetMyrmalmStorage(this);
    }

    public void GetMyrmalm(float amount)
    {

    }
    public Vector3 GetStandPoint(Vector3 startPos)
    {
        Vector3 pos = standPoints[0].position;
        float dist = Vector3.Distance(pos, startPos);

        for (int i = 0; i < standPoints.Length; i++)
        {
            float d = Vector3.Distance(standPoints[i].position, startPos);
            if(d < dist)
            {
                dist = d;
                pos = standPoints[i].position;
            }
        }

        return pos;
    }

    private void OnDestroy()
    {
        if (structureManager.myrmalmStorages.Contains(this))
        {
            structureManager.myrmalmStorages.Remove(this);
        }

    }

    public float MyrmalmCapacityGiven()
    {
        return myrmalmCapacityGiven;
    }

}
