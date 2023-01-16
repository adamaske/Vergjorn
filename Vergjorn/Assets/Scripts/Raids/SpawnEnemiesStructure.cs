using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SpawnEnemiesStructure : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int spIndex;

    public int amountOfEnemiesToSpawn;

    public GameObject enemyPrefab;

    public float enemyLevel;

    public bool hasSpawned;

    
    private void Start()
    {
        hasSpawned = false;

    }
    

    public void SpawnEnemies()
    {
        if (!hasSpawned)
        {
            for (int i = 0; i < amountOfEnemiesToSpawn; i++)
            {
                GameObject go = Instantiate(enemyPrefab, SpawnPoint(), transform.rotation, this.transform);
                EnemyUnit unit = go.GetComponent<EnemyUnit>();
                unit.inBattle = true;
            }

            hasSpawned = true;
        }
        
    }
    Vector3 SpawnPoint()
    {
        Vector3 pos = spawnPoints[spIndex].position;
        spIndex += 1;
        if(spIndex >= spawnPoints.Length)
        {
            spIndex = 0;
        }

        return pos;
    }
}
