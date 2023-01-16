using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;

    public List<Structure> allStructures = new List<Structure>();

    public List<WoodStorage> woodStorages = new List<WoodStorage>();

    public List<Shipyard> shipyards = new List<Shipyard>();

    public List<FoodStorage> foodStorages = new List<FoodStorage>();

    public List<MyrmalmStorage> myrmalmStorages = new List<MyrmalmStorage>();

    public List<TrainingGrounds> trainingGrounds = new List<TrainingGrounds>();

    public List<Myrovn> myrovns = new List<Myrovn>();

    public List<BuildingStructures> unbuiltStructures = new List<BuildingStructures>();

    public List<Forest> forests = new List<Forest>();

    public List<Myr> myrs = new List<Myr>();
    void Awake()
    {
        Instance = this;
    }

    #region Wood
    public void GetWoodStorage(WoodStorage woodStorage)
    {
        woodStorages.Add(woodStorage);
    }

    public WoodStorage GetCloseWoodStorage(Vector3 playerPos)
    {
        if(woodStorages.Count == 0)
        {
            Debug.Log("Returned null");
            return null;
        }
        WoodStorage m = woodStorages[0];
        float dist = Vector3.Distance(playerPos, m.transform.position);

        for (int i = 0; i < woodStorages.Count; i++)
        {
            float d = Vector3.Distance(playerPos, woodStorages[i].transform.position);
            if (d < dist)
            {
                dist = d;
                m = woodStorages[i];
            }
        }

        return m;
    }

    public void GetForest(Forest forest)
    {
        if (!forests.Contains(forest))
        {
            forests.Add(forest);
        }
    }

    public Forest GetClosestForest(Vector3 pos)
    {
        if(forests.Count == 0)
        {
            return null;
        }
        Forest f = forests[0];
        float dist = Vector3.Distance(pos, f.transform.position);

        for (int i = 0; i < forests.Count; i++)
        {
            float d = Vector3.Distance(forests[i].transform.position, pos);
            if(d < dist)
            {
                dist = d;
                f = forests[i];
            }
        }

        return f;
    }
    #endregion

    #region SHipyard
    public void GetShipyard(Shipyard shipyard)
    {
        shipyards.Add(shipyard);
    }

    public Shipyard GetCloseShipyard(Vector3 playerPos)
    {
       if(shipyards.Count == 0)
        {
            return null;
        }

        return shipyards[0];
    }

    
    #endregion

    #region Myrmalm & Myrovn
    public void GetMyrmalmStorage(MyrmalmStorage myrmalmStorage)
    {
        myrmalmStorages.Add(myrmalmStorage);
    }

    public MyrmalmStorage GetCloseMyrmalmStorage(Vector3 playerPos)
    {
        //do logic
        if(myrmalmStorages.Count == 0)
        {
            return null;
        }
        MyrmalmStorage m = myrmalmStorages[0];
        float dist = Vector3.Distance(playerPos, m.transform.position);

        for (int i = 0; i < myrmalmStorages.Count; i++)
        {
            float d = Vector3.Distance(playerPos, myrmalmStorages[i].transform.position);
            if(d < dist)
            {
                dist = d;
                m = myrmalmStorages[i];
            }
        }

        return m;
    }

    public void GetMyrovn(Myrovn myrovn)
    {
        myrovns.Add(myrovn);
    }
    #endregion

    #region Gathering
    public void GetFoodStorage(FoodStorage foodStorage)
    {
        foodStorages.Add(foodStorage);
    }
    public FoodStorage GetCloseFoodStorage(Vector3 playerPos)
    {
        //do logic
        if(foodStorages.Count == 0)
        {
            return null;
        }
        return foodStorages[0];
    }
    #endregion

    #region Training Grounds

    public void GetTraingingGrounds(TrainingGrounds trainingGround)
    {
        trainingGrounds.Add(trainingGround);
    }

    public TrainingGrounds GetCloseTrainingGrounds(Vector3 playerPos)
    {
        if(trainingGrounds.Count == 0)
        {
            return null;
        }

        return trainingGrounds[0];
    }
    #endregion


    #region Unbuilt structures
    public void GetUnbuilt(BuildingStructures building)
    {
        unbuiltStructures.Add(building);
    }
    public void RemoveUnbuilt(BuildingStructures building)
    {
        if (unbuiltStructures.Contains(building))
        {
            unbuiltStructures.Remove(building);
        }
    }

    public BuildingStructures GetCloseUnbuilt(Vector3 playerPos)
    {
        //do logic

        if(unbuiltStructures.Count == 0)
        {
            return null;
        }

        return unbuiltStructures[0];
    }

    #endregion

    public void GetMyr(Myr m)
    {
        if (!myrs.Contains(m))
        {
            myrs.Add(m);
        }
    }

    public Myr GetCloseMyr(Vector3 pos)
    {

        return myrs[0];
    }
    public void GetStructure(Structure s)
    {
        if (!allStructures.Contains(s))
        {
            allStructures.Add(s);
        }
    }

    public void RemoveStructure(Structure s)
    {
        if (allStructures.Contains(s))
        {
            allStructures.Remove(s);
        }
    }
}
