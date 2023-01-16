using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockPart : MonoBehaviour
{
    public List<Ship> myShips = new List<Ship>();

    public Transform[] shipSpawnPoints;

    public float maxShips; //should be the same as spawnPoints lenght

    public GameObject shipPrefab;

    public int currentIndex;

    private void Start()
    {
        
    }
    public void GetShip(Ship ship)
    {
        myShips.Add(ship);
    }

    void SpawnShips()
    {
        for (int i = 0; i < myShips.Count; i++)
        {
            Transform parent = shipSpawnPoints[i];
            GameObject go = Instantiate(shipPrefab, parent.position, parent.rotation, parent);
            ShipGraphic s = go.GetComponent<ShipGraphic>();
            s.GetShip(myShips[i]);
        }
        
    }
}
