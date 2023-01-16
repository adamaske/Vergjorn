using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public GameObject worldMap;

    private void Start()
    {
        CloseWorldMap();
    }


    public void OpenWorldMap()
    {
        worldMap.SetActive(true);

    }

    public void CloseWorldMap()
    {
        worldMap.SetActive(false);
    }
}
