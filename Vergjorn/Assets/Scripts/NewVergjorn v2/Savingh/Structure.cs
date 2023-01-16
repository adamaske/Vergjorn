using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public StructureData myData;
    public StructureType myType;

    BuildingStructures b;

    public GameObject checkOverlapAndGroundParent;
    private void Start()
    {
        
        StructureManager.Instance.GetStructure(this);
        
      
        b = GetComponent<BuildingStructures>();
        if(b != null)
        {
            if (myData.built)
            {
                b.Built(false);
            }
        }
        
    }

  
    private void Update()
    {
        myData.type = myType;
        myData.position = transform.position;
        myData.rotation = transform.rotation;
        if (myData.built == false && b.unbuilt == false)
        {
            myData.built = true;
        }

    }

    public void Die()
    {
        StructureManager.Instance.RemoveStructure(this);
        Destroy(gameObject);
    }
}
