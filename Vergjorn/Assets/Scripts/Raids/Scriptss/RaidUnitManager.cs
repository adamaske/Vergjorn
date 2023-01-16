using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidUnitManager : MonoBehaviour
{
    private RaidManager raidManager;
    public List<Worker> selectedUnits = new List<Worker>();

    
    public LayerMask interactableLayer;


    public int selectedUnitsButton = 0;
    public int giveTaskButton = 1;

    bool selecting;

    Vector2 mouseClick1;
    Vector2 mouseClick2;

    public GUIStyle mouseDragSkin;
    private void Start()
    {
        raidManager = RaidManager.Instance;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(selectedUnitsButton))
        {
            SelectUnit();
            StartSelecting();
        }

        if (Input.GetMouseButtonUp(selectedUnitsButton) && selecting)
        {
            EndSelection();
        }
        if (Input.GetMouseButtonDown(giveTaskButton))
        {
            GiveUnitsTask();
        }


    }


    public void StartSelecting()
    {
        selecting = true;
        mouseClick1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            ClearSelection();
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            Worker unit = hit.collider.GetComponent<Worker>();
            if(unit != null)
            {
                if (!selectedUnits.Contains(unit))
                {
                    selectedUnits.Add(unit);
                    unit.Selected();
                }
            }
        }
    }
    public void EndSelection()
    {
        selecting = false;
        mouseClick2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Rect selectRect = new Rect(mouseClick1.x, mouseClick1.y, mouseClick2.x - mouseClick1.x, mouseClick2.y - mouseClick1.y);

        foreach(Worker viking in raidManager.vikings)
        {
            if(viking != null)
            {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(viking.transform.position), true))
                {
                    if (!selectedUnits.Contains(viking))
                    {
                        selectedUnits.Add(viking);
                        viking.Selected();
                    }
                    
                }
            }
            
        }
    }
    
    void SelectUnit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, interactableLayer))
        {
            Worker unit = hit.collider.GetComponent<Worker>();
            if(unit != null)
            {
                if (!selectedUnits.Contains(unit))
                {
                    selectedUnits.Add(unit);
                    
                }
            }
        }
    }

    void ClearSelection()
    {
        foreach(Worker unit in selectedUnits)
        {
            if (raidManager.vikings.Contains(unit))
            {
                unit.Deselected();
            }
            
        }
        selectedUnits.Clear();
    }
    void GiveUnitsTask()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000, interactableLayer))
        {
            if(selectedUnits.Count == 0)
            {
                return;
            }
            RaidTask raidTask = hit.collider.GetComponent<RaidTask>();
            if(raidTask != null && !raidTask.dead)
            {
                if (raidTask.dead == false)
                {
                    foreach (Worker unit in selectedUnits)
                    {
                        if (unit != null)
                        {

                            unit.GetRaidTask(raidTask);
                        }

                    }
                }
                else
                {
                    return;
                }
               
                

                ClearSelection();
            }

            EnemyUnit enemyUnit = hit.collider.GetComponent<EnemyUnit>();
            if(enemyUnit != null)
            {
                foreach(Worker unit in selectedUnits)
                {
                    unit.GetEnemyUnit(enemyUnit);
                }
                ClearSelection();
            }

            if(enemyUnit == null && raidTask == null && selectedUnits.Count != 0)
            {
                Vector3[] poss = SetPositions(selectedUnits.Count , hit.point);
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetDestination(poss[i]);
                }
                //foreach(Worker unit in selectedUnits)
                //{
                //    unit.GetDestination(hit.point);
                //}


            }
        }
        
    }

    public float spaceMultiplier;
    
    public Vector3[] SetPositions(int unitAmount, Vector3 hitPoint)
    {
        int units = unitAmount + 1;

        Vector3[] positions = new Vector3[units];

        //
        float unitsInRow = 2 + Mathf.Ceil(units / 6);
        float rows = Mathf.Ceil(units / unitsInRow);
        float columns = Mathf.Ceil(units / rows);

        int unitCount = 0;
        for (int i = 0; i < rows; i++)
        {
            float remaningUnits = units - unitCount;

            if(remaningUnits < columns)
            {
                for(float k = -(remaningUnits/2); k < (remaningUnits / 2); k++)
                {
                    Vector3 unitPos = new Vector3(k, 0, i) * spaceMultiplier + hitPoint;
                    positions[unitCount] = unitPos;
                    unitCount++;
                }
                break;
            }

            for(float j = -columns/2; j < columns/2; j++)
            {
                Vector3 unitPos = new Vector3(j, 0, i) * spaceMultiplier + hitPoint;
                
                positions[unitCount] = unitPos;
                unitCount++;
                if(unitCount == units)
                {
                    break;
                }
            }
        }

        Vector3 avaragePos = Vector3.zero;
        foreach(Vector3 pos in positions)
        {
            avaragePos += (pos - hitPoint);

        }
        avaragePos = avaragePos / positions.Length;

        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] -= avaragePos;
        }

        return positions;
    }
}
