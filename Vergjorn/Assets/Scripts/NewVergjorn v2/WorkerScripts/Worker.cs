using UnityEngine;
using UnityEngine.AI;
public class Worker : MonoBehaviour
{
    public enum WorkerState
    {
        idle, logging, myrmalmGathering, building, shipBuilder, myrovn, smithing, training, makingBabies, attacking, fishing, armory, destroyingBuilding, gathering, farming, hunting
    }
    public WorkerState state;

    public bool forcedIdle;

    #region Audio
    [Header("Audio")]
    public bool isWalking;

    public AudioClip stepSound;
    public AudioSource audioSource;
    public float playStepSoundDistance;

    public float stepSoundRate;
    float stepSoundT;
    [Space]
    public float playWoodChopSoundRate;
    float playWoodChopSoundT;
    public AudioClip woodChopSound;
    public float woodChopSoundVariation;
    #endregion

    #region Sailing Vars

    [HideInInspector] public enum SailingState { walkingToBoat, jumpingToSeat, inBoat}
    [Space] [HideInInspector] public SailingState sailingState;

    [HideInInspector] public VikingShipSeat mySeat;
    [HideInInspector] public VikingShip myShip;
    #endregion

    #region Fishing vars
    public enum FishingState { walkingToFishingPoint, fishing}
    public FishingState fishingState;

    bool fishingHutHasMe;

    public FishingPoint fishingPoint;
    public FishingHut currentFishingHut;

    public GameObject fishingRodParent;

    [HideInInspector] public float fishingT;
    #endregion

    #region Armory vars
    public enum ArmoryState { walkingToArmory, gettingWeapons, walkingAwayFromArmory}
    public ArmoryState armoryState;
    public Armory currentArmory;
    [HideInInspector] public Vector3 armoryWalkPoint;
    [HideInInspector] public Vector3 armoryWalkBackPoint;
    #endregion

    #region Training Grounds vars
    public enum TrainingState { walkingToQueueSpot, inQueue, moveToTrainingSpot, training, walkingOutOfCamp}
    [Header("Training Grounds")]
    public TrainingState trainingState;
    public TrainingGrounds currentTrainingGrounds;
    [HideInInspector]public Vector3 currentQueueStandPoint;
    [HideInInspector] public Vector3 trainingStandPoint;
    [HideInInspector] public Vector3 outOfCampWalkPoint;

    [HideInInspector] public bool trainingGroundsKnowIAmTraining;
    #endregion

    #region Logging vars

    public enum LoggingState { walkingToTree, chopping, walkingToWoodStorage, lookingForWoodStorage, lookingForForest, lookingForTree, droppingOffWoodAtStorage, waitingForMoreWoodCapacity}
    [Header("Logging")]
    public LoggingState loggingState;

    [HideInInspector]public Forest currentForest;

    [HideInInspector] public Vector3 treeStandPoint;
    [HideInInspector]public bool treeHasMe = false;
    public Tree currentTree;

    float choppingT;
    public float currentWoodStored;
    [HideInInspector]public WoodStorage currentWoodStorage;
    [HideInInspector] public Vector3 woodStorageStandPoint;

    public float dropOffWoodTime;
    float storeWoodT;
    public FloatVariable wood;
    [Space]
    public Transform holdingLogsParent;
    public Transform axeForLoggingParent;
    
   
    #endregion

    #region Myrmalm
    public enum MyrmalmState { walkingToMyrmalm, chopping, walkingToMyrmalmStorage, lookingForMyrmalmStorage, lookingForMyr, lookingForMyrmalm, droppingOffMyrmalmAtStorage, waitingForMoreMyrmalmCapacity }
    [Header("Myrmalm")]
    public MyrmalmState myrmalmState;

    [HideInInspector] public Myr currentMyr;

    [HideInInspector] public Vector3 myrmalmStandPoint;
    [HideInInspector]public bool myrmalmHasMe = false;
    [HideInInspector]public Myrmalm currentMyrmalm;

    float miningT;
    public float currentMyrmalmStored;
    public MyrmalmStorage currentMyrmalmStorage;
    [HideInInspector] public Vector3 myrmalmStorageStandPoint;

    public float dropOffMyrmalmTime;
    float storeMyrmalmT;
    public FloatVariable myrmalm;
    [Space]
    public Transform myrmalmHoldingParent;
    public Transform pickaxeForMyrmalmParent;
    
    #endregion
    
    #region Myrovn
    public enum MyrovnState { walkingToMyrovn, workingAtMyrovn}
    [Space]
    public MyrovnState myrovnState;
    public Myrovn currentMyrovn;
    Vector3 myrovnStandPoint;
    [HideInInspector]public bool myrovnHasMe;

    #endregion

    #region Combat
    [Header("Combat")]
    public CombatLevel[] combatLevels;
    public float currentCombatXP;
    public CombatLevel currentCombatLevel;
    [HideInInspector]public int combatLevelIndex;



    public float health;
    public float maxHealth;
    public EnemyUnit currentEnemy;
    public bool attackingEnemy;
    public float lookAtEnemySpeed;
    float attackT;

    public bool isDead;
    #endregion

    #region Farming
    [Header("Farming")]
    public Farm currentFarm;

    Vector3 farmingStandPoint;

    public bool farming;

    public float currentFarmingSpeed;
    public float farmingT;

    bool farmHasMe;
    #endregion

    #region Hunting
    public enum HuntingState { walkingToHuntingArea, walkingToFoodStorage, hunting, droppingOffAtFoodStorage}
    [Header("Hunting")]
    public HuntingState huntingState;

    public HuntingArea currentHuntingArea;
    Vector3 currentHuntingStandPoint;

    bool hunting;
    float currentHuntingSwapSpeed;
    public float swapHuntingSpotSpeed;
    float huntingSwapT;

    public float currentHuntingSpeed;
    public HuntingLodge currentHuntingLodge;
    Vector3 currentHuntingLodgeStandPoint;

    float huntingT;
    #endregion

    #region Raiding
    [Header("Raiding")]
    public RaidTask currentRaidTask;
    public bool attackingRaidTask;
    float raidTaskT;
    Vector3 raidTaskStandPoint;

    public float storedGold;

    #endregion

    #region Equipment
    public GameObject swordPrefab;
    public Transform swordIdleParent;
    public GameObject shieldPrefab;
    public Transform shieldIdleParent;
    public GameObject helmetPrefab;
    public Transform helmetIdleParent;

    public Transform swordInCombatParent;
    public Transform shieldInCombatParent;
    
    GameObject swordGO;
    GameObject shieldGO;
    GameObject helmetGO;
    #endregion
   
    #region Building
    BuildingStructures currentBuilding;
    bool buildingBuilding;
    float buildT;
    [Header("Building")]
    public float buildSpeed;
    public float buildPer;

    bool buildingHasMe;
    [Space]
    Vector3 buildStandPoint;
    #endregion

    #region Ship Building
    Shipyard currentShipyard;
    bool buildingShip;
    float buildShipT;
    [Header("Ship building")]
    public float buildShipSpeed;
    public float buildShipPer;
    bool shipyardHasMe;
    [Space]
    Vector3 shipStandPoint;
    #endregion

    #region Smithing
    Smie currentSmie;
    bool isSmithing;
    bool smieHasMe;
    Vector3 smieStandPoint;

    #endregion
   
    #region Baby makin
    BabyMaking currentBabyMaker;
    [HideInInspector] public bool babyMakerHasMe;
    
    [HideInInspector] public Vector3 babyMakerStandPoint;
    [HideInInspector] public bool atBabyMakerStandPoint;
    #endregion

    #region WorkerLevel
    [System.Serializable]
    public class WorkerLevel
    {
        public string levelName;
        [Space]
        public float loggingSpeed;
        public float WoodPerTreeChopped;
        public float xpPerWoodDrop;
        [Space]
        public float buildScorePer;
        public float buildSpeed;
        [Space]
        public float myrmalmGatherSpeed;
        public float myrmalmPer;
        public float xpPerMyrmalmDrop;
        [Space]
        public float xpForNextLevel;
        [Space]
        public float gatheringSpeed;
        public float gatheringDamage;
        [Space]
        public float fishingSpeed;
        public float foodPerFishCatch;
        [Space]
        public float farmingSpeed;
        public float foodPerFarming;
        [Space]
        public float huntingSpeed;
        public float foodPerHuntReturn;
    }
    public WorkerLevel[] workerLevels;
    public WorkerLevel currentWorkerLevel;
    [HideInInspector] public int workerLevelIndex;
    public float currentXP;
    public AudioClip levelUpClip;
    #endregion

    #region Random Stuff
    public WorkerType workerType;

    public float rotateSpeed = 45;

    bool movingToDestination;

    [HideInInspector] public NavMeshAgent agent;

   
    Vector3 currDestination; 
    public WorkerData myData;
    [HideInInspector] public bool allWeaponsFromStart = true;

    ProgressBar bar;

    public GameObject selectedIndicator;
    #endregion

    #region Graphics
    private WorkerGraphic workerGraphic;
    #endregion

    #region Gathering Food
    public enum GatheringState { walkingToFood, gathering, lookingForFoodStorage, walkingToFoodStorage, lookingForFood, droppingOffAtFoodStorage}
    [Header("Gathering")]
    public GatheringState gatheringState;

    public GatherableFood currentFood;
    [HideInInspector] public Vector3 foodStandPoint;

    public GatheringArea currentGatheringArea;

    public float foodToDeposit;
    public FoodStorage currentFoodStorage;
    [HideInInspector] public Vector3 foodStorageStandPoint;
    float gatheringT;
    public float dropFoodTime;
    float dropFoodT;
    public FloatVariable foodCount;
    public BoolVariable useFoodCount;
    [Space]
    [HideInInspector] public bool foodHasMe;
    #endregion
    ProgressBar currentProgressBar;

    public Name myName;
    
    public bool onRaid;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
       
        
    }
    void Start()
    {
        selectedIndicator.SetActive(false);
        health = maxHealth;
        if (!onRaid)
        {
            PopulationManager.Instance.GetWorker(this);
        }

        workerGraphic = GetComponent<WorkerGraphic>();
        
        if (allWeaponsFromStart)
        {
            myData.hasShield = true;
            myData.hasSword = true;
            myData.hasHelmet = true;
        }
        fishingRodParent.SetActive(false);
        SetWorkerLevel();
        EquipWeapons();
       
        if(myName == null)
        {
            myName = NameGenerator.Instance.GetName();
        }
        if(string.IsNullOrEmpty(myName.nameString))
        {
            //Debug.Log("Got new name");
            myName = NameGenerator.Instance.GetName();
        }

       
        this.gameObject.name = myName.nameString;

        workerGraphic.SetGraphic();
    }


    void Update()
    {
        SetWorkerDataInfo();
        if (state == WorkerState.idle)
        {
            if (movingToDestination)
            {
                if (Vector3.Distance(transform.position, currDestination) < 0.25f)
                {
                    agent.ResetPath();
                }

            }
        }

       
        #region Audio
        if (agent.velocity != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
       
        //if(agent.velocity != Vector3.zero && Vector3.Distance(transform.position, Camera.main.transform.position) < playStepSoundDistance)
        //{
        //    isWalking = true;
        //    if(stepSoundT < stepSoundRate)
        //    {
        //        stepSoundT += Time.deltaTime;
        //    }
        //    else
        //    {
                
        //        stepSoundT = 0;
        //        audioSource.PlayOneShot(stepSound, 1f);
        //        Debug.Log("Played step sound");
        //    }
        //}
        //else
        //{
        //    stepSoundT = stepSoundRate / 2;
        //    isWalking = false;
        //}

        #endregion

        #region Logging

        if(state == WorkerState.logging && (loggingState == LoggingState.walkingToWoodStorage || loggingState == LoggingState.droppingOffWoodAtStorage))
        {
            if (!holdingLogsParent.gameObject.activeSelf)
            {
                holdingLogsParent.gameObject.SetActive(true);
            }
            
        }
        else
        {
            if (holdingLogsParent.gameObject.activeSelf)
            {
                holdingLogsParent.gameObject.SetActive(false);
            }
        }
        if (state == WorkerState.logging && (loggingState == LoggingState.chopping || loggingState == LoggingState.walkingToTree))
        {
            if (!axeForLoggingParent.gameObject.activeSelf)
            {
                axeForLoggingParent.gameObject.SetActive(true);
            }
        }
        else
        {
            if (axeForLoggingParent.gameObject.activeSelf)
            {
                axeForLoggingParent.gameObject.SetActive(false);
            }
        }

        if (state == WorkerState.logging)
        {
            LoggingFunction();
            

        }




        #endregion

        #region Myrmalm
        //Holding 
        if (state == WorkerState.myrmalmGathering && (myrmalmState == MyrmalmState.walkingToMyrmalmStorage || myrmalmState == MyrmalmState.droppingOffMyrmalmAtStorage))
        {
            if (!myrmalmHoldingParent.gameObject.activeSelf)
            {
                myrmalmHoldingParent.gameObject.SetActive(true);
            }

        }
        else
        {
            if (myrmalmHoldingParent.gameObject.activeSelf)
            {
                myrmalmHoldingParent.gameObject.SetActive(false);
            }
        }
        //Pickaxe
        if(state == WorkerState.myrmalmGathering && (myrmalmState == MyrmalmState.chopping || myrmalmState == MyrmalmState.walkingToMyrmalm))
        {
            if (!pickaxeForMyrmalmParent.gameObject.activeSelf)
            {
                pickaxeForMyrmalmParent.gameObject.SetActive(true);
            }
        }
        else
        {
            if (pickaxeForMyrmalmParent.gameObject.activeSelf)
            {
                pickaxeForMyrmalmParent.gameObject.SetActive(false);
            }
        }
        
        if (state == WorkerState.myrmalmGathering)
        {
            if ((myrmalm.value + currentMyrmalmStored) > myrmalm.capacity)
            {
                if (agent.remainingDistance != 0)
                {
                    agent.ResetPath();
                }
                myrmalmState = MyrmalmState.waitingForMoreMyrmalmCapacity;
            }


            if (myrmalmState == MyrmalmState.chopping && !myrmalmHasMe)
            {

                currentMyrmalm.GetWorker(this);
                myrmalmHasMe = true;
            }
            if (myrmalmState == MyrmalmState.walkingToMyrmalm && !myrmalmHasMe)
            {
                currentMyrmalm.GetWorker(this);
                myrmalmHasMe = true;
            }

            if (myrmalmState != MyrmalmState.walkingToMyrmalm && myrmalmState != MyrmalmState.chopping && myrmalmHasMe)
            {
                currentMyrmalm.RemoveWorker(this);
                myrmalmHasMe = false;
            }

            if (myrmalmState == MyrmalmState.lookingForMyr)
            {
                currentMyr = StructureManager.Instance.GetCloseMyr(transform.position);
                if (currentMyr != null)
                {
                    currentMyrmalm = currentMyr.GetClosestNotBusyMyrmalm(this);
                    if (currentMyrmalm == null)
                    {
                        myrmalmState = MyrmalmState.lookingForMyrmalm;
                    }
                    else
                    {
                        myrmalmStandPoint = currentMyrmalm.GetStandPoint();
                        myrmalmState = MyrmalmState.walkingToMyrmalm;
                    }


                }
            }
            if (myrmalmState == MyrmalmState.lookingForMyrmalm)
            {
                currentMyrmalm = currentMyr.GetClosestNotBusyMyrmalm(this);

                if (currentMyrmalm != null)
                {
                    myrmalmStandPoint = currentMyrmalm.GetStandPoint();
                    myrmalmState = MyrmalmState.walkingToMyrmalm;
                }
            }
            if (myrmalmState == MyrmalmState.walkingToMyrmalm)
            {
                if (currentMyrmalm == null)
                {
                    myrmalmState = MyrmalmState.lookingForMyrmalm;
                }

                if (agent.destination != myrmalmStandPoint)
                {
                    agent.SetDestination(myrmalmStandPoint);
                }
                if (!myrmalmHasMe)
                {
                    currentMyrmalm.GetWorker(this);
                    myrmalmHasMe = true;
                }
                if (Vector3.Distance(transform.position, myrmalmStandPoint) < 0.1f)
                {
                    agent.ResetPath();
                    myrmalmState = MyrmalmState.chopping;
                }
            }
            if (myrmalmState == MyrmalmState.chopping)
            {
                if (currentProgressBar == null)
                {
                    currentProgressBar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.3f);
                }
                currentProgressBar.bar.fillAmount = miningT / currentWorkerLevel.myrmalmGatherSpeed;
                RotateTowards(currentMyrmalm.transform.position, 180);


                //chopping animation state
                if (miningT < MyrmalmGatheringSpeed())
                {
                    //Play chopping sound Logic
                    miningT += Time.deltaTime;
                }
                else
                {
                    miningT = 0;
                    GatherMyrmalm();
                }

            }
            if (myrmalmState == MyrmalmState.lookingForMyrmalmStorage)
            {
                currentMyrmalmStorage = StructureManager.Instance.GetCloseMyrmalmStorage(transform.position);
                if (currentMyrmalmStorage != null)
                {
                    myrmalmState = MyrmalmState.walkingToMyrmalmStorage;
                    myrmalmStorageStandPoint = currentMyrmalmStorage.GetStandPoint(transform.position);
                }
                else
                {
                    Debug.Log("No Wood storage found");
                }
            }
            if (myrmalmState == MyrmalmState.walkingToMyrmalmStorage)
            {
                if (agent.destination != myrmalmStorageStandPoint)
                {
                    agent.SetDestination(myrmalmStorageStandPoint);
                }

                if (Vector3.Distance(transform.position, myrmalmStorageStandPoint) < 0.5f)
                {
                    myrmalmState = MyrmalmState.droppingOffMyrmalmAtStorage;
                    storeMyrmalmT = 0;
                    agent.ResetPath();
                }
            }
            if (myrmalmState == MyrmalmState.droppingOffMyrmalmAtStorage)
            {
                RotateTowards(currentMyrmalmStorage.transform.position, 0);


                if (storeMyrmalmT < dropOffMyrmalmTime)
                {
                    storeMyrmalmT += Time.deltaTime;
                }
                else
                {
                    StoreMyrmalmAtMyrmalmStorage();
                }

            }
            if (myrmalmState == MyrmalmState.waitingForMoreMyrmalmCapacity)
            {
                if ((myrmalm.value + MyrmalmPerGather()) <= myrmalm.capacity)
                {
                    if (currentMyrmalmStored == 0)
                    {
                        myrmalmState = MyrmalmState.lookingForMyr;
                    }
                    else
                    {
                        myrmalmState = MyrmalmState.walkingToMyrmalmStorage;
                    }

                }
            }

            if(myrmalmState != MyrmalmState.chopping && currentProgressBar != null)
            {
                Destroy(currentProgressBar.gameObject);
                currentProgressBar = null;
            }

        }
        #endregion

        #region Myrovn
        if (state == WorkerState.myrovn)
        {
            if (currentMyrovn == null)
            {
                GoToIdle();
            }


            if (myrovnState == MyrovnState.walkingToMyrovn)
            {
                if (myrovnHasMe)
                {
                    currentMyrovn.RemoveWorker(this);
                    myrovnHasMe = false;
                }
                if (agent.destination != myrovnStandPoint)
                {
                    agent.SetDestination(myrovnStandPoint);
                }

                if (Vector3.Distance(transform.position, myrovnStandPoint) < 0.5f)
                {
                    agent.ResetPath();
                    myrovnState = MyrovnState.workingAtMyrovn;
                }
            }

            if (myrovnState == MyrovnState.workingAtMyrovn)
            {
                if (!myrovnHasMe)
                {
                    currentMyrovn.GetWorker(this);
                    myrovnHasMe = true;
                }

                Quaternion needRotation = Quaternion.LookRotation(currentMyrovn.transform.position - transform.position);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, needRotation, rotateSpeed * Time.deltaTime);
            }

        }

        if (state != WorkerState.myrovn && myrovnHasMe)
        {
            currentMyrovn.RemoveWorker(this);
            myrovnHasMe = false;
        }
        #endregion

        #region Training Grounds
        if (state == WorkerState.training)
        {
            if (trainingState == TrainingState.walkingToQueueSpot)
            {
                if (agent.destination != currentQueueStandPoint)
                {
                    agent.SetDestination(currentQueueStandPoint);
                }

                if (Vector3.Distance(transform.position, currentQueueStandPoint) < 1f)
                {
                    agent.ResetPath();
                    trainingState = TrainingState.inQueue;
                }
            }

            if (trainingState == TrainingState.moveToTrainingSpot)
            {
                if (agent.destination != trainingStandPoint)
                {
                    agent.SetDestination(trainingStandPoint);
                }

                if (Vector3.Distance(transform.position, trainingStandPoint) < 0.5f)
                {
                    agent.ResetPath();
                    trainingState = TrainingState.training;
                }


            }
            if (trainingState == TrainingState.training)
            {
                if (agent.remainingDistance != 0)
                {
                    agent.ResetPath();
                }


            }

            if (trainingState == TrainingState.walkingOutOfCamp)
            {
                if (agent.destination != outOfCampWalkPoint)
                {
                    agent.SetDestination(outOfCampWalkPoint);
                }
                if (Vector3.Distance(transform.position, outOfCampWalkPoint) < 0.5f)
                {
                    agent.ResetPath();
                    GoToIdle();
                }
            }
        }

        #endregion

        #region Weapon Equipment in attackMode
        if (state == WorkerState.attacking)
        {
            if (swordGO != null)
            {
                if (swordGO.transform.parent != swordInCombatParent)
                {
                    swordGO.transform.SetParent(swordInCombatParent);
                    swordGO.transform.position = swordInCombatParent.position;
                    swordGO.transform.localEulerAngles = Vector3.zero;
                }
            }

            if (shieldGO != null)
            {
                if (shieldGO.transform.parent != shieldInCombatParent)
                {
                    shieldGO.transform.SetParent(shieldInCombatParent);
                    shieldGO.transform.position = shieldInCombatParent.position;
                    shieldGO.transform.localEulerAngles = Vector3.zero;
                }

            }

            if (health != maxHealth)
            {
                if (bar == null)
                {
                    bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.3f);
                }

                bar.bar.fillAmount = health / maxHealth;
            }
        }
        else
        {
            if (bar != null)
            {
                Destroy(bar.gameObject);
                bar = null;
            }
            if (swordGO != null)
            {
                if (swordGO.transform.parent != swordIdleParent)
                {
                    swordGO.transform.SetParent(swordIdleParent);
                    swordGO.transform.position = swordIdleParent.position;
                    swordGO.transform.localEulerAngles = Vector3.zero;
                }
            }
            if (shieldGO != null)
            {
                if (shieldGO.transform.parent != shieldIdleParent)
                {
                    shieldGO.transform.SetParent(shieldIdleParent);
                    shieldGO.transform.position = shieldIdleParent.position;
                    shieldGO.transform.localEulerAngles = Vector3.zero;
                }
            }
        }
        #endregion

        #region CityBuilding      

        
        if (buildingBuilding && state == WorkerState.building)
        {
            if (!buildingHasMe)
            {
                currentBuilding.GetWorker(this);
                buildingHasMe = true;
            }
        }
        else
        {
            if (buildingHasMe)
            {
                currentBuilding.RemoveWorker(this);
                buildingHasMe = false;
                GoToIdle();
            }
        }
        if (state == WorkerState.building)
        {
            if (currentBuilding != null)
            {

                if (Vector3.Distance(transform.position, buildStandPoint) < 1)
                {
                    buildingBuilding = true;
                    if (agent.remainingDistance != 0)
                    {
                        agent.ResetPath();
                    }
                }
                else
                {
                    if (agent.destination != buildStandPoint)
                    {
                        agent.SetDestination(buildStandPoint);
                    }
                    buildingBuilding = false;
                }

                if (buildingBuilding)
                {
                    
                    RotateTowards(currentBuilding.transform.position, 180);

                }

                if(currentBuilding.unbuilt == false)
                {
                    currentBuilding.RemoveWorker(this);
                    buildingHasMe = false;
                    GoToIdle();
                } 
            }
            else
            {


            }

            
        }

        if (state == WorkerState.shipBuilder)
        {
            if (currentShipyard != null)
            {
                if (Vector3.Distance(transform.position, shipStandPoint) < 1)
                {
                    if (!shipyardHasMe)
                    {
                        currentShipyard.GetWorker(this);
                        shipyardHasMe = true;
                    }
                    if (agent.remainingDistance != 0)
                    {
                        agent.ResetPath();
                    }
                }
                else
                {
                    if (shipyardHasMe)
                    {
                        currentShipyard.RemoveWorker(this);
                        shipyardHasMe = false;
                    }
                    if (agent.destination != shipStandPoint)
                    {
                        agent.SetDestination(shipStandPoint);
                    }

                }


            }
            else
            {
                GoToIdle();
            }
        }

        if (state == WorkerState.smithing)
        {
            if (currentSmie != null)
            {
                if (Vector3.Distance(transform.position, smieStandPoint) < 1)
                {
                    if (!smieHasMe)
                    {
                        
                        currentSmie.GetWorker(this);
                        smieHasMe = true;
                    }
                    if (agent.remainingDistance != 0)
                    {
                        
                        agent.ResetPath();
                    }
                }
                else
                {
                    if (smieHasMe)
                    {
                        
                        currentSmie.RemoveWorker(this);
                        smieHasMe = false;
                    }
                    if (agent.destination != smieStandPoint)
                    {
                        
                        agent.SetDestination(smieStandPoint);
                    }
                }
            }
            else
            {
                GoToIdle();
            }
        }

        if (state == WorkerState.makingBabies)
        {
            if (Vector3.Distance(transform.position, babyMakerStandPoint) < 1)
            {
                if (agent.remainingDistance != 0)
                {
                    agent.ResetPath();
                }
                atBabyMakerStandPoint = true;
            }
            else
            {
                if (agent.destination != babyMakerStandPoint)
                {
                    agent.SetDestination(babyMakerStandPoint);
                }
                atBabyMakerStandPoint = false;
            }
        }

        if (state != WorkerState.makingBabies && babyMakerHasMe)
        {
            babyMakerHasMe = false;
            currentBabyMaker.RemoveWorker(this);
        }

        if (state != WorkerState.shipBuilder && shipyardHasMe)
        {
            currentShipyard.RemoveWorker(this);
            shipyardHasMe = false;
        }

        if (state != WorkerState.smithing && smieHasMe)
        {
            currentSmie.RemoveWorker(this);
            smieHasMe = false;
        }

        #endregion

        #region Combat
        if (state == WorkerState.attacking)
        {
            if (currentEnemy != null)
            {
                if (Vector3.Distance(transform.position, currentEnemy.transform.position) > 2)
                {
                    agent.SetDestination(currentEnemy.transform.position);
                    attackT = 0;
                    attackingEnemy = false;
                }
                else
                {
                    if (agent.remainingDistance != 0)
                    {
                        agent.ResetPath();
                    }
                    attackingEnemy = true;
                }

                if (attackingEnemy)
                {
                    if (attackT < CombatAttackSpeed())
                    {
                        attackT += Time.deltaTime;
                    }
                    else
                    {
                        attackT = 0;
                        AttackEnemy();
                    }
                }

                //look at enemy
                Quaternion needRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, needRotation, lookAtEnemySpeed * Time.deltaTime);
            }
            else
            {
                GoToIdle();
                
            }
        }

        #endregion

        #region Fishing
        if (state == WorkerState.fishing)
        {

            if (fishingState == FishingState.walkingToFishingPoint)
            {
                if (agent.destination != fishingPoint.transform.position)
                {
                    agent.SetDestination(fishingPoint.transform.position);
                }

                if (Vector3.Distance(transform.position, fishingPoint.transform.position) < 0.5f)
                {
                    agent.ResetPath();
                    StartFishing();
                }
            }
            if (fishingState != FishingState.fishing && fishingRodParent.activeSelf)
            {
                fishingRodParent.SetActive(false);
            }

            if (fishingState == FishingState.fishing)
            {
                if (agent.remainingDistance != 0)
                {
                    agent.ResetPath();
                }

                if (!fishingRodParent.activeSelf)
                {
                    fishingRodParent.SetActive(true);
                }

                if (useFoodCount.value)
                {
                    if(fishingT < currentWorkerLevel.fishingSpeed)
                    {
                        fishingT += Time.deltaTime;
                    }
                    else
                    {
                        fishingT = 0;
                        CatchFish();
                    }
                }

                Vector3 pos = fishingPoint.workerLookPoint.position;
                pos.y = transform.position.y;

                transform.LookAt(pos);
            }
        }

        if(state != WorkerState.fishing && fishingPoint != null)
        {
            StopFishing();
        }
        if (state != WorkerState.fishing && fishingRodParent.activeSelf)
        {
            fishingRodParent.SetActive(false);
        }

        if (fishingHutHasMe && state != WorkerState.fishing)
        {
            currentFishingHut.RemoveWorker(this);
        }
        #endregion

        #region Armory
        if(state == WorkerState.armory)
        {
            if(armoryState == ArmoryState.walkingToArmory)
            {
                if(agent.destination != armoryWalkPoint)
                {
                    agent.SetDestination(armoryWalkPoint);
                }

                if(Vector3.Distance(armoryWalkPoint, transform.position) < 0.5f)
                {
                    GetWeaponsFromArmory();
                }
            }

            if(armoryState == ArmoryState.gettingWeapons)
            {
                transform.LookAt(currentArmory.transform.position);
            }

            if(armoryState == ArmoryState.walkingAwayFromArmory)
            {
                if(agent.destination != armoryWalkBackPoint)
                {
                    agent.SetDestination(armoryWalkBackPoint);
                }

                if(Vector3.Distance(transform.position, armoryWalkBackPoint) < 0.5f)
                {
                    agent.ResetPath();
                    GoToIdle();
                }
            }
        }
        #endregion

        #region Raiding
        if (state == WorkerState.destroyingBuilding)
        {
            if (currentRaidTask == null)
            {
                GoToIdle();
            }
            if (currentRaidTask.dead == true)
            {
                GoToIdle();
            }

            if (Vector3.Distance(transform.position, raidTaskStandPoint) > 1)
            {
                if (agent.destination != raidTaskStandPoint)
                {
                    agent.SetDestination(raidTaskStandPoint);
                }
                raidTaskT = 0;
                attackingRaidTask = false;
            }
            else
            {
                attackingRaidTask = true;
                if (agent.remainingDistance != 0)
                {
                    agent.ResetPath();
                }
            }

            if (attackingRaidTask)
            {
                RotateTowards(currentRaidTask.transform.position, -90);
                if (raidTaskT < CombatAttackSpeed())
                {
                    raidTaskT += Time.deltaTime;
                }
                else
                {
                    raidTaskT = 0;
                    currentRaidTask.TakeDamage(this, CurrentAttackDamage());
                }
            }





        }
        #endregion

        #region Gathering

        if(state != WorkerState.gathering && foodHasMe)
        {
            if(currentFood != null)
            {
                currentFood.RemoveWorker(this);
                currentFood = null;
            }
            else
            {
                foodHasMe = false;
            }
            
        }


        if(state == WorkerState.gathering)
        {
            if(foodHasMe && gatheringState != GatheringState.gathering && gatheringState != GatheringState.walkingToFood)
            {
                currentFood.RemoveWorker(this);
                currentForest = null;
            }
            if(!foodHasMe && (gatheringState == GatheringState.gathering || gatheringState == GatheringState.gathering))
            {
                currentFood.GetWorker(this);
                foodHasMe = true;
            }

            if(gatheringState == GatheringState.walkingToFood)
            {
                if(agent.destination != foodStandPoint)
                {
                    agent.SetDestination(foodStandPoint);
                }

                if(Vector3.Distance(transform.position, foodStandPoint) < 1)
                {
                    ReachedFood();
                }
            }

            if(gatheringState == GatheringState.gathering)
            {
                if(gatheringT < currentWorkerLevel.gatheringSpeed)
                {
                    gatheringT += Time.deltaTime;
                }
                else
                {
                    gatheringT = 0;
                    AttackFood();
                }
            }
            if(gatheringState == GatheringState.walkingToFoodStorage)
            {
                if(agent.destination != foodStorageStandPoint)
                {
                    agent.SetDestination(foodStorageStandPoint);
                }
                if(Vector3.Distance(transform.position, foodStorageStandPoint) < 2)
                {
                    ReachedFoodStorage();
                }
            }
            if(gatheringState == GatheringState.droppingOffAtFoodStorage)
            {
                if(dropFoodT < dropFoodTime)
                {
                    dropFoodT += Time.deltaTime;
                }
                else
                {
                    dropFoodT = 0;
                    DepositFood();
                }
            }
        }

        #endregion

        #region Farming
        if(state == WorkerState.farming)
        {
            FarmingFunction();
        }

        if(farmHasMe && state != WorkerState.farming)
        {
            currentFarm.RemvoeWorker(this);
            farmHasMe = false;
        }
        #endregion

        #region Hunting
        if(state == WorkerState.hunting)
        {
            HuntingFunction();
        }

        #endregion
    }




    #region Logging functions
    void LoggingFunction()
    {
        if ((wood.value + currentWoodStored) > wood.capacity)
        {
            if (agent.remainingDistance != 0)
            {
                agent.ResetPath();
            }
            loggingState = LoggingState.waitingForMoreWoodCapacity;
        }


        if (loggingState == LoggingState.chopping && !treeHasMe)
        {

            currentTree.GetWorker(this);
            treeHasMe = true;
        }
        if (loggingState == LoggingState.walkingToTree && !treeHasMe)
        {
            currentTree.GetWorker(this);
            treeHasMe = true;
        }

        if (loggingState != LoggingState.walkingToTree && loggingState != LoggingState.chopping && treeHasMe)
        {
            currentTree.RemoveWorker(this);
            treeHasMe = false;
        }

        if (loggingState == LoggingState.lookingForForest)
        {
            currentForest = StructureManager.Instance.GetClosestForest(transform.position);
            if (currentForest != null)
            {
                currentTree = currentForest.GetClosestNotBusyTree(this);
                if (currentTree == null)
                {
                    loggingState = LoggingState.lookingForTree;
                }
                else
                {
                    treeStandPoint = currentTree.GetStandPoint();
                    loggingState = LoggingState.walkingToTree;
                }


            }
        }
        if (loggingState == LoggingState.lookingForTree)
        {
            currentTree = currentForest.GetClosestNotBusyTree(this);

            if (currentTree != null)
            {
                treeStandPoint = currentTree.GetStandPoint();
                loggingState = LoggingState.walkingToTree;
            }
        }
        if (loggingState == LoggingState.walkingToTree)
        {
            if (currentTree == null)
            {
                loggingState = LoggingState.lookingForTree;
            }

            if (agent.destination != treeStandPoint)
            {
                agent.SetDestination(treeStandPoint);
            }
            if (!treeHasMe)
            {
                currentTree.GetWorker(this);
                treeHasMe = true;
            }
            if (Vector3.Distance(transform.position, treeStandPoint) < 0.1f)
            {
                agent.ResetPath();

                loggingState = LoggingState.chopping;
            }
        }
        if (loggingState == LoggingState.chopping)
        {
            audioSource.clip = null;
            if (currentProgressBar == null)
            {
                currentProgressBar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.3f);
            }
            currentProgressBar.bar.fillAmount = choppingT / currentWorkerLevel.loggingSpeed;

            RotateTowards(currentTree.transform.position, 0);

            //Audio
            if (playWoodChopSoundT < playWoodChopSoundRate)
            {
                playWoodChopSoundT += Time.deltaTime;
            }
            else
            {
                playWoodChopSoundT = 0;
                audioSource.loop = true;
                Debug.Log("Played wood chop sound");
                audioSource.PlayOneShot(woodChopSound, 1f);
            }


            //chopping animation state
            if (choppingT < LoggingSpeed())
            {
                //Play chopping sound Logic
                choppingT += Time.deltaTime;
            }
            else
            {
                choppingT = 0;
                ChopTree();
            }

        }
        if (loggingState == LoggingState.lookingForWoodStorage)
        {
            currentWoodStorage = StructureManager.Instance.GetCloseWoodStorage(transform.position);
            if (currentWoodStorage != null)
            {
                loggingState = LoggingState.walkingToWoodStorage;
                woodStorageStandPoint = currentWoodStorage.GetStandPoint();
            }
            else
            {
                Debug.Log("No Wood storage found");
            }
        }
        if (loggingState == LoggingState.walkingToWoodStorage)
        {
            if (agent.destination != woodStorageStandPoint)
            {
                agent.SetDestination(woodStorageStandPoint);
            }

            if (Vector3.Distance(transform.position, woodStorageStandPoint) < 0.5f)
            {
                loggingState = LoggingState.droppingOffWoodAtStorage;
                storeWoodT = 0;
                agent.ResetPath();
            }
        }
        if (loggingState == LoggingState.droppingOffWoodAtStorage)
        {
            RotateTowards(currentWoodStorage.transform.position, 180);


            if (storeWoodT < dropOffWoodTime)
            {
                storeWoodT += Time.deltaTime;
            }
            else
            {
                StoreWoodAtStorage();
            }

        }
        if (loggingState == LoggingState.waitingForMoreWoodCapacity)
        {
            if ((wood.value + WoodPerTreeChopped()) <= wood.capacity)
            {
                if (currentWoodStored == 0)
                {
                    loggingState = LoggingState.lookingForForest;
                }
                else
                {
                    loggingState = LoggingState.walkingToWoodStorage;
                }

            }
        }

        if (loggingState != LoggingState.chopping && currentProgressBar != null)
        {
            Destroy(currentProgressBar.gameObject);
            currentProgressBar = null;
        }
    }

    public float LoggingSpeed()
    {
        float t = currentWorkerLevel.loggingSpeed;

        float k = t - 4;
        float j = t + 4;

        float s = Random.Range(k, j);
        
        return s;
    }
    public float WoodPerTreeChopped()
    {
        float t = currentWorkerLevel.WoodPerTreeChopped;

        return t;
    }

    void ChopTree()
    {
        currentWoodStored = WoodPerTreeChopped();

        currentWoodStorage = StructureManager.Instance.GetCloseWoodStorage(transform.position);
        if(currentWoodStorage == null)
        {
            loggingState = LoggingState.lookingForWoodStorage;
        }
        else
        {
            woodStorageStandPoint = currentWoodStorage.GetStandPoint();
            loggingState = LoggingState.walkingToWoodStorage;
        }
    }

    void StoreWoodAtStorage()
    {
        if((wood.value + WoodPerTreeChopped()) > wood.capacity)
        {
            NotificationManager.Instance.OpenNotification("Cant store wood", "Theres is no more wood capacity, cant dropp wood here", 5f, null);
            loggingState = LoggingState.waitingForMoreWoodCapacity;
        }
        else
        {
            currentWoodStorage.GetWood(WoodPerTreeChopped());
            wood.value += currentWoodStored;
            GetWorkerXP(currentWorkerLevel.xpPerWoodDrop);
            currentWoodStored = 0;
            currentWoodStorage = null;
            loggingState = LoggingState.lookingForForest;
        }
        
    }

    public void StartLogging(Tree t)
    {
        if((wood.value + 1) > wood.capacity)
        {
            NotificationManager.Instance.OpenNotification("No", "There is no more room for wood, build more storage!", 5f, null);
        }
        currentTree = t;
        currentForest = currentTree.myForest;
        state = WorkerState.logging;
        treeStandPoint = currentTree.GetStandPoint();
        loggingState = LoggingState.walkingToTree;
    }
    #endregion

    #region Myrmalm Fuctions
    public float MyrmalmGatheringSpeed()
    {
        float t = currentWorkerLevel.myrmalmGatherSpeed;

        

        return t;
    }
    public float MyrmalmPerGather()
    {
        float t = currentWorkerLevel.myrmalmPer;

        float k = Random.Range(t - 3, t + 3);

        return t;
    }

    void GatherMyrmalm()
    {
        currentMyrmalmStored += MyrmalmPerGather();

        currentMyrmalmStorage = StructureManager.Instance.GetCloseMyrmalmStorage(transform.position);
        if (currentMyrmalmStorage == null)
        {
            myrmalmState = MyrmalmState.lookingForMyrmalmStorage;
        }
        else
        {
            myrmalmStorageStandPoint = currentMyrmalmStorage.GetStandPoint(transform.position);
            myrmalmState = MyrmalmState.walkingToMyrmalmStorage;
        }
    }

    void StoreMyrmalmAtMyrmalmStorage()
    {
        if ((myrmalm.value + MyrmalmPerGather()) > myrmalm.capacity)
        {
            NotificationManager.Instance.OpenNotification("Cant store Myrmalm", "Theres is no more Myrmalm capacity, cant dropp Myrmalm here", 5f, null);
            myrmalmState = MyrmalmState.waitingForMoreMyrmalmCapacity;
        }
        else
        {
            currentMyrmalmStorage.GetMyrmalm(MyrmalmPerGather());
            myrmalm.value += currentMyrmalmStored;
            GetWorkerXP(currentWorkerLevel.xpPerMyrmalmDrop);
            currentMyrmalmStored = 0;
            myrmalmState = MyrmalmState.lookingForMyr;
        }

    }

    public void StartMyrmalmGathering(Myr myr)
    {
        if ((myrmalm.value + 1) > myrmalm.capacity)
        {
            NotificationManager.Instance.OpenNotification("No", "Fuck you, you dont have room for more myrmalm", 5f, null);
        }
        currentMyr = myr;
        state = WorkerState.myrmalmGathering;
        myrmalmState = MyrmalmState.lookingForMyrmalm;
    }

    public void GetMyrovn(Myrovn m)
    {
        currentMyrovn = m;
        state = WorkerState.myrovn;

        myrovnState = MyrovnState.walkingToMyrovn;
        myrovnStandPoint = currentMyrovn.GetStandPoint();
    }
    #endregion

    #region Training Ground functions
    public void GetTrainingGrounds(TrainingGrounds t, Vector3 queueSpot)
    {
        state = WorkerState.training;

        trainingState = TrainingState.walkingToQueueSpot;

        currentQueueStandPoint = queueSpot;
    }

    public void GetQueueSpot(Vector3 pos)
    {
        if(state == WorkerState.training)
        {
            trainingState = TrainingState.walkingToQueueSpot;
            currentQueueStandPoint = pos;
        }
    }

    public void StartTraining(Vector3 pos)
    {
        trainingState = TrainingState.moveToTrainingSpot;
        trainingStandPoint = pos;
    }

    public void EndTraining(Vector3 pos)
    {
        outOfCampWalkPoint = pos;
        trainingState = TrainingState.walkingOutOfCamp;

        //Turn viking
        TurnViking();
    }
    public void TurnViking()
    {
        workerType = WorkerType.Viking;
        workerGraphic.SetGraphic();
        NotificationManager.Instance.OpenNotification("New Viking!", myName.nameString + " just became a viking!", 5f, null);
    }

    #endregion

    #region Fishing functions

    public void GetFishingHut(FishingHut hut)
    {
        if(currentFishingHut != null && currentFishingHut != hut)
        {
            hut.RemoveWorker(this);
        }
        currentFishingHut = hut;

        hut.GetWorker(this);

        state = WorkerState.fishing;
        fishingState = FishingState.walkingToFishingPoint;

        fishingPoint = currentFishingHut.GetFishingPoint(this);
        fishingPoint.GetWorker(this);
    }

    void CatchFish()
    {
        foodCount.value += currentWorkerLevel.foodPerFishCatch;
    }

    public void StopFishing()
    {
        if(fishingPoint != null)
        {
            fishingPoint.RemoveWorker(this);
            fishingPoint = null;
        }
    }
        

    void StartFishing()
    {
        currentFishingHut.WorkerStartedFishing(this);
        fishingHutHasMe = true;
        fishingState = FishingState.fishing;
    }

    #endregion

    #region Gathering
    public void GetGatherableFood(GatherableFood f)
    {
        currentFood = f;

        currentFood.GetWorker(this);
        foodHasMe = true;

        currentGatheringArea = currentFood.gatheringArea;

        foodStandPoint = currentFood.GetStandPoint();
        state = WorkerState.gathering;

        gatheringState = GatheringState.walkingToFood;
    }
    void AttackFood()
    {
        currentFood.TakeDamage(currentWorkerLevel.gatheringDamage, this);
    }
    void ReachedFood()
    {
        agent.ResetPath();
        gatheringState = GatheringState.gathering;
    }
    void ReachedFoodStorage()
    {
        agent.ResetPath();
        gatheringState = GatheringState.droppingOffAtFoodStorage;
    }
    void DepositFood()
    {
        foodCount.value += foodToDeposit;

        currentFood = currentGatheringArea.GetCloseFood(transform.position);
        if(currentFood != null)
        {
            currentFood.GetWorker(this);
            foodHasMe = true;

            currentGatheringArea = currentFood.gatheringArea;

            foodStandPoint = currentFood.GetStandPoint();
            state = WorkerState.gathering;

            gatheringState = GatheringState.walkingToFood;
        }
        else
        {

            GoToIdle();
        }

    }
    public void GatheredFood(float amount)
    {
        gatheringState = GatheringState.lookingForFoodStorage;

        foodToDeposit = amount;

        currentFoodStorage = StructureManager.Instance.GetCloseFoodStorage(transform.position);

        currentFood.RemoveWorker(this);
        foodHasMe = false;
        if(currentFoodStorage == null)
        {
            gatheringState = GatheringState.lookingForFoodStorage;
            GoToIdle();
            return;
        }
        foodStorageStandPoint = currentFoodStorage.GetStandPoint();

        gatheringState = GatheringState.walkingToFoodStorage;
    }
    #endregion

    #region Armory
    public void GetArmory(Armory a)
    {
        currentArmory = a;

        armoryWalkPoint = currentArmory.walkPoint.position;

        state = WorkerState.armory;

        armoryState = ArmoryState.walkingToArmory;
    }

    void GetWeaponsFromArmory()
    {
        armoryState = ArmoryState.gettingWeapons;

        currentArmory.GiveMeWeapons(this);

        armoryWalkBackPoint = currentArmory.walkBackPoint.position;

        EquipWeapons();

        armoryState = ArmoryState.walkingAwayFromArmory;
    }
    #endregion

    #region Saving n stuff
    public void SetWorkerLevel()
    {
        workerLevelIndex = myData.workerLevel;
        currentWorkerLevel = workerLevels[workerLevelIndex];

        currentXP = myData.workerXP;

        combatLevelIndex = myData.combatLevel;
        currentCombatLevel = combatLevels[combatLevelIndex];
        currentCombatXP = myData.combatXP;
        myData.goalCombatXP = currentCombatLevel.xpToLevelUp;
    }
   

    public void LevelUpWorkerLevel()
    {
        workerLevelIndex += 1;

        if(workerType == WorkerType.Child)
        {
            workerType = WorkerType.Adult;
            string s = myName.nameString + " became an adult!";
            workerGraphic.SetGraphic();
            NotificationManager.Instance.OpenNotification(myName.nameString, s, 3f, null);
        }
        if(workerLevelIndex >= workerLevels.Length)
        {
            workerLevelIndex = workerLevels.Length - 1;
        }

        if(levelUpClip != null)
        {
            AudioSource.PlayClipAtPoint(levelUpClip, transform.position);
        }

        currentWorkerLevel = workerLevels[workerLevelIndex];

        currentXP = 0;
        
    }


    public void GetWorkerXP(float amount)
    {
        currentXP += amount;

        if(currentXP >= currentWorkerLevel.xpForNextLevel)
        {
            LevelUpWorkerLevel();
        }
    }
    public void SetWorkerDataInfo()
    {
        //saving, changeing stats on my seave element
        myData.position = transform.position;
        myData.rotation = transform.rotation;
        myData.myName = myName;
        //set my data type
        myData.goalWorkerXP = currentWorkerLevel.xpForNextLevel;
        myData.workerXP = currentXP;
        myData.type = workerType;


    }
    #endregion

    #region Equipping weapons fuctions
    public void GetWeapon(WeaponType t)
    {
        if(t == WeaponType.sword)
        {
            myData.hasSword = true;
        }
        if (t == WeaponType.shield)
        {
            myData.hasShield = true;
        }
        if (t == WeaponType.helmet)
        {
            myData.hasHelmet = true;
        }

        EquipWeapons();
    }

    public void RemovepWeapon(WeaponType t)
    {
        if (t == WeaponType.sword)
        {
            myData.hasSword = false;
        }
        if (t == WeaponType.shield)
        {
            myData.hasShield = false;
        }
        if (t == WeaponType.helmet)
        {
            myData.hasHelmet = false;
        }

        UnequipWeapon(false, t);
    }

    public void EquipWeapons()
    {
        if(myData.hasSword && swordGO == null)
        {
            swordGO = Instantiate(swordPrefab, swordIdleParent);
        }
        if (myData.hasShield && shieldGO == null)
        {
            shieldGO = Instantiate(shieldPrefab, shieldIdleParent);
        }
        if (myData.hasHelmet && helmetGO == null)
        {
            helmetGO = Instantiate(helmetPrefab, helmetIdleParent);
        }
    }
    
    public void UnequipWeapon(bool uneqiupAll, WeaponType t)
    {
        if (uneqiupAll)
        {
            if(swordGO != null)
            {
                Destroy(swordGO);
                swordGO = null;
            }
            if (shieldGO != null)
            {
                Destroy(shieldGO);
                shieldGO = null;
            }
            if (helmetGO != null)
            {
                Destroy(helmetGO);
                helmetGO = null;
            }
        }
        else
        {
            if(t == WeaponType.sword)
            {
                if (swordGO != null)
                {
                    Destroy(swordGO);
                    swordGO = null;
                }
            }
            if(t == WeaponType.shield)
            {
                if (shieldGO != null)
                {
                    Destroy(shieldGO);
                    shieldGO = null;
                }
            }
            if(t == WeaponType.helmet)
            {
                if (helmetGO != null)
                {
                    Destroy(helmetGO);
                    helmetGO = null;
                }
            }
        }

    }
    #endregion

    #region Raiding
    public void GetGold(float amount, bool giveNotification)
    {
        storedGold += amount;

        if (giveNotification)
        {
            GoldNotificationManager.Instance.GiveMeGoldNotification(this.gameObject, amount, 0.5f);
        }

    }

    public void GetRaidTask(RaidTask task)
    {
        Debug.Log("Got raid task");
        currentRaidTask = task;
        raidTaskStandPoint = currentRaidTask.GetStandPoint();
        if (state != WorkerState.destroyingBuilding)
        {
            state = WorkerState.destroyingBuilding;
        }
        
    }
    #endregion

    #region Farming
    void FarmingFunction()
    {
        if(Vector3.Distance(transform.position, farmingStandPoint) > 1)
        {
            if(agent.destination != farmingStandPoint)
            {
                agent.SetDestination(farmingStandPoint);
            }

            farming = false;
        }
        else
        {
            if(agent.remainingDistance != 0)
            {
                agent.ResetPath();
                
            }
            farming = true;
        }


        if (farming)
        {
            if(farmingT < currentFarmingSpeed)
            {
                farmingT += Time.deltaTime;
            }
            else
            {
                farmingT = 0;
                FarmedCrop();
            }
        }
    }

    public void SetCurrentFarmingSpeed()
    {
        float t = currentWorkerLevel.farmingSpeed;

        float min = t * 0.8f;
        float max = t * 2f;

        t = Random.Range(min, max);

        currentFarmingSpeed = t;
    }

    void FarmedCrop()
    {
        SetCurrentFarmingSpeed();
        foodCount.value += currentWorkerLevel.foodPerFarming;
    }

    public void GetFarm(Farm f)
    {
        currentFarm = f;
        farmingStandPoint = currentFarm.GetFarmingStandPoint();

        SetCurrentFarmingSpeed();

        currentFarm.GetWorker(this);
        farmHasMe = true;

        state = WorkerState.farming;
    }

    #endregion

    #region Hunting
    void HuntingFunction()
    {
        if(huntingState == HuntingState.walkingToHuntingArea)
        {
            if(agent.destination != currentHuntingStandPoint)
            {
                agent.SetDestination(currentHuntingStandPoint);
            }

            if(Vector3.Distance(transform.position, currentHuntingStandPoint) < 1)
            {
                agent.ResetPath();
                huntingState = HuntingState.hunting;
            }
        }

        if(huntingState == HuntingState.hunting)
        {
            if (agent.destination != currentHuntingStandPoint)
            {
                agent.SetDestination(currentHuntingStandPoint);
            }

            if (Vector3.Distance(transform.position, currentHuntingStandPoint) < 1)
            {
                agent.ResetPath();
            }

            if (huntingSwapT < currentHuntingSwapSpeed)
            {
                huntingSwapT += Time.deltaTime;
            }
            else
            {
                huntingSwapT = 0;
                currentHuntingStandPoint = currentHuntingArea.GetHuntingStandPoint();
                SetHuntingSwapSpeed();
            }

            if(huntingT < currentHuntingSpeed)
            {
                huntingT += Time.deltaTime;
            }
            else
            {
                huntingT = 0;
                SetHuntingSpeed();
                CatchAnimalFromHunting();
            }
        }

        if(huntingState == HuntingState.walkingToFoodStorage)
        {
            if (agent.destination != foodStorageStandPoint)
            {
                agent.SetDestination(foodStorageStandPoint);
            }

            if (Vector3.Distance(transform.position, foodStorageStandPoint) < 1)
            {
                agent.ResetPath();
                huntingState = HuntingState.droppingOffAtFoodStorage;
            }


        }

        if(huntingState == HuntingState.droppingOffAtFoodStorage)
        {
            if(dropFoodT < dropFoodTime)
            {
                dropFoodT += Time.deltaTime;
            }
            else
            {
                dropFoodT = 0;
                DropOffHuntedFood();
            }
        }
    }

    void DropOffHuntedFood()
    {
        foodCount.value += currentWorkerLevel.foodPerHuntReturn;

        huntingState = HuntingState.walkingToHuntingArea;
        currentHuntingStandPoint = currentHuntingArea.GetHuntingStandPoint();
    }
    void CatchAnimalFromHunting()
    {
        SetHuntingSwapSpeed();

        currentFoodStorage = StructureManager.Instance.GetCloseFoodStorage(transform.position);

        huntingState = HuntingState.walkingToFoodStorage;

        foodStorageStandPoint = currentFoodStorage.GetStandPoint();
    }
    void SetHuntingSwapSpeed()
    {
        float t = swapHuntingSpotSpeed;

        float min = t * 0.8f;
        float max = t * 2f;

        t = Random.Range(min, max);
        currentHuntingSwapSpeed = t;
    }
    void SetHuntingSpeed()
    {
        float t = currentWorkerLevel.huntingSpeed;

        float min = t * 0.7f;
        float max = t * 2f;

        t = Random.Range(min, max);
        currentHuntingSpeed = t;
    }
    public void GetHuntingLodge(HuntingLodge hl)
    {
        SetHuntingSwapSpeed();
        currentHuntingLodge = hl;
        currentHuntingArea = currentHuntingLodge.GetCloseHutningArea(transform.position);

        state = WorkerState.hunting;

        huntingState = HuntingState.walkingToHuntingArea;

        currentHuntingStandPoint = currentHuntingArea.GetHuntingStandPoint();

        SetHuntingSwapSpeed();
    }
    public void GetHuntingArea(HuntingArea a)
    {
        SetHuntingSwapSpeed();
        currentHuntingArea = a;

        state = WorkerState.hunting;

        huntingState = HuntingState.walkingToHuntingArea;
    }

    #endregion
    public void RotateTowards(Vector3 target, float yOffset)
    {
        Vector3 vectorToTarget = target - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - yOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }
    void GoToIdle()
    {
        float t = 0;
        t = Random.Range(0, 100);
        if(t < 80)
        {
            //(AudioType.happy);
        }
        
        if (forcedIdle)
        {
            
            return;
        }

        AudioManager.Instance.PlayWorkerWentToIdle(true, 10, transform.position);
        state = WorkerState.idle;
    }

    #region GetCityBuilding stuff
    public void GetBabyMaker(BabyMaking b)
    {
        PlayVoiceLine(AudioType.babyMaking);
        if (forcedIdle)
        {
            return;
        }
        state = WorkerState.makingBabies;
        currentBabyMaker = b;
        babyMakerStandPoint = currentBabyMaker.MyStandPoint(this);
    }
   

    public void GetSmie(Smie smie)
    {
        
        if (forcedIdle)
        {
            return;
        }
        currentSmie = smie;
        smieStandPoint = currentSmie.GetSmieStandPoint();
        agent.SetDestination(smieStandPoint);
        state = WorkerState.smithing;

    }
    public void GetBuilding(BuildingStructures b)
    {
        
        if (forcedIdle)
        {
            return;
        }
        state = WorkerState.building;
        currentBuilding = b;
        buildStandPoint = b.GetStandPoint();
    }

    public void FinishedBuilding()
    {
        if(currentBuilding != null)
        {
            currentBuilding.RemoveWorker(this);
        }
        
        GoToIdle();
    }
    public void GetShipyard(Shipyard shipyard)
    {
        if (forcedIdle)
        {
            return;
        }
        state = WorkerState.shipBuilder;
        shipStandPoint = shipyard.GetStandPoint(transform.position);
        currentShipyard = shipyard;
    }
   
   

    
    public void GetTrainingGrounds()
    {
       
        if (forcedIdle)
        {
            return;
        }
        state = WorkerState.training;


    }
    public void StopBuildingShip()
    {

    }


    public void GetDestination(Vector3 pos)
    {
        currDestination = pos;
        agent.SetDestination(currDestination);
        GoToIdle();
        movingToDestination = true;
    }

    #endregion

    #region Audio
    void PlayVoiceLine(AudioType t)
    {
        AudioManager.Instance.PlayWorkerVoiceline(true, 500, transform.position, t);
    }

    

    #endregion

    #region Combat

    public void KilledEnemy()
    {
        currentCombatXP += 5;
    }

    public void GetEnemy(EnemyUnit e)
    {
        if(state != WorkerState.attacking)
        {
            state = WorkerState.attacking;          
        }
        EquipWeapons();
        currentEnemy = e;
    }
    void UpdateCombatLevel()
    {
        myData.combatLevel = combatLevelIndex;
        myData.combatXP = currentCombatXP;
        myData.goalCombatXP = currentCombatLevel.xpToLevelUp;
    }

    public void AttackEnemy()
    {
        currentEnemy.TakeDamage(CurrentAttackDamage(), this);
    }
    public float CurrentAttackDamage()
    {
        float dmg = currentCombatLevel.baseAttackDamage;

        if (myData.hasSword)
        {
            dmg += currentCombatLevel.swordBonusAttackDamage;
        }

        return dmg;
    }
    public void LevelUpCombat()
    {
        combatLevelIndex += 1;
        if (combatLevelIndex >= combatLevels.Length)
        {
            combatLevelIndex = combatLevels.Length;
        }

        currentCombatXP = 0;
        currentCombatLevel = combatLevels[combatLevelIndex];
        health = currentCombatLevel.maxHealth + BonusHealth();
        UpdateCombatLevel();
    }
    float CombatAttackSpeed()
    {

        return currentCombatLevel.baseAttackSpeed;
    }
    float BonusHealth()
    {
        float t = 0;
        if (myData.hasHelmet)
        {
            t += currentCombatLevel.helmetBonusHealth;
        }

        return t;
    }
    float DamageToMidigate()
    {
        float t = 0;

        if (myData.hasShield)
        {
            t = currentCombatLevel.shieldDamageMidigation;
        }

        return t;
    }
    public void TakeDamage(EnemyUnit enemy, float amount)
    {
        amount -= DamageToMidigate();
        health -= amount;

        //Maybe change to if "enemy unit == null";
        if (enemy != null && currentEnemy == null)
        {
            GetEnemyUnit(enemy);
        }
        //if (myProgressBar == null)
        //{
        //    myProgressBar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.3f);
        //}
        //myProgressBar.bar.fillAmount = health / 100;

        if (health <= 0)
        {
            Die(false);
        }
    }

    public void GetEnemyUnit(EnemyUnit enemy)
    {
        state = WorkerState.attacking;

        currentEnemy = enemy;
    }
    #endregion

    #region Selected
    public void Selected()
    {
        selectedIndicator.SetActive(true);
    }

    public void Deselected()
    {
        selectedIndicator.SetActive(false);
        
    }
    #endregion

    #region Death
    public void Die(bool destroyMe)
    {
        if(bar != null)
        {
            Destroy(bar.gameObject);
        }

        isDead = true;

        if (destroyMe)
        {
            PopulationManager.Instance.RemoveWorker(this);
            Destroy(gameObject);
        }
    }
    #endregion

    #region NavMesh
    public void TurnOffNavMeshAgent()
    {
        agent.enabled = false;
    }
    public void TurnOnNavMeshAgent()
    {
        agent.enabled = true;
    }
    #endregion
}

[System.Serializable]
public enum WorkerType { Child, Adult, Viking, Trell }
