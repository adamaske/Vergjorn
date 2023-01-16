using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    GatheringManager gatheringManager;
    void Start()
    {
        gatheringManager = GatheringManager.Instance;

        gatheringManager.GetBush(this);
    }

    public void BushGathered()
    {
        gatheringManager.BushDestroyed(this);
        Destroy(this.gameObject);
    }
}
