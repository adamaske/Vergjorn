using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class RaidSaving : MonoBehaviour
{
    public string thisRaidSaveName;

    public static RaidSaving Instance;

    public GameObject generic;

    public GameObject genericPrefab;
    public GameObject katolskPrefab;
    public GameObject ortodoksPrefab;
    public GameObject moskePrefab;
    public GameObject islamHousePrefab;
    public GameObject yorkHouse;
    public WhatToDoOnStart wtdos;

    public RaidStructureSpawner raidStructureSpawner; 
    private void Awake()
    {
        Instance = this;

    }


    private void Start()
    {
        if (wtdos.loadCurrentSave)
        {
            FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/" + thisRaidSaveName + ".save");
            if (info.Exists)
            {
                
                LoadRaid();
            }
            else
            {
                Debug.Log("Loaded fresh raid because raid file dont exist");
                LoadFreshRaid();
            }
        }
        if (wtdos.loadNewGameSave)
        {
            Debug.Log("Loaded fresh raid");
            LoadFreshRaid();
        }
        
    }
    public void SaveRaid(bool completed)
    {
        
        RaidSave r = new RaidSave();

        r.raidComleted = completed;
        //save structures
        List<RaidStructureData> d = new List<RaidStructureData>();
        for (int i = 0; i < RaidManager.Instance.raidTasks.Count; i++)
        {
            d.Add(RaidManager.Instance.raidTasks[i].myData);
        }
        r.structures = d;
        Debug.Log(d.Count.ToString());
        SerializationManager.Save(thisRaidSaveName, r);

        Debug.Log("Saved raid file as " + completed.ToString());
    }
    public void LoadRaid()
    {
        RaidManager.Instance.addBuildingGraphicForeachGetRaid = false;
        Debug.Log("Loaded raid");
        //Destroy all structures
        RaidTask[] rts = FindObjectsOfType(typeof(RaidTask)) as RaidTask[];
        Debug.Log(rts.Length.ToString());
        foreach(RaidTask rp in rts)
        {
            rp.DestroyThis();
        }
        /*
        Debug.Log(RaidManager.Instance.raidTasks.Count);
        RaidTask[] qs = new RaidTask[RaidManager.Instance.raidTasks.Count];
        for (int i = 0; i < qs.Length; i++)
        {
            qs[i] = RaidManager.Instance.raidTasks[i];
        }
        Debug.Log(qs.Length.ToString());
        for (int i = 0; i < qs.Length; i++)
        {
            
            qs[i].DestroyThis();
        }
        */
        FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/" + thisRaidSaveName + ".save");
        if (!info.Exists)
        {
            Debug.Log("File dont exist");
        }
        RaidSave r = (RaidSave)SerializationManager.Load(Application.persistentDataPath + "/saves/" + thisRaidSaveName + ".save");

        for (int i = 0; i < r.structures.Count; i++)
        {
            RaidStructureData data = r.structures[i];
            GameObject obj = Instantiate(StructurePrefab(data.type));

            RaidTask rt = obj.GetComponent<RaidTask>();
            rt.myData = data;

            obj.transform.position = data.position;
            obj.transform.rotation = data.rotation;

            rt.myType = data.type;
            rt.dead = data.dead;

            rt.health = data.health;
            raidStructureSpawner.AddRaidTaskGraphic(rt);
            Debug.Log("Spawned raid taks");
        }
    }

    public GameObject StructurePrefab(RaidStructureType t)
    {
        if(t == RaidStructureType.generic)
        {
            return genericPrefab;
        }

        if(t == RaidStructureType.katolsk)
        {
            return katolskPrefab;
        }

        if(t == RaidStructureType.moske)
        {
            return moskePrefab;
        }

        if(t == RaidStructureType.ortodoks)
        {
            return ortodoksPrefab;
        }
        if(t == RaidStructureType.islamHouse)
        {
            return islamHousePrefab;
        }
        if(t == RaidStructureType.yorkHouse)
        {
            return yorkHouse;
        }
        return generic;
    }

    public void LoadFreshRaid()
    {
        Debug.Log("Started fresh raid");
    }
    
}


[System.Serializable]
public class RaidSave
{
    public bool raidComleted;

    public List<RaidStructureData> structures = new List<RaidStructureData>();
}

[System.Serializable]
public class RaidStructureData
{
    public Vector3 position;
    public Quaternion rotation;

    public float health;


    public bool dead;

    public RaidStructureType type;
}

[System.Serializable]
public enum RaidStructureType
{
    generic, katolsk, ortodoks, moske, islamHouse, yorkHouse
}
