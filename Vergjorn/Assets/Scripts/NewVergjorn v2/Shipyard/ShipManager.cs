using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;
    
    public List<Ship> allShips = new List<Ship>();

    
    private void Awake()
    {
        Instance = this;
    }


    public float ShipCapacity()
    {

        return 10;
    }

    public bool CanBuildMoreShips()
    {
        if(allShips.Count < ShipCapacity())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void SaveShips()
    {
        ShipSave s = new ShipSave();

        s.ships = allShips;

        SerializationManager.Save("Ships", s);
    }
    
    public void LoadShips()
    {
        FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/Ships.save");
        if(!info.Exists)
        {
            Debug.Log("Ship file dont exists");
            allShips = new List<Ship>();
            return;
        }

        ShipSave s = (ShipSave)SerializationManager.Load(Application.persistentDataPath + "/saves/Ships.save");
        
        allShips = s.ships;
    }

    public void NewShip(Ship.ShipLevels shipLevel, float seats)
    {
        Ship s = new Ship();
        s.myLevel = shipLevel;
        s.seats = seats;
        allShips.Add(s);

        ShipGraphicSpawner.Instance.SpawnShips();

    }
}
[System.Serializable]
public class Ship
{
    public enum ShipLevels { one, two, three}
    public ShipLevels myLevel;

    public float seats;
}

[System.Serializable]
public class ShipSave
{
   

    public List<Ship> ships = new List<Ship>();
}



