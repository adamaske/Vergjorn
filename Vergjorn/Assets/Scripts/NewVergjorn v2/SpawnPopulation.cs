using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPopulation : MonoBehaviour
{
    private BuildingStructures bs;

    public GameObject workerPrefab;

    public bool shouldSpawn;
    public float amountToSpawn;

    public Transform spawnPointParent;
    public List<Transform> spawnPoints;

    bool spawned;
    private void Start()
    {
        bs = GetComponent<BuildingStructures>();
        foreach(Transform child in spawnPointParent)
        {
            spawnPoints.Add(child);
        }
        //check strucutredata if built on start, then cancel all this
        StructureData d = GetComponent<Structure>().myData;
        if(d.built == true)
        {
            shouldSpawn = false;
            Destroy(this);
        }
        else
        {
            shouldSpawn = true;
        }

        
    }

    private void Update()
    {
        if (!spawned && bs.unbuilt == false && shouldSpawn)
        {
            spawned = true;
            Spawn();
        }
    }

    void Spawn()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject go = Instantiate(workerPrefab, spawnPoints[i].position, transform.rotation, null);
            Worker worker = go.GetComponent<Worker>();

            worker.TurnOffNavMeshAgent();

            
            worker.TurnOnNavMeshAgent();
        }

        Destroy(this);
    }
}
