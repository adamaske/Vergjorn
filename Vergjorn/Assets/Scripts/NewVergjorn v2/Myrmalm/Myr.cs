using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myr : MonoBehaviour
{
    public Myrmalm[] myrmalmsInMyr;
    private void Start()
    {
        StructureManager.Instance.GetMyr(this);

        foreach (Myrmalm myrmalm in myrmalmsInMyr)
        {
            myrmalm.myMyr = this;
        }
    }


    public Myrmalm GetClosestNotBusyMyrmalm(Worker w)
    {
        if (myrmalmsInMyr.Length == 0)
        {
            return null;
        }
        List<Myrmalm> notBusyMyrmalms = new List<Myrmalm>();
        foreach (Myrmalm Myrmalm in myrmalmsInMyr)
        {
            if (Myrmalm.workersOnMe.Count == 0)
            {
                notBusyMyrmalms.Add(Myrmalm);
            }
        }

        if (notBusyMyrmalms.Count != 0)
        {
            //Declare closest not busy Myrmalm
            Myrmalm t = notBusyMyrmalms[0];
            float dist = Vector3.Distance(t.transform.position, w.transform.position);

            foreach (Myrmalm Myrmalm in notBusyMyrmalms)
            {
                float d = Vector3.Distance(Myrmalm.transform.position, w.transform.position);
                if (d < dist)
                {
                    dist = d;
                    t = Myrmalm;
                }
            }

            return t;
        }

        if (myrmalmsInMyr.Length == 0)
        {
            return null;
        }
        return myrmalmsInMyr[0];
    }
}
