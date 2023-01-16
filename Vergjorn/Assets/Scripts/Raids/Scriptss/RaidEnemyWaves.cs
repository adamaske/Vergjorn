using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidEnemyWaves : MonoBehaviour
{
    public GameObject enemyPrefab;
   [System.Serializable]
    public class Wave
    {
        public Transform[] thisWaveSpawnPoints;
        public float amountOfEnemies;
        public float timeForStart = 10;

        public float t = 0;
    }

    public Wave[] waves;
    public int waveIndex;

    public bool isSpawning;

    private void Start()
    {
        isSpawning = true;
    }
    private void Update()
    {
        if (isSpawning)
        {
            if (waves[waveIndex].t < waves[waveIndex].timeForStart)
            {
                waves[waveIndex].t += Time.deltaTime;
            }
            else
            {
                SpawnWave();
            }
        }
        
    }


    void SpawnWave()
    {
        for (int i = 0; i < waves[waveIndex].amountOfEnemies; i++)
        {
            GameObject go = Instantiate(enemyPrefab, waves[waveIndex].thisWaveSpawnPoints[i].position, transform.rotation, this.transform);
            EnemyUnit unit = go.GetComponent<EnemyUnit>();
            unit.inBattle = true;
        }

        waveIndex += 1;
        if(waveIndex == waves.Length)
        {
            isSpawning = false;
        }
    }
}
