using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class RaidManager : MonoBehaviour
{

    public enum RaidStage { standby, settingup, raidgoing, raidFinished}
    public RaidStage stage;
    public static RaidManager Instance;

    public RaidWorkerShipLists workersList;

    [Header("Lists")]
    public List<Worker> vikings = new List<Worker>();
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    public List<RaidTask> raidTasks = new List<RaidTask>();
    [Space]
    public float totalRaidHealth;
    public float raidCurrentHealth;
    [Space]

    [Header("Viking Spawning")]
    public GameObject vikingPrefab;
    public Transform vikingSpawnPoint;

    [Header("Standby")]
    public float standbyTimer;
    float curStandby;
    
    [Header("Raid Timer")]
    public float raidTime;
    float currentTime;
    public TextMeshProUGUI raidTimerText;
    public float minutes;
    public float seconds;


    public bool addBuildingGraphicForeachGetRaid = true;

    [Header("Ending setup")]
    public RaidReward raidReward;
    public WhatToDoOnStart wtdos;

    public bool saveAfterRaid;

    public RaidStructureSpawner structureGraphicSpawner;
    private void Awake()
    {
        Instance = this;
        
    }

    

    private void Update()
    {
        switch (stage)
        {
            case RaidStage.standby:
                if(curStandby < standbyTimer)
                {
                    curStandby += Time.deltaTime;
                }
                else
                {
                    stage = RaidStage.settingup;
                }
                break;
            case RaidStage.settingup:
                if (!hasSpawned)
                {
                    SpawnVikings();
                    hasSpawned = true;
                }
                
                break;
            case RaidStage.raidgoing:
                float time = raidTime - currentTime;
                minutes = Mathf.Floor(time / 60);
                seconds = time - minutes * 60;
                seconds = Mathf.RoundToInt(seconds);
                raidTimerText.text = minutes.ToString() + " : " + seconds.ToString();

                if(currentTime < raidTime)
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    EndRaid();
                }

                break;
            case RaidStage.raidFinished:

                break;

        }


        

        RaidHealth();
        


        
      
    }

    #region Get And Remove objects
    public void GetViking(Worker v)
    {
        if (!vikings.Contains(v))
        {
            vikings.Add(v);
        }
        
    }
    public void GetEnemyUnit(EnemyUnit e)
    {
        if (!enemyUnits.Contains(e))
        {
            enemyUnits.Add(e);
        }
        
    }
    public void GetRaidTask(RaidTask r)
    {
        if (!raidTasks.Contains(r))
        {
            raidTasks.Add(r);
            if (addBuildingGraphicForeachGetRaid)
            {
                structureGraphicSpawner.AddRaidTaskGraphic(r);
            }
            Debug.Log("Added building to raid manager");
        }
        
    }

    public void RemoveEnemyUnit(EnemyUnit u)
    {
        if (enemyUnits.Contains(u))
        {
            enemyUnits.Remove(u);
        }
    }
    public void RemoveViking(Worker v)
    {
        if (vikings.Contains(v))
        {
            vikings.Remove(v);
        }
    }
    public void RemoveRaidTask(RaidTask r)
    {
        if (raidTasks.Contains(r))
        {
            raidTasks.Remove(r);
        }
    }

    #endregion

    public void RaidHealth()
    {
        float totalHealth = 0;
        float currentHealth = 0;

        for (int i = 0; i <raidTasks.Count; i++)
        {
            totalHealth += raidTasks[i].maxHealth;
            currentHealth += raidTasks[i].health;
        }

        totalRaidHealth = totalHealth;
        raidCurrentHealth = currentHealth;

    }

    public float MaxRaidHealth()
    {
        float totalHealth = 0;
        

        for (int i = 0; i < raidTasks.Count; i++)
        {
            totalHealth += raidTasks[i].maxHealth;
            
        }

        return totalHealth;
    }
    public float CurrentRaidHealth()
    {
        
        float currentHealth = 0;

        for (int i = 0; i < raidTasks.Count; i++)
        {
           
            currentHealth += raidTasks[i].health;
        }

        return currentHealth;
    }

    public bool RaidCompleted()
    {
        if(raidCurrentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void GetClosestEnemyUnit(Worker viking)
    {
        if(enemyUnits.Count == 0)
        {
            viking.GetEnemyUnit(null);
            return;
        }
        EnemyUnit unit = enemyUnits[0];
        
        for (int i = 0; i < enemyUnits.Count; i++)
        {

            float dist = Vector3.Distance(viking.transform.position, enemyUnits[i].transform.position);
            if(dist < Vector3.Distance(unit.transform.position, viking.transform.position))
            {
                unit = enemyUnits[i];
            }

        }

        viking.GetEnemyUnit(unit);
    }

    public EnemyUnit VikingGetEnemy(Worker viking)
    {
        if(enemyUnits.Count == 0)
        {
            return null;
        }
        EnemyUnit unit = enemyUnits[0];

        for (int i = 0; i < enemyUnits.Count; i++)
        {

            float dist = Vector3.Distance(viking.transform.position, enemyUnits[i].transform.position);
            if (dist < Vector3.Distance(unit.transform.position, viking.transform.position))
            {
                unit = enemyUnits[i];
            }

        }

        return unit;
    }
    
    void SpawnVikings()
    {
        Vector3[] pos = SetPositions(workersList.workers.Count, vikingSpawnPoint.position);
        for (int i = 0; i < workersList.workers.Count; i++)
        {
            
            GameObject go = Instantiate(vikingPrefab, pos[i], transform.rotation, null);
            Worker v = go.GetComponent<Worker>();
            WorkerData data = workersList.workers[i];

            v.myData = data;

            v.myName = data.myName;
            v.workerType = data.type;

            v.onRaid = true;

            vikings.Add(v);
            //Set combat levels etc
        }

        stage = RaidStage.raidgoing;
    }
  
    public float spaceMultiplier = 1.6f;
    private bool hasSpawned;

    public Vector3[] SetPositions(int unitAmount, Vector3 hitPoint)
    {
        int units = unitAmount +1;

        Vector3[] positions = new Vector3[units];

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
    
    public float GoldCollected()
    {
        float gold = 0;

        for (int i = 0; i < vikings.Count; i++)
        {
            gold += vikings[i].storedGold;
        }

        return gold;
    }
    public void EndRaid()
    {
        wtdos.loadCurrentSave = true;
        wtdos.loadNewGameSave = false;
        SetReward();

        //Check for completed raid

        RaidSaving.Instance.SaveRaid(RaidCompleted());

        /*
        workersList.workers.Clear();
        for (int i = 0; i < vikings.Count; i++)
        {
            workersList.workers.Add(vikings[i].myData);
        }
        workersList.returnWorkersFromRaid = true;

        //Create new saveDataFile?
        List<WorkerData> newWorkers = new List<WorkerData>();
        for (int i = 0; i < workersList.workersNotOnRaid.Count; i++)
        {
            newWorkers.Add(workersList.workersNotOnRaid[i]);
        }
        for (int i = 0; i < vikings.Count; i++)
        {
            newWorkers.Add(vikings[i].myData);
        }
        WorkerSaver.Instance.workers = newWorkers;
        Debug.Log(WorkerSaver.Instance.workers.Count.ToString());
        if (saveAfterRaid)
        {
            Debug.Log("Saved: " + WorkerSaver.Instance.workers.Count);
            SerializationManager.Save("Workers", WorkerSaver.Instance);

        }
        */
        SceneLoader.Instance.LoadScene("Level_1");
    }

    void SetReward()
    {
        Debug.Log("Gold collected: " + GoldCollected().ToString());
        raidReward.goldReward = GoldCollected();
    }

    public void ShipLeave()
    {
        EndRaid();   
    }
   
}

