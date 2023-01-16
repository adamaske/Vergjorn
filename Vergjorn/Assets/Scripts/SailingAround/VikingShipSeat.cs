using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingShipSeat : MonoBehaviour
{
    public Worker workerInMe;

    
    public bool HasWorker()
    {
        if(workerInMe == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void GeWorker(Worker w)
    {
        workerInMe = w;
    }

    public void RemoveWorker(Worker w)
    {
        if(workerInMe == w)
        {
            workerInMe = null;
        }
    }
}
