using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyrmalmManager : MonoBehaviour
{
    public static MyrmalmManager Instance;

    public List<Myrmalm> myrmalms = new List<Myrmalm>();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(this);
        }

        
    }

    public void GetMyrmalm(Myrmalm myrmalm)
    {
        myrmalms.Add(myrmalm);
    }

    public void RemoveMyrmalm(Myrmalm myrmalm)
    {
        if (myrmalms.Contains(myrmalm))
        {
            myrmalms.Remove(myrmalm);
        }
    }

    public Myrmalm GetCloseMyrmalm(Vector3 playerPos)
    {
        if(myrmalms.Count  == 0)
        {
            return null;
        }
        return myrmalms[0];
    }
}
