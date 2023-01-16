using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Shipyard : MonoBehaviour
{
    private StructureManager structureManager;

    //Levels etc
    [System.Serializable]
    public class ShipyardLevel
    {
        public string levelName = "Level";
        [Space]
        public float goldUpgradeCost;
        public float woodUpgradeCost;
        public float metalUpgradeCost;
        [Space]
        public Ship.ShipLevels shipLevelToCreate;
        public float shipSeats;
        [Space]
        public float goldShipProduceCost;
        public float woodShipProduceCost;
        public float metalShipProduceCost;
        [Space]
        public float baseProductionTime = 10f;
        public float perWorkerMultiplier = 0.9f;
    }
    public ShipyardLevel[] shipyardLevels;
    int levelIndex = 0;
    [HideInInspector]
    public ShipyardLevel currentShipyardLevel;

    public FloatVariable gold;
    public FloatVariable metal;
    public FloatVariable wood;

    public bool freeUpgrading;
    public bool freeProduction;

    public Color canAffordColor;
    public Color cantAffordColor;

    public List<Worker> workersAtShipyard = new List<Worker>();

    public bool canBeUpgraded = true;

    public Transform[] standPoints;
    int standPointIndex;

    public FloatVariable ships;
    bool producing;
    float t;
    public float baseProductionTime;

    ProgressBar bar;
    Structure structure;

    public bool ignoreUI;

    [Header("Audio")]
    public AudioClip buildingSounds;
    public AudioClip completedShipClip;
    private AudioSource source;
    void Start()
    {
        structureManager = StructureManager.Instance;

        structureManager.GetShipyard(this);
        structure = GetComponent<Structure>();
        source = GetComponent<AudioSource>();
        SetLevel();
    }

    void SetLevel()
    {
        //Check for structureData level
        
        
        if(structure.myData.level == StructureLevel.one)
        {
            levelIndex = 0;
        }
        if (structure.myData.level == StructureLevel.two)
        {
            levelIndex = 1;
        }
        if (structure.myData.level == StructureLevel.three)
        {
            levelIndex = 2;
        }
        //Set level
        currentShipyardLevel = shipyardLevels[levelIndex];
    }

    void UpdateDataLevel()
    {

        if(levelIndex == 0)
        {
            structure.myData.level = StructureLevel.one;
        }
        if (levelIndex == 1)
        {
            structure.myData.level = StructureLevel.two;
        }
        if (levelIndex == 2)
        {
            structure.myData.level = StructureLevel.three;
        }
    }

    private void Update()
    {
        if(workersAtShipyard.Count != 0)
        {
            if (producing && !ignoreUI)
            {
                if (t < ProductionTime())
                {
                    if (bar == null)
                    {
                        bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.4f);
                    }

                    bar.bar.fillAmount = t / ProductionTime();
                    t += Time.deltaTime;
                }
                else
                {
                    t = 0;
                    ProduceShip();
                }
            }
            if (ignoreUI)
            {
                if (t < ProductionTime())
                {
                    if (bar == null)
                    {
                        bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.4f);
                    }

                    bar.bar.fillAmount = t / ProductionTime();
                    t += Time.deltaTime;
                }
                else
                {
                    t = 0;
                    ProduceShip();
                }
            }

        }

        if (producing && buildingSounds != null)
        {
            if (!source.isPlaying)
            {
                source.clip = buildingSounds;
                source.Play();
            }
        }
        else
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        
    }

    

    public void UpgradeShipyard()
    {
        levelIndex += 1;

        if(levelIndex >= shipyardLevels.Length)
        {
            levelIndex = shipyardLevels.Length - 1;
        }
        

        currentShipyardLevel = shipyardLevels[levelIndex];
        UpdateDataLevel();
    }

    void ProduceShip()
    {
        ShipManager.Instance.NewShip(currentShipyardLevel.shipLevelToCreate, currentShipyardLevel.shipSeats);
        producing = false;
        Destroy(bar.gameObject);
        NotificationManager.Instance.OpenNotification("Ship finished!", "Congratulations, your builders finsihed a brand new ship!", 7f, null);
    }

    public void StartProducing()
    {
        if (ShipManager.Instance.CanBuildMoreShips())
        {
            if (producing == false)
            {
                producing = true;
            }
        }
        else
        {
            NotificationManager.Instance.OpenNotification("No ship!", "Theres no room for more ships, expand your docks to build more ships!", 5f, null);
        }
        
    }


    
    float ProductionTime()
    {
        float amount = workersAtShipyard.Count;

        float time = currentShipyardLevel.baseProductionTime;

        for (int i = 0; i < amount; i++)
        {
            time *= currentShipyardLevel.perWorkerMultiplier;
        }

        return time;
    }
    public Vector3 GetStandPoint(Vector3 workerPos)
    {
        Vector3 pos = standPoints[standPointIndex].position;
        standPointIndex += 1;
        if(standPointIndex >= standPoints.Length)
        {
            standPointIndex = 0;
        }
        return pos;

    }
    
    public void GetWorker(Worker worker)
    {
        if (!workersAtShipyard.Contains(worker))
        {
            workersAtShipyard.Add(worker);
        }
        
    }

   

    public void RemoveWorker(Worker worker)
    {
        if (workersAtShipyard.Contains(worker))
        {
                    
            workersAtShipyard.Remove(worker);
        }
    }
    public void FinishedBuilding()
    {
        for (int i = 0; i < workersAtShipyard.Count; i++)
        {
            workersAtShipyard[i].StopBuildingShip();
        }
    }
}
