using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public Transform assignedUnit;

    public Vector3 offset;

    public Image bar;
    private void Update()
    {
        if(assignedUnit != null)
        {
            PlaceOverAssignedUnit();
        }
        
    }
    void PlaceOverAssignedUnit()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(assignedUnit.position);
        pos += offset;

        transform.position = pos;
    }


    public void GetUnit(GameObject unit, float sizeFactor)
    {
        
         assignedUnit = unit.transform;
        transform.localScale *= sizeFactor;
        
    }

    
    
}
