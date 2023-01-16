using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingShip : MonoBehaviour
{
    public List<VikingShipSeat> seats = new List<VikingShipSeat>();

    public Transform workerWalkToPoint;
    public float AvailableSeats()
    {
        float k = 0;

        for (int i = 0; i < seats.Count; i++)
        {
            if (!seats[i].HasWorker())
            {
                k += 1;
            }
        }

        return k;
    }



}
