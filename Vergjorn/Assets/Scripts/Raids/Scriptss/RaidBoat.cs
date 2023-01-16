using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidBoat : MonoBehaviour
{
    public List<VikingUnit> vikingsInBoat = new List<VikingUnit>();
        //Stand point
    public Transform[] vikingsStandPoints;
    int standPointIndex;

    //Boat places
    public Transform[] boatPlaces;
    int boatPlaceIndex;

    RaidManager raidManager;

    Vector3 moveFrom;
    Vector3 moveTo;
    float moveTime;
    float t;
    bool moving;
    private void Start()
    {
        raidManager = RaidManager.Instance;       
    }

    private void Update()
    {
        if (moving)
        {
            if(t < moveTime)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(moveFrom, moveTo, t / moveTime);
            }
            else
            {
                moving = false;
            }

            
        }
    }

    public void VikingEnterBoat(VikingUnit unit)
    {
        if (!vikingsInBoat.Contains(unit))
        {
            vikingsInBoat.Add(unit);
            unit.transform.SetParent(this.transform);

            unit.forcedIdle = true;

        }
    }

   

    public Vector3 MyStandPoint()
    {
        Vector3 pos = vikingsStandPoints[standPointIndex].position;
        standPointIndex += 1;
        if(standPointIndex >= vikingsStandPoints.Length)
        {
            standPointIndex = 0;
        }
        return pos;
    }

    public Vector3 MyBoatPlace()
    {
        Vector3 pos = boatPlaces[boatPlaceIndex].position;

        boatPlaceIndex += 1;
        if(boatPlaceIndex >= boatPlaces.Length)
        {
            boatPlaceIndex = 0;
        }

        return pos;

    }

    public float spaceMultiplier = 1.6f;

    public Vector3[] SetPositions(int unitAmount, Vector3 hitPoint)
    {
        int units = unitAmount + 1;

        Vector3[] positions = new Vector3[units];

        //
        float unitsInRow = 2 + Mathf.Ceil(units / 6);
        float rows = Mathf.Ceil(units / unitsInRow);
        float columns = Mathf.Ceil(units / rows);

        int unitCount = 0;
        for (int i = 0; i < rows; i++)
        {
            float remaningUnits = units - unitCount;

            if (remaningUnits < columns)
            {
                for (float k = -(remaningUnits / 2); k < (remaningUnits / 2); k++)
                {
                    Vector3 unitPos = new Vector3(k, 0, i) * spaceMultiplier + hitPoint;
                    positions[unitCount] = unitPos;
                    unitCount++;
                }
                break;
            }

            for (float j = -columns / 2; j < columns / 2; j++)
            {
                Vector3 unitPos = new Vector3(j, 0, i) * spaceMultiplier + hitPoint;
                Debug.Log(positions.Length.ToString() + "Unit count = " + unitCount.ToString());
                positions[unitCount] = unitPos;
                unitCount++;
                if (unitCount == units)
                {
                    break;
                }
            }
        }

        Vector3 avaragePos = Vector3.zero;
        foreach (Vector3 pos in positions)
        {
            avaragePos += (pos - hitPoint);

        }
        avaragePos = avaragePos / positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] -= avaragePos;
        }

        return positions;
    }


    public void ShipLeave(Vector3 from, Vector3 to, float time)
    {
        if (!moving)
        {
            moveFrom = from;
            moveTo = to;
            moveTime = time;
            t = 0;

            moving = true;
        }
    }
}
