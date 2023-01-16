using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject workerPrefab;
    public bool loadOnStart = true;
    public List<WorkerData> workerDatas;
    public static SaveManager Instance;
    public RaidWorkerShipLists raidConnect;

    public string[] raidSaveNamesToDeleteOnNewGame;

    [Header("Prefabs")]
    public GameObject loggingHut;
    public GameObject house;
    public GameObject fishingHut;
    public GameObject gard;
    public GameObject huntingHut;
    public GameObject myrovn;
    public GameObject shipyard;
    public GameObject smie;
    public GameObject stabburd;
    public GameObject trainingGrounds;
    public GameObject shitShipyard;
    public GameObject armory;
    public GameObject longhouse;
    [Space]
    [Header("What to do on start")]
    //set by main menu scene
    public WhatToDoOnStart wtdos;

    public SaveData saveData;

    [Header("New game stats")]
    public float numberOfWorkers;
    public Transform[] spawnPoins;

    public string workersSaveFileName;
    public string structureSaveFileName;

    public FloatVariable gold;
    public RaidReward raidReward;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (wtdos.loadCurrentSave)
        {
            OnLoad();
        }else if (wtdos.loadNewGameSave)
        {
            LoadNewGame();
        }

        if(raidConnect.returnWorkersFromRaid== true)
        {
            VikingsReturnFromRaid();
        }
    }

    void CheckReward()
    {
        if (raidReward.goldReward != 0)
        {
            gold.value += raidReward.goldReward;
            Debug.Log("Added gold: " + raidReward.goldReward);

            NotificationManager.Instance.OpenNotification("Gold looted!", "You looted " + raidReward.goldReward.ToString() + " gold on the raid!", 10f, null);
            raidReward.goldReward = 0;

            
        }
    }
    void VikingsReturnFromRaid()
    {
        
        raidConnect.returnWorkersFromRaid = false;
    }
    public void OnSave()
    {
        ShipManager.Instance.SaveShips();
        SaveWorkers();
        SaveStructures();
        ResourceSaver.Instance.SaveResources();
        WeaponsManager.Instance.SaveWeapons();
        
        TimeManager.Instance.SaveTime();
    }

    public void LoadNewGame()
    {
        foreach(string s in raidSaveNamesToDeleteOnNewGame)
        {
            FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/" + s + ".save");
            if (info.Exists)
            {
                info.Delete();
            }
        }

        ResourceSaver.Instance.LoadResources();
        ResourceSaver.Instance.ResetToNewGame();
        TimeManager.Instance.LoadNewGame();
    }
    public void OnLoad()
    {
        ResourceSaver.Instance.LoadResources();
        TimeManager.Instance.LoadTime();
        LoadWorkers();

        LoadStructures();

        ShipManager.Instance.LoadShips();

        WeaponsManager.Instance.SaveWeapons();

        Invoke("CheckReward", 0.1f);
    }

   
    
    public void DestroyAllStructures()
    {
        Structure[] s = new Structure[StructureManager.Instance.allStructures.Count];
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = StructureManager.Instance.allStructures[i];
        }
        for (int i = 0; i < s.Length; i++)
        {
            s[i].Die();
        }
    }
   
    

    public void DeleteSaves()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete();
        Directory.CreateDirectory(path);
    }

    #region Workers Logic

    public void SaveWorkers()
    {
        //Delcare new list of workers
        List<WorkerData> w = new List<WorkerData>();

        //Using population manager works because all workers give themselfs to populationManager.Instance on their start function
        for (int i = 0; i < PopulationManager.Instance.workersInGame.Count; i++)
        {
            Worker worker = PopulationManager.Instance.workersInGame[i];
            WorkerData p = new WorkerData();
            p.myName = worker.myName;

            
            p.position = worker.transform.position;
            p.rotation = worker.transform.rotation;

            w.Add(worker.myData);
        }
        //This can be swaped with just using the same as before, but only adding new ones, istead of declareing a whole new
        WorkerSaver.Instance.workers = w;
        //Debug.Log(WorkerSaver.Instance.workers.Count + "This many workers saved");
        SerializationManager.Save(workersSaveFileName, WorkerSaver.Instance);
    }

    
    public void LoadWorkers()
    {
        //Kill all current workers first
        //Decalare new array of workers, uses array so it doesnt update/shorten the count when one is null, keeps its length
        Worker[] ws = FindObjectsOfType(typeof(Worker)) as Worker[];
        Debug.Log(ws.Length + "workers killed");
        for (int i = 0; i < ws.Length; i++)
        {
            ws[i].Die(true);
        }
        //Can just use a float instead? and use the 0 index every for loop?

        WorkerSaver.Instance = (WorkerSaver)SerializationManager.Load(Application.persistentDataPath + "/saves/" +  workersSaveFileName +".save");

        Debug.Log(WorkerSaver.Instance.workers.Count + " workers spawned");
        //Instantiate all the workers in WorkerSaver.Instance
        for (int i = 0; i < WorkerSaver.Instance.workers.Count; i++)
        {

            WorkerData data = WorkerSaver.Instance.workers[i];
            GameObject obj = Instantiate(workerPrefab);
            Worker worker = obj.GetComponent<Worker>();

            //maybe have to find the agent itself, instead of the worker, looks not very good to have it in the awake function
            worker.TurnOffNavMeshAgent();

            worker.workerType = data.type;

            worker.myData = data;
            worker.transform.position = data.position;
            worker.transform.rotation = data.rotation;
            worker.myName = data.myName;
            worker.onRaid = false;
            worker.TurnOnNavMeshAgent();
        }
    }

    public void KillAllWorkers()
    {
        Worker[] w = new Worker[PopulationManager.Instance.workersInGame.Count];
        for (int i = 0; i < w.Length; i++)
        {
            w[i] = PopulationManager.Instance.workersInGame[i];
        }

        for (int i = 0; i < w.Length; i++)
        {
            w[i].Die(false);

        }
    }

    #endregion

    #region Structure logic
    public void SaveStructures()
    {
        //Declares new list of structureDatas
        List<StructureData> d = new List<StructureData>();
        //For each structure in StructureManager, add it to the new list "d"
        for (int i = 0; i < StructureManager.Instance.allStructures.Count; i++)
        {
            StructureData s = StructureManager.Instance.allStructures[i].myData;
            s.type = StructureManager.Instance.allStructures[i].myData.type;

            d.Add(s);
        }
        //Set strucutre savers version equals to the new D
        StructureSaver.Instance.structures = d;

        SerializationManager.Save(structureSaveFileName, StructureSaver.Instance);
        

    }

    public void LoadStructures()
    {
        Debug.Log(StructureManager.Instance.allStructures.Count.ToString());
        //Destroy all structures
        Structure[] qs = new Structure[StructureManager.Instance.allStructures.Count];
        for (int i = 0; i < qs.Length; i++)
        {
            qs[i] = StructureManager.Instance.allStructures[i];
        }
        for (int i = 0; i < qs.Length; i++)
        {
            Debug.Log("Killed building");
            qs[i].Die();
        }


        StructureSaver.Instance = (StructureSaver)SerializationManager.Load(Application.persistentDataPath + "/saves/" + structureSaveFileName +".save");

        //Instantiate the structures
        
        for (int i = 0; i < StructureSaver.Instance.structures.Count; i++)
        {
            StructureData data = StructureSaver.Instance.structures[i];

            GameObject obj = Instantiate(GetPrefab(data));

            Structure s = obj.GetComponent<Structure>();

            s.myData = data;

            obj.transform.position = data.position;
            obj.transform.rotation = data.rotation;

            s.myType = data.type;
        }
        
    }

    GameObject GetPrefab(StructureData data)
    {
        if (data.type == StructureType.house)
        {

            return house;
        }
        if (data.type == StructureType.loggingHut)
        {
            
            return loggingHut;
        }
        if (data.type == StructureType.fishingHut)
        {
            
            return fishingHut;
        }
        if (data.type == StructureType.gard)
        {
            return gard;
        }
        if (data.type == StructureType.trainingGrounds)
        {
            return trainingGrounds;
        }
        if (data.type == StructureType.smie)
        {
            return smie;
        }
        if (data.type == StructureType.myrovn)
        {
            return myrovn;
        }
        if (data.type == StructureType.stabburd)
        {
            return stabburd;
        }
        if (data.type == StructureType.shipyard)
        {
            return shipyard;
        }
        if (data.type == StructureType.huntingHut)
        {
            return huntingHut;
        }
        if(data.type == StructureType.armory)
        {
            return armory;
        }
        if(data.type == StructureType.shitShipyard)
        {
            return shitShipyard;
        }
        if(data.type == StructureType.longhouse)
        {
            return longhouse;
        }
        Debug.Log("Returned end");
        return house;
    }
    #endregion
}
