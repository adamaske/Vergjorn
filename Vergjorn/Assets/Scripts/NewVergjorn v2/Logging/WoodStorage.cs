using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStorage : MonoBehaviour
{
    private StructureManager structureManager;

    public Transform standPoint;

    public float woodCapacityGiven;

    BuildingStructures buildingStructures;

    public bool checkForBuilt;
    public bool structureManagerHasMe;
    void Start()
    {
        structureManager = StructureManager.Instance;

        buildingStructures = GetComponent<BuildingStructures>();

        if (checkForBuilt && buildingStructures != null)
        {
            if (buildingStructures.unbuilt)
            {
                structureManagerHasMe = false;
            }
            else
            {
                structureManagerHasMe = true;
                structureManager.GetWoodStorage(this);
            }
        }
        else
        {
            structureManager.GetWoodStorage(this);
        }
       
    }

    private void Update()
    {
        
        if (checkForBuilt && !structureManagerHasMe && buildingStructures != null)
        {
            if(buildingStructures.unbuilt == false)
            {
                structureManagerHasMe = true;
                structureManager.GetWoodStorage(this);
            }
           
        }
    }

    public void GetWood(float amount)
    {

    }
    public Vector3 GetStandPoint()
    {
        Vector3 pos = standPoint.transform.position;

        return pos;
    }

    private void OnDestroy()
    {
        if (structureManager.woodStorages.Contains(this))
        {
            structureManager.woodStorages.Remove(this);
        }
        
    }

    public float WoodCapacityGiven()
    {
        return woodCapacityGiven;
    }
}
