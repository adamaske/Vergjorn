using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringArea : MonoBehaviour
{
    public List<GatherableFood> foods = new List<GatherableFood>();

    private void Start()
    {
        foreach(GatherableFood food in foods)
        {
            food.GetGatheringArea(this);
        }
    }
    public GatherableFood GetCloseFood(Vector3 pos)
    {
        if(foods.Count == 0)
        {
            return null;
        }
        List<GatherableFood> foodWithoutWorker = new List<GatherableFood>();
        foreach (GatherableFood g in foods)
        {
            if(g.workers.Count == 0)
            {
                foodWithoutWorker.Add(g);
            }
        }
        if(foodWithoutWorker.Count != 0)
        {
            GatherableFood t = foodWithoutWorker[0];
            float dist = Vector3.Distance(t.transform.position, pos);

            foreach (GatherableFood gf in foodWithoutWorker)
            {
                float d = Vector3.Distance(gf.transform.position, pos);
                if (d < dist)
                {
                    dist = d;
                    t = gf;
                }
            }

            return t;
        }
        else
        {
            GatherableFood food = foods[0];
            float dist = Vector3.Distance(pos, food.transform.position);
            foreach (GatherableFood b in foods)
            {
                if (Vector3.Distance(b.transform.position, pos) < dist)
                {
                    food = b;
                }
            }


            return food;
        }

        
    }
}
