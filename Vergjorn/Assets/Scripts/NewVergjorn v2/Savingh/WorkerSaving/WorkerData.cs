using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerData 
{
    public string workerName;

    public Vector3 position;
    public Quaternion rotation;

    public int combatLevel;

    public bool hasSword;

    public bool hasShield;

    public bool hasHelmet;

    public WorkerType type;

    public float combatXP;
    public float goalCombatXP;

    public int workerLevel;
    public float workerXP;
    public float goalWorkerXP;

    public Name myName;
}

[System.Serializable]
public enum Type
{
    child,
    adult,
    viking,
    trell
}




