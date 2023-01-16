using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingAreaManager : MonoBehaviour
{
    public static HuntingAreaManager Instance;

    public List<HuntingArea> huntingAreas = new List<HuntingArea>();
    private void Awake()
    {
        Instance = this;
    }
    public void GetHuntingArea(HuntingArea a)
    {
        if (!huntingAreas.Contains(a))
        {
            huntingAreas.Add(a);
        }
    }
    public HuntingArea GetCloseHuntingArea(Vector3 pos)
    {
        HuntingArea a = huntingAreas[0];

        float dist = Vector3.Distance(a.transform.position, pos);

        foreach(HuntingArea area in huntingAreas)
        {
            float d = Vector3.Distance(area.transform.position, pos);
            if(d < dist)
            {
                a = area;
                dist = d;
            }
        }

        return a;
    }
}
