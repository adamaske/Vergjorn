using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMover : MonoBehaviour
{
    Structure currentStructure;

    public Grid grid;
    public bool useGrid;
    public bool allowRotation;

    public LayerMask groundLayer;

    public bool selecting;

    public bool moving;

    public BuildManager buildManager;

    public Vector3 firstPos;
    public  float checkGroundRadius;
    private LayerMask structureLayer;
    public int checkGroundRange;

    public bool allowedToPlace;

    float t;

    public float waitForPlace = 0.1f;
    public float rotationSens;
    Vector3 offset;

    public float movingAlpha = 60;
    public float standardAlpha;

    Quaternion originalRot;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            selecting = !selecting;
        }

        if (selecting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckHit();
            }
        }

        if (moving)
        {
           
            currentStructure.transform.position = grid.GetNearestAllowedPoint(MouseWorldPos() + offset);

            //Rotation
            float rot = Input.GetAxis("Rotation") * rotationSens * Time.deltaTime;
            currentStructure.transform.Rotate(0, rot, 0);

            if(t < waitForPlace)
            {
                t += Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (CanPlace(currentStructure.transform.position, currentStructure.gameObject, currentStructure.transform.rotation))
                    {
                        moving = false;


                        selecting = false;
                    }
                    else
                    {
                        Debug.Log("Build Manager didnt allow placement");
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    currentStructure.transform.position = firstPos;

                    currentStructure.transform.rotation = originalRot;

                    moving = false;

                    selecting = false;
                }
            }
           
        }
    }
    public bool CanPlace(Vector3 point, GameObject structurePrefab, Quaternion rotation)
    {
        BoxCollider collider = structurePrefab.GetComponent<BoxCollider>();
        Vector3 size = collider.transform.TransformVector(collider.bounds.size);
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        size.z = Mathf.Abs(size.z);


        Collider[] hitColliders = Physics.OverlapBox(point + collider.center, size, currentStructure.transform.rotation, structureLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            foreach(Collider c in hitColliders)
            {
                Structure g = c.GetComponent<Structure>();
                if (g != null && g != currentStructure)
                {
                    
                    return false;
                }
            }
        }

        if (!NoOverlapAndGround(point))
        {
            Debug.Log("Overlap or didnt hit ground");
            return false;
        }
        return true;
    }
    public bool NoOverlapAndGround(Vector3 point)
    {
        Structure s = currentStructure;
        GameObject go = currentStructure.checkOverlapAndGroundParent;
        go.transform.position = point;

        List<Transform> t = new List<Transform>();
        foreach (Transform child in go.transform)
        {
            t.Add(child);
        }

        foreach (Transform to in t)
        {
            Collider[] colls = Physics.OverlapSphere(to.position, checkGroundRadius, structureLayer);
            foreach (Collider coll in colls)
            {
                Structure g = coll.GetComponent<Structure>();
                if (g != null && g != s)
                {
                    
                    Debug.Log("Hit structure");
                    return false;
                }
            }
        }
        foreach (Transform tk in t)
        {
            Vector3 pok = tk.transform.position;
            pok.y += 10;
            RaycastHit hit;
            if (!Physics.Raycast(pok, Vector3.down, out hit, checkGroundRange, groundLayer))
            {

                Debug.Log("Didnt hit ground");
                return false;
            }
        }



      
        return true;
    }

    void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000))
        {
            Structure s = hit.collider.GetComponent<Structure>();
            if(s != null)
            {
                originalRot = s.transform.rotation;
                offset = s.transform.position - MouseWorldPos();
                firstPos = s.transform.position;
                currentStructure = s;

                t = 0;

                moving = true;

                selecting = false;

                allowedToPlace = false;

              
            }
            else
            {
                currentStructure = null;

                moving = false;

                selecting = false;
            }
        }
    }

    Vector3 MouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            
                return hit.point;
            
        }

        return Vector3.zero;
    }


}
