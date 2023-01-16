using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringManager : MonoBehaviour
{
    public static GatheringManager Instance;

    public List<Bush> bushes = new List<Bush>();

    public List<GatherableFood> gatherableFoods = new List<GatherableFood>();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    
    public void GetBush(Bush bush)
    {
        bushes.Add(bush);
    }

    public GatherableFood GetCloseFood(Vector3 pos)
    {
        if (gatherableFoods.Count == 0)
        {
            return null;
        }
        //do logic
        GatherableFood food = gatherableFoods[0];
        float dist = Vector3.Distance(pos, food.transform.position);
        foreach (GatherableFood b in gatherableFoods)
        {
            if (Vector3.Distance(b.transform.position, pos) < dist)
            {
                food = b;
            }
        }


        return food;
    }
    public Bush GetCloseBush(Vector3 playerPos)
    {
        if (bushes.Count == 0)
        {
            return null;
        }
        //do logic
        Bush bush = bushes[0];
        float dist = Vector3.Distance(playerPos, bush.transform.position);
        foreach(Bush b in bushes)
        {
            if(Vector3.Distance(b.transform.position, playerPos) < dist)
            {
                bush = b;
            }
        }
        

        return bush;
    }

    public void BushDestroyed(Bush bush)
    {
        if (bushes.Contains(bush))
        {
            bushes.Remove(bush);
        }
    }
}
