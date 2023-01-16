using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceSaver : MonoBehaviour
{
    public static ResourceSaver Instance;

    public RaidReward raidReward;

    public float newGameGold = 5;
    public float newGameWood = 50;
    public float newGameMetal = 10;
    public float newGameMyrmalm = 5;
    public float newGameFoodCount = 15f;
    private void Awake()
    {
        Instance = this;
    }

    public FloatVariable gold;
    public FloatVariable wood;
    public FloatVariable food;
    public FloatVariable myrmalm;
    public FloatVariable metal;
    public FloatVariable foodCountV;
    public void SaveResources()
    {
        Resources r = new Resources
        {
            gold = gold.value,
            wood = wood.value,
            foodCount = foodCountV.value,
            myrmalm = myrmalm.value,
            metal = metal.value
        };
        SerializationManager.Save("resources", r);

    }

    private void Update()
    {
        wood.capacity = WoodCapacity();
        myrmalm.capacity = MyrmalmCapacity();
    }

    public float WoodCapacity()
    {
        float t = 0;

        for (int i = 0; i < StructureManager.Instance.woodStorages.Count; i++)
        {
            t += StructureManager.Instance.woodStorages[i].WoodCapacityGiven();
        }

        return t;
    }

    public float MyrmalmCapacity()
    {
        float t = 0;

        for (int i = 0; i < StructureManager.Instance.myrmalmStorages.Count; i++)
        {
            t += StructureManager.Instance.myrmalmStorages[i].MyrmalmCapacityGiven();
        }

        return t;
    }
    public void LoadResources()
    {
        FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/resources.save");
        if (!info.Exists)
        {
            ResetToNewGame();
            return;
        }

        Resources r = (Resources)SerializationManager.Load(Application.persistentDataPath + "/saves/resources.save");

        gold.value = r.gold;
        wood.value = r.wood;
        food.value = r.food;
        foodCountV.value = r.foodCount;
        metal.value = r.metal;
        myrmalm.value = r.myrmalm;

        
    }
    public void ResetToNewGame()
    {
        gold.value = newGameGold;
        wood.value = newGameWood;
        foodCountV.value = newGameFoodCount;
        metal.value = newGameMetal;
        myrmalm.value = newGameMyrmalm;
    }
}

[System.Serializable]
public class Resources
{
    public float gold;
    public float wood;
    public float food;
    public float foodCount;
    public float myrmalm;
    public float metal;

}

