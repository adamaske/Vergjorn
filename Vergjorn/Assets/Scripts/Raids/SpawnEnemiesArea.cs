using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesArea : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int spIndex;

    public int amountOfEnemiesToSpawn;

    public GameObject enemyPrefab;

    public float enemyLevel;

    public bool hasSpawned;

    public float buildingCheckRadius = 10;

    public bool spawnIndepentdentOfBuilding;
    private void Start()
    {
        
    }

    public bool ShouldSpawn()
    {
        if (spawnIndepentdentOfBuilding)
        {
            return true;
        }
        Collider[] colls = Physics.OverlapSphere(transform.position, buildingCheckRadius);

        foreach(Collider col in colls)
        {
            RaidTask t = col.GetComponent<RaidTask>();
            if(t != null)
            {
                if (t.dead)
                {
                    Debug.Log("Raid task detected is dead");
                    return false;

                }
                else
                {
                    Debug.Log("Raid task is alive");
                    return true;
                }
            }
            else
            {
                Debug.Log("Didnt find anybuildings");
            }
        }
        

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SomethingEntered");
        Worker unit1 = other.GetComponent<Worker>();

        if (unit1 != null)
        {
            Debug.Log("WorkerEntered");
            //SpawnEnemies();
        }
        if (!hasSpawned)
        {
            if (ShouldSpawn())
            {
                Worker unit = other.GetComponent<Worker>();

                if (unit != null)
                {
                    Debug.Log("WorkerEntered");
                    SpawnEnemies();
                }
            }
        }
        
       
    }
    public void SpawnEnemies()
    {
       for (int i = 0; i < amountOfEnemiesToSpawn; i++)
       {
           GameObject go = Instantiate(enemyPrefab, SpawnPoint(), transform.rotation, null);
           EnemyUnit unit = go.GetComponent<EnemyUnit>();
           unit.inBattle = true;
       }

       hasSpawned = true;
        

    }
    Vector3 SpawnPoint()
    {
        Vector3 pos = spawnPoints[spIndex].position;
        spIndex += 1;
        if (spIndex >= spawnPoints.Length)
        {
            spIndex = 0;
        }

        return pos;
    }
}
