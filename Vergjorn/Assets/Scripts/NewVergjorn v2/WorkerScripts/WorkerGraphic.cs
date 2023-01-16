using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerGraphic : MonoBehaviour
{

    public Worker worker;

    [Header("Graphics")]
    public GameObject femaleGraphic;
    public GameObject maleGraphic;
    [Space]
    public GameObject maleChildGraphic;
    public GameObject femaleChildGraphic;
    [Space]
    public GameObject trellGraphic;
    [Space]
    public GameObject maleVikingGraphic;
    public GameObject femaleVikingGraphic;

    [Space]
    public GameObject currentGraphic;
    public Transform graphicParent;
    
    public void SetGraphic()
    {
        if (currentGraphic != null)
        {
            Destroy(currentGraphic);
        }

        if (worker.workerType == WorkerType.Adult)
        {
            if (worker.myName.male)
            {
                currentGraphic = Instantiate(maleGraphic, graphicParent.transform.position, graphicParent.transform.rotation, graphicParent);
                Debug.Log("Spahned male");
            }
            else
            {
                Debug.Log("Spawned female");
                currentGraphic = Instantiate(femaleGraphic, graphicParent.transform.position, graphicParent.transform.rotation, graphicParent);
            }
        }
        if (worker.workerType == WorkerType.Viking)
        {
            if (worker.myName.male)
            {
                currentGraphic = Instantiate(maleVikingGraphic, graphicParent.transform.position, graphicParent.transform.rotation, graphicParent);
            }
            else
            {
                currentGraphic = Instantiate(femaleVikingGraphic, graphicParent.transform.position, graphicParent.transform.rotation, graphicParent);
            }
        }
        if (worker.workerType == WorkerType.Child)
        {
            if (worker.myName.male)
            {
                currentGraphic = Instantiate(maleChildGraphic, graphicParent.transform.position, graphicParent.transform.rotation, graphicParent);
            }
            else
            {
                currentGraphic = Instantiate(femaleChildGraphic, graphicParent.transform.position, graphicParent.transform.rotation, graphicParent);
            }
        }
    }

}
