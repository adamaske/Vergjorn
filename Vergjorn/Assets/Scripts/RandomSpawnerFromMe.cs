using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnerFromMe : MonoBehaviour
{
    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    public float minZ;
    public float maxZ;

    public float minYrot;
    public float maxYrot;

    public GameObject[] treePrefabs;

    public float numberOfTrees;

    public float minXscale;
    public float maxXscale;

    public float minYscale;
    public float maxYscale;

    public float minZscale;
    public float maxZscale;
    private void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        for(int i = 0; i < numberOfTrees; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            float z = Random.Range(minZ, maxZ);

            Vector3 pos = new Vector3(x, y, z);

            pos += transform.position;

            float yr = Random.Range(minYrot, maxYrot);
            

            int b = Random.Range(0, treePrefabs.Length);
            GameObject go = Instantiate(treePrefabs[b], pos, transform.rotation, this.transform);
            Quaternion rot = go.transform.rotation;
            rot.y = yr;
            go.transform.rotation = rot;

            Vector3 scale = new Vector3(Random.Range(minXscale, maxXscale), Random.Range(minYscale, maxYscale), Random.Range(minZscale, maxZscale));
            go.transform.localScale = scale;

        }
    }
}
