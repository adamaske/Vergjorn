using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public float maxTreesAtOnce;
    public float checkInterval;

    float t;

    TreeManager treeManager;
    float allowedToSpawn;

    public List<GameObject> treePrefabs;
    public Transform spawnPointParent;
    public List<Transform> spawnPoints;
    private void Start()
    {
        treeManager = TreeManager.Instance;
        foreach (Transform child in spawnPointParent)
        {
            spawnPoints.Add(child);
        }
    }

    private void Update()
    {
        if(t < checkInterval)
        {
            t += Time.deltaTime;
        }
        else
        {
            t = 0;
            CheckTreeCount();
        }
    }
    void SpawnAllowed()
    {
        for (int i = 0; i < allowedToSpawn; i++)
        {
            GameObject go = treePrefabs[Random.Range(0, treePrefabs.Count)];
            Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Count)].position;

            Instantiate(go, pos, transform.rotation, this.transform);
        }
    }



    public void CheckTreeCount()
    {
        float currentTrees = treeManager.treesInGame.Count;

        float allowedToSpawnTrees = maxTreesAtOnce - currentTrees;

        if(allowedToSpawnTrees > 0)
        {
            allowedToSpawn = allowedToSpawnTrees;
            SpawnAllowed();
        }
    }
}
