
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public static WaterManager Instance;

    public FishingPoint[] fishingPoints;

    public int index;
    
    private void Awake()
    {
        Instance = this;
    }


    public FishingPoint GetClosestNotBusyFishingPoint(Worker w)
    {
        if(fishingPoints.Length == 0)
        {
            return null;
        }

        List<FishingPoint> notBusyPoints = new List<FishingPoint>();
        foreach(FishingPoint point in fishingPoints)
        {
            if(point.workersAtMe.Count == 0)
            {
                notBusyPoints.Add(point);
            }


        }

        if(notBusyPoints.Count != 0)
        {
            FishingPoint p = notBusyPoints[0];
            float dist = Vector3.Distance(w.transform.position, p.transform.position);

            foreach(FishingPoint point in notBusyPoints)
            {
                float d = Vector3.Distance(point.transform.position, w.transform.position);
                if(d < dist)
                {
                    dist = d;
                    p = point;
                }
            }

            return p;
        }

        FishingPoint pi = fishingPoints[index];

        index += 1;
        if(index >= fishingPoints.Length)
        {
            index = 0;
        }

        return pi;
        
    }
}
