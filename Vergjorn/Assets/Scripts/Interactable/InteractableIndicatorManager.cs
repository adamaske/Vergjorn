using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIndicatorManager : MonoBehaviour
{
    public GameObject hoveredObject;

    public GameObject indicatorGO;
    bool open;

    private void Start()
    {
        ClearSelected();
    }
    private void Update()
    {
        CheckHit();
        /*
        if (open)
        {
            Renderer[] rs = hoveredObject.GetComponentsInChildren<Renderer>();
            Bounds bigBounds = new Bounds();
            foreach(Renderer r in rs)
            {
                bigBounds.Encapsulate(r.bounds);
            }

            float diameter = bigBounds.size.z;

            indicatorGO.transform.localScale = new Vector3(diameter, 1, diameter);
            
            
            Vector3 pos = bigBounds.center;
            pos.y = 0;
            indicatorGO.transform.position = pos;
        }
        */
    }
    void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000))
        {
            Interactable i = hit.collider.gameObject.GetComponent<Interactable>();

            if(i != null)
            {
                SelectObject(i.gameObject);
            }
            else
            {
                ClearSelected();
            }
        }
    }

    void SelectObject(GameObject g)
    {
        hoveredObject = g;

        if (!open)
        {
            open = true;
            indicatorGO.SetActive(true);
        }

        //Vector3 pos = hoveredObject.transform.position;
        //pos.y = 0;
        //indicatorGO.transform.position = pos;


        
    }

    void ClearSelected()
    {
        open = false;
        hoveredObject = null;
        indicatorGO.SetActive(false);
    }
}
