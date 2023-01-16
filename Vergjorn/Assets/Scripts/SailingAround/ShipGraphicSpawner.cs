using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGraphicSpawner : MonoBehaviour
{
    public static ShipGraphicSpawner Instance;
    public GameObject shipPrefab;

    public Transform[] shipSpawnPoints;

    public int index;

    public List<VikingShip> ships = new List<VikingShip>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SpawnShips();
    }
    public void SpawnShips()
    {
        //Destroy all first
        VikingShip[] rts = FindObjectsOfType(typeof(VikingShip)) as VikingShip[];
        Debug.Log(rts.Length.ToString());
        foreach (VikingShip rp in rts)
        {
            Destroy(rp.gameObject);
        }


        for (int i = 0; i < ShipManager.Instance.allShips.Count; i++)
        {
            GameObject go = Instantiate(shipPrefab, SpawnPoint(), shipSpawnPoints[index].rotation, this.transform);

            ships.Add(go.GetComponent<VikingShip>());
        }
        index = 0;
    }

    Vector3 SpawnPoint()
    {
        Vector3 pos = shipSpawnPoints[index].position;

        index += 1;

        if(index >= shipSpawnPoints.Length)
        {
            index = 0;
        }

        return pos;
    }

}
