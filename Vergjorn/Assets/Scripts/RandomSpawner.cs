using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public float numberOfTrees;
    public float numberOfRocks;

    public GameObject[] treesPrefabs;
    public GameObject[] rocksPrefabs;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    private void Start()
    {

        for (int i = 0; i < numberOfRocks; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            float z = Random.Range(minZ, maxZ);

            Vector3 point = new Vector3(x, y, z);
            point += transform.position;
            int index = Random.Range(0, rocksPrefabs.Length);
            Instantiate(rocksPrefabs[index], point, transform.rotation, this.transform);
        }
        for (int i = 0; i < numberOfTrees; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            float z = Random.Range(minZ, maxZ);

            Vector3 point = new Vector3(x, y, z);
            point += transform.position;
            int index = Random.Range(0, treesPrefabs.Length);
            Instantiate(treesPrefabs[index], point, transform.rotation, this.transform);
        }


    }
}
