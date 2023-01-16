using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class StructurePlacer : MonoBehaviour
{
    //only able to place on the groiund alyer so a layer mask for the raycasts
    public LayerMask groundLayer;
    public LayerMask structureLayer;
    bool isPlacing;

    GameObject dummyGraphic;
    GameObject currentDummyGraphic;

    public Transform rotationRefrence;
    GameObject currentStructurePrefab;

    public int placeButtonID;
    public int quitPlacingButtonID = 1;

    public bool useGrid;
    public Grid grid;

    public float checkGroundRange;
    public float checkGroundRadius;


    private void Update()
    {
        if (isPlacing)
        {
            PlaceDummy();
        }
        if (isPlacing && MouseOverUI() && Input.GetMouseButtonDown(placeButtonID))
        {
            QuitPlacing();
        }

        if (isPlacing && Input.GetMouseButtonDown(placeButtonID) && !MouseOverUI())
        {

            Place(GetMouseWorldPos());

        }

        if (isPlacing && Input.GetMouseButtonDown(quitPlacingButtonID))
        {
            QuitPlacing();
        }
    }



    public bool CanPlace(Vector3 point, GameObject structurePrefab, Quaternion rotation)
    {
        BoxCollider collider = structurePrefab.GetComponent<BoxCollider>();
        Vector3 size = collider.transform.TransformVector(collider.bounds.size / 2);
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        size.z = Mathf.Abs(size.z);


        Collider[] hitColliders = Physics.OverlapBox(point + collider.center, size, rotationRefrence.rotation, structureLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<StructureGraphic>())
            {
                Debug.Log("Hit another structure");
                return false;
            }
        }

        if (!NoOverlapAndGround(point))
        {

            return false;
        }

        return true;
    }

    public bool NoOverlapAndGround(Vector3 point)
    {
        Structure s = currentStructurePrefab.GetComponent<Structure>();
        GameObject go = Instantiate(s.checkOverlapAndGroundParent);
        go.transform.position = point;

        List<Transform> t = new List<Transform>();
        foreach (Transform child in go.transform)
        {
            t.Add(child);
        }

        foreach (Transform to in t)
        {
            Collider[] colls = Physics.OverlapSphere(to.position, checkGroundRadius);
            foreach (Collider coll in colls)
            {
                Structure g = coll.GetComponent<Structure>();
                if (g != null && g != s)
                {
                    Debug.Log("Structure in the way");
                    return false;
                }
            }
        }

        foreach(Transform tk in t)
        {
            Vector3 pok = tk.transform.position;
            pok.y += 10;

            

            RaycastHit hit;
            if(!Physics.Raycast(pok, Vector3.down, out hit, checkGroundRange))
            {
                Debug.Log("Didnt hit ground");
                return false;
            }
        }


        

        return true;
    }

    void Place(Vector3 point)
    {

        if (CanPlace(point, currentStructurePrefab, rotationRefrence.rotation))
        {

            Instantiate(currentStructurePrefab, point, rotationRefrence.rotation, null);
        }
        else
        {

        }

        QuitPlacing();


    }






    void QuitPlacing()
    {
        isPlacing = false;
        DestroyDummy();
        dummyGraphic = null;
        currentStructurePrefab = null;
    }
    public void GetStructure(GameObject structurePrefab)
    {
        currentStructurePrefab = structurePrefab;
        dummyGraphic = structurePrefab.GetComponent<StructureGraphic>().thisGraphic;
        isPlacing = true;

        //SpawnDummy
        SpawnDummy();
    }

    bool HitGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            if (useGrid)
            {
                return grid.GetNearestAllowedPoint(hit.point);
            }
            else
            {
                return hit.point;
            }

        }
        return Vector3.zero;
    }

    void PlaceDummy()
    {
        currentDummyGraphic.transform.position = GetMouseWorldPos();
    }

    void SpawnDummy()
    {
        currentDummyGraphic = Instantiate(dummyGraphic, GetMouseWorldPos(), rotationRefrence.rotation);
    }

    void DestroyDummy()
    {
        if (currentDummyGraphic != null)
        {
            Destroy(currentDummyGraphic);
        }

    }
    bool MouseOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
