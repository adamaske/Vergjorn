using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class WorkerSelector : MonoBehaviour
{
    public static WorkerSelector Instance;

    public LayerMask layerMask;

    public int workerClickButton = 0;
   
    public bool selecting;
    Vector2 click1;
    Vector2 click2;
    public float spaceMultiplier = 1.6f;
    public List<Worker> selectedUnits = new List<Worker>();

    public bool thisIsRaid;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }



   
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(workerClickButton)  && !PauseMenu.Instance.Paused())
        {
            WorkerPressed();
        }


        if (Input.GetMouseButtonDown(workerClickButton) && !PauseMenu.Instance.Paused())
        {
            StartSelection();
        }
        if(Input.GetMouseButtonUp(workerClickButton) && selecting && !PauseMenu.Instance.Paused())
        {
            EndSelection();
        }

        if (Input.GetMouseButtonDown(1) && !PauseMenu.Instance.Paused())
        {
            GiveSelectedWorkerTask();
            
        }
        
    }
  
  
    void ClearSelection()
    {
        foreach(Worker worker in selectedUnits)
        {
            worker.Deselected();

        }
        selectedUnits.Clear();
    }
    void StartSelection()
    {
        click1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        selecting = true;
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            ClearSelection();
        }
        

        //GetOneHit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            Worker worker = hit.collider.GetComponent<Worker>();
            if(worker != null)
            {
                if (!selectedUnits.Contains(worker))
                {
                    selectedUnits.Add(worker);
                    worker.Selected();
                    if (thisIsRaid)
                    {

                    }
                    else
                    {
                        WorkerInfo.Instance.GetWorker(worker);
                    }
                    
                }
            }
        }
    }
    void EndSelection()
    {
        click2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        selecting = false;

        Rect selectRect = new Rect(click1.x, click1.y, click2.x - click1.x, click2.y - click1.y);

        if (thisIsRaid)
        {
            foreach(Worker worker in RaidManager.Instance.vikings)
            {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(worker.transform.position), true))
                {
                    if (!selectedUnits.Contains(worker))
                    {
                        selectedUnits.Add(worker);
                        worker.Selected();
                    }
                }
            }
            return;
        }
        else
        {
            foreach (Worker worker in PopulationManager.Instance.workersInGame)
            {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(worker.transform.position), true))
                {
                    if (!selectedUnits.Contains(worker))
                    {
                        selectedUnits.Add(worker);
                        worker.Selected();
                    }
                }
            }
        }

        
    }
    void GiveSelectedWorkerTask()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {        
            if(selectedUnits.Count == 0)
            {
                return;
            }
            EnemyUnit enemy = hit.collider.GetComponent<EnemyUnit>();
            if(enemy != null)
            {
                Debug.Log("Hit enemy unit");
                foreach(Worker worker in selectedUnits)
                {
                    worker.GetEnemy(enemy);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            RaidTask raidTask = hit.collider.GetComponent<RaidTask>();
            if (raidTask != null)
            {
                Debug.Log("Hit raidTask");
                if (raidTask.dead)
                {
                    Debug.Log("Hit building, but its dead");
                }
                 foreach (Worker unit in selectedUnits)
                 {
                    if (unit != null)
                    {
                       unit.GetRaidTask(raidTask);
                    }

                 }
                
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            BuildingStructures b = hit.collider.GetComponent<BuildingStructures>();
            if (b != null)
            {
                if (b.unbuilt == true)
                {
                    foreach (Worker worker in selectedUnits)
                    {
                        worker.GetBuilding(b);
                    }
                    AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                    ClearSelection();
                    return;
                }

            }
            Farm farm = hit.collider.GetComponent<Farm>();
            if(farm != null)
            {
                foreach (Worker worker in selectedUnits)
                {
                    worker.GetFarm(farm);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            HuntingLodge hl = hit.collider.GetComponent<HuntingLodge>();
            if(hl != null)
            {
                foreach (Worker worker in selectedUnits)
                {
                    worker.GetHuntingLodge(hl);

                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }

            GatherableFood gf = hit.collider.GetComponent<GatherableFood>();
            if(gf != null)
            {
                foreach (Worker worker in selectedUnits)
                {
                    worker.GetGatherableFood(gf);

                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            FishingHut hut = hit.collider.GetComponent<FishingHut>();
            if(hut != null)
            {
                foreach(Worker worker in selectedUnits)
                {
                    if (hut.CanGetMoreWorkers())
                    {
                        if(worker.state == Worker.WorkerState.fishing)
                        {
                            worker.StopFishing();
                        }
                        worker.GetFishingHut(hut);
                    }
                    else
                    {
                        NotificationManager.Instance.OpenNotification("No", "Theres no more room in the fishing hut, upgrade it or build another", 5f, null);
                    }
                    
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            Armory armory = hit.collider.GetComponent<Armory>();
            if(armory != null)
            {
                float workersSentToArmory = 0;
                foreach(Worker worker in selectedUnits)
                {
                    if(worker.myData.hasHelmet && worker.myData.hasShield && worker.myData.hasSword)
                    {


                    }
                    else
                    {
                        if (armory.HasWeaponsForThisWorker(worker))
                        {
                            worker.GetArmory(armory);
                            workersSentToArmory += 1;
                        }
                    }
                   
                }
                if(WeaponsManager.Instance.weapons.Count == 0)
                {
                    NotificationManager.Instance.OpenNotification("No weapons", "You dont have any weapons to give, use the smith to produce weapons", 5f, null);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            Myrovn myrovn = hit.collider.GetComponent<Myrovn>();
            if(myrovn != null)
            {
                foreach(Worker worker in selectedUnits)
                {
                    worker.GetMyrovn(myrovn);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            TrainingGrounds trainingGrounds = hit.collider.GetComponent<TrainingGrounds>();
            if(trainingGrounds != null)
            {
                foreach(Worker worker in selectedUnits)
                {
                    if(worker.workerType == WorkerType.Child)
                    {
                        NotificationManager.Instance.OpenNotification("No", "Children cant become vikings", 3f, null);
                        return;
                    }
                    if(worker.forcedIdle == false)
                    {
                        if (worker.workerType == WorkerType.Adult)
                        {
                            if (trainingGrounds.CanGetNewWorker())
                            {
                                trainingGrounds.AddWorker(worker);

                            }
                        }
                    }
                    
                    
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }

            Forest f = hit.collider.GetComponent<Forest>();
            if(f != null)
            {
                foreach(Worker worker in selectedUnits)
                {
                    //worker.StartLogging(f);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            Tree t = hit.collider.GetComponent<Tree>();
            if( t != null)
            {
                foreach(Worker worker in selectedUnits)
                {
                    worker.StartLogging(t);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }

            Myrmalm myrmalm = hit.collider.GetComponent<Myrmalm>();
            if(myrmalm != null)
            {
                Debug.Log("Hit myrmalm");
                foreach(Worker worker in selectedUnits)
                {
                    worker.StartMyrmalmGathering(myrmalm.myMyr);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            Myr myr = hit.collider.GetComponent<Myr>();
            if (myr != null)
            {
                foreach (Worker worker in selectedUnits)
                {
                    worker.StartMyrmalmGathering(myr);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }
            
            
            Shipyard shipyard = hit.collider.GetComponent<Shipyard>();
            if(shipyard != null)
            {
                foreach (Worker worker in selectedUnits)
                {
                    worker.GetShipyard(shipyard);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }

            
            Smie smie = hit.collider.GetComponent<Smie>();
            if(smie!= null)
            {
                foreach(Worker worker in selectedUnits)
                {
                    worker.GetSmie(smie);
                }
                AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                ClearSelection();
                return;
            }

            BabyMaking babyMaker = hit.collider.GetComponent<BabyMaking>();
            if(babyMaker != null)
            {
                if (babyMaker.EnoughFoodForBaby())
                {
                    foreach (Worker worker in selectedUnits)
                    {
                        if (babyMaker.CanMakeBaby())
                        {
                            if(worker.workerType != WorkerType.Child && worker.workerType != WorkerType.Trell)
                            {
                                babyMaker.GetWorker(worker);
                            }
                            else
                            {
                                NotificationManager.Instance.OpenNotification("No", "Children cant make babies!", 5f, null);
                            }
                            
                        }
                    }
                    AudioManager.Instance.PlayWorkerVoiceline(true, 1, selectedUnits[0].transform.position, AudioType.gotTask);
                    ClearSelection();
                    return;
                }
               
            }

            if(shipyard == null && b == null &&
                smie == null && t == null && f == null && myr == null && myrmalm == null)
            {
                Vector3[] pos = SetPositions(selectedUnits.Count, hit.point);
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetDestination(pos[i]);      
                }

                AudioManager.Instance.PlayWorkerVoiceline(true, 1, hit.point, AudioType.happy);
            }
        }
    }
    

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

            if (remaningUnits < columns)
            {
                for (float k = -(remaningUnits / 2); k < (remaningUnits / 2); k++)
                {
                    Vector3 unitPos = new Vector3(k, 0, i) * spaceMultiplier + hitPoint;
                    positions[unitCount] = unitPos;
                    unitCount++;
                }
                break;
            }

            for (float j = -columns / 2; j < columns / 2; j++)
            {
                Vector3 unitPos = new Vector3(j, 0, i) * spaceMultiplier + hitPoint;

                positions[unitCount] = unitPos;
                unitCount++;
                if (unitCount == units)
                {
                    break;
                }
            }
        }

        Vector3 avaragePos = Vector3.zero;
        foreach (Vector3 pos in positions)
        {
            avaragePos += (pos - hitPoint);

        }
        avaragePos = avaragePos / positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] -= avaragePos;
        }

        return positions;
    }
    void WorkerPressed()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000))
        {
            Worker worker = hit.collider.GetComponent<Worker>();
            if(worker != null)
            {
                
               

            }
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
