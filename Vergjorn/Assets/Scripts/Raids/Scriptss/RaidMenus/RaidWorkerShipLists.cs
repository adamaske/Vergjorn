using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class RaidWorkerShipLists : ScriptableObject
{
    public List<WorkerData> workers = new List<WorkerData>();

    public List<Ship> ships = new List<Ship>();

    public List<WorkerData> workersNotOnRaid = new List<WorkerData>();

    public bool returnWorkersFromRaid;
}
