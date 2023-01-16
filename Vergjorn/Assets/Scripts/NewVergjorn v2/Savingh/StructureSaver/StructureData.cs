using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum StructureType
{
    house,
    loggingHut,
    fishingHut,
    gard,
    trainingGrounds,
    smie,
    stabburd,
    myrovn,
    huntingHut,
    shipyard,
    volvehus, 
    shitShipyard,
    armory, 
    longhouse

}

[System.Serializable]
public class StructureData 
{
    public StructureType type;

    public Vector3 position;

    public Quaternion rotation;

    public bool built;

    public StructureLevel level;

    public int upkeepLevel;

    public bool dead;
}
[System.Serializable]
public enum StructureLevel
{
    one, two, three, four, five
}
