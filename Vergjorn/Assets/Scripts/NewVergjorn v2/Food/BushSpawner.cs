using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public Transform spawnPointParent;

    public GameObject bushPrefab;

    private void Start()
    {
        foreach(Transform child in spawnPointParent)
        {
            spawnPoints.Add(child);
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Instantiate(bushPrefab, spawnPoints[i].position, spawnPoints[i].rotation, this.transform);
        }
    }
}
