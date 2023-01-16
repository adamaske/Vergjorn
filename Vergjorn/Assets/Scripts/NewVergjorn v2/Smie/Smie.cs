using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Smie : MonoBehaviour
{
    public List<Worker> workersOnMe = new List<Worker>();
    
    [System.Serializable]
    public class SmieLevel
    {
        public float perWorkerTimeMultiplier = 0.9f;

        public float baseProductionTime = 30f;

        [Header("Sword costs")]
        public float swordGold;
        public float swordMetal;
        [Header("Helmet costs")]
        public float helmetGold;
        public float helmetMetal;
        [Header("Shield costs")]
        public float shieldGold;
        public float shieldMetal;
        [Space]
        public float goldUpgradeCost;
        public float metalUpgradeCost;
        [Space]
        public bool canProduceSwords;
        public bool canProduceShield;
        public bool canProduceHelmets;


    }

    public SmieLevel[] smieLevels;
    [HideInInspector]
    public SmieLevel currentSmieLevel;
    int levelIndex;
    [Space]
    public bool producing;
    public float t;
    [Space]
    public List<Weapon> weaponQueue = new List<Weapon>();
    public float maxQueueLength = 10f;
    public Weapon currentWeaponBeingMade;

    public FloatVariable gold;
    public FloatVariable metal;

    public GameObject armory;
    BuildingStructures bs;

    Structure structure;
    //standpoints
    [Space]
    public Transform[] standPoints;
    int standPointIndex;

    [Space]
    public AudioSource source;
    public AudioClip smieSounds;

    private void Start()
    {
        currentSmieLevel = smieLevels[levelIndex];

        structure = GetComponent<Structure>();
        bs = GetComponent<BuildingStructures>();
        SetLevel();

        source = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if(workersOnMe.Count > 0)
        {
            producing = true;
        }
        else
        {
            producing = false;
        }
        if(weaponQueue.Count != 0)
        {
            if (producing)
            {
                if (currentWeaponBeingMade == null)
                {
                    currentWeaponBeingMade = weaponQueue[0];
                }
                if (t < ProductionTime())
                {
                    t += Time.deltaTime;

                }
                else
                {
                    ProduceCurrentWeapon();
                    t = 0;
                }
                if (!source.isPlaying)
                {
                    source.clip = smieSounds;
                    source.Play();
                    source.loop = true;
                }
            }
            else
            {
                t = 0;
            }

        }

        if (!producing && source.isPlaying)
        {
            
                source.Stop();
            
        }
        
        if(!bs.unbuilt && !armory.activeSelf)
        {
            armory.SetActive(true);
        }
        else
        {
            armory.SetActive(false);
        }
    }


    void SetLevel()
    {
        //Check for structureData level


        if (structure.myData.level == StructureLevel.one)
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
        currentSmieLevel = smieLevels[levelIndex];
    }

    void UpdateLevel()
    {
    
        if (levelIndex == 0)
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
    public void ProduceCurrentWeapon()
    {
        WeaponsManager.Instance.GetWeapon(currentWeaponBeingMade.myType);

        //set next weapon
        weaponQueue.Remove(currentWeaponBeingMade);
        if(weaponQueue.Count != 0)
        {
            currentWeaponBeingMade = weaponQueue[0];
        }
        
    }


    public void ProduceWeapon(WeaponType t)
    {
        if(weaponQueue.Count < maxQueueLength)
        {
            Weapon w = new Weapon();

            w.myType = t;

            weaponQueue.Add(w);
        }
        
    }
  

    public float ProductionTime()
    {
        float amount = workersOnMe.Count;

        float time = currentSmieLevel.baseProductionTime;

        for (int i = 0; i < amount; i++)
        {
            time *= currentSmieLevel.perWorkerTimeMultiplier;
        }

        return time;
    }

    public Vector3 GetSmieStandPoint()
    {
        Vector3 pos = standPoints[standPointIndex].position;
        standPointIndex += 1;

        if(standPointIndex >= standPoints.Length)
        {
            standPointIndex = 0;
        }


        return pos;
    }
     
    #region Get n remove WOrkers
    public void GetWorker(Worker w)
    {
        if (!workersOnMe.Contains(w))
        {
            workersOnMe.Add(w);
        }
        
    }

    public void RemoveWorker(Worker w)
    {
        if (workersOnMe.Contains(w))
        {
            workersOnMe.Remove(w);
        }
    }

    #endregion

    public void UpgradeSmie()
    {
        if (!CanAffordUpgrade())
        {
            NotificationManager.Instance.OpenNotification("No weapons for you", "You dont have enough resources to that", 5f, null);
            return;
        }
        levelIndex += 1;

        if(levelIndex >= smieLevels.Length)
        {
            levelIndex = smieLevels.Length - 1;
        }

        currentSmieLevel = smieLevels[levelIndex];
        UpdateLevel();
    }
   
    bool CanAffordUpgrade()
    {
        if((gold.value - currentSmieLevel.goldUpgradeCost) < 0)
        {
            return false;
        }
        if((metal.value - currentSmieLevel.metalUpgradeCost) < 0)
        {
            return false;
        }

        return false;
    }

    public bool CanAffordWeaponGoldProduction(WeaponType t)
    {
        if(t == WeaponType.shield)
        {
            if((gold.value - currentSmieLevel.shieldGold) < 0)
            {
                return false;
            }
        }
        if (t == WeaponType.sword)
        {
            if ((gold.value - currentSmieLevel.swordGold) < 0)
            {
                return false;
            }
        }
        if (t == WeaponType.helmet)
        {
            if ((gold.value - currentSmieLevel.helmetGold) < 0)
            {
                return false;
            }
        }
        return true;
    }
    public bool CanAffordWeaponMetalProduction(WeaponType t)
    {
        if (t == WeaponType.shield)
        {
            if ((metal.value - currentSmieLevel.shieldMetal) < 0)
            {
                return false;
            }
        }
        if (t == WeaponType.sword)
        {
            if ((metal.value - currentSmieLevel.swordMetal) < 0)
            {
                return false;
            }
        }
        if (t == WeaponType.helmet)
        {
            if ((metal.value - currentSmieLevel.helmetMetal) < 0)
            {
                return false;
            }
        }
        return true;
        

    }
    public void PurchaseWeaponProduction(WeaponType t)
    {
        if(t == WeaponType.sword)
        {
            gold.value -= currentSmieLevel.swordGold;
            metal.value -= currentSmieLevel.swordMetal;
        }
        if (t == WeaponType.shield)
        {
            gold.value -= currentSmieLevel.shieldGold;
            metal.value -= currentSmieLevel.shieldMetal;
        }
        if (t == WeaponType.helmet)
        {
            gold.value -= currentSmieLevel.helmetGold;
            metal.value -= currentSmieLevel.helmetMetal;
        }
    }
}

