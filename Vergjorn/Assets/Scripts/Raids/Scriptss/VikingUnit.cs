using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VikingUnit : MonoBehaviour
{
    public RaidTask myTask;

    public WorkerData myData;

    public string myName;

    public enum VikingState { idle, attackingEnemy, destroyingBuilding, inCutscene}
    public VikingState state;
    public bool forcedIdle;

    public float rotateSpeed = 4f;
    [Header("Health")]
    public float health;

    [Header("Equipment")]
    public Transform swordParent;
    public Transform helmetParent;
    public Transform shieldParent;
    public bool hasSword;
    public bool hasShield;
    public bool hasHelmet;

    public GameObject swordPrefab;
    public GameObject shieldPrefab;
    public GameObject helmetPrefab;
    [Space]

    #region Combat
    //CombatStuff
    [HideInInspector]
    public CombatLevel currentCombatLevel;
   
    public CombatLevel[] combatLevels;
    int combatLevelIndex;
    public float currentXp;
    public float xpFromEnemy;
    public float attackRange = 2f;
    #endregion



    [Header("Gold Storage")]
    public float storedGold;


    #region Destroy buildings
    [Header("Destroying Buildings")]
    RaidTask currentTask;
    bool attackingTask;
    float taskT;
    public float buildingAttackSpeed;
    public float buildingAttackDamage;
    Vector3 standPoint;
    #endregion

    #region Walking
    public bool walkingToDestination;
    Vector3 curDestination;
    #endregion

    #region Attack Enemy Unit
    [Header("Enemy Killing")]
    public EnemyUnit enemyUnit;
    public float enemyAttackDamage;
    public float enemyAttackSpeed;
    float attackT;
    bool attackingEnemy;

    public bool getNewEnemyAfterDeath = true;
    #endregion

    

    private NavMeshAgent agent;
    private RaidManager raidManager;
    

    ProgressBar myProgressBar;
    public float lookAtEnemySpeed = 4;

    public GameObject selectionGraphic;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        //RaidManager.Instance.GetViking(this);

        selectionGraphic.SetActive(false);

        SetCombatLevel();

        EnableWeapons();
    }
    private void Update()
    {
        UpdateCombatLevel();
        #region Destroying building logic
        if(state == VikingState.destroyingBuilding)
        {
            if(currentTask == null)
            {
                GoToIdle();
            }
            if (currentTask.dead == true)
            {
                GoToIdle();
            }

            if(Vector3.Distance(transform.position, standPoint) > 1)
            {
                if(agent.destination != standPoint)
                {
                    agent.SetDestination(standPoint);
                }
                taskT = 0;
                attackingTask = false;
            }
            else
            {
                attackingTask = true;
                if(agent.remainingDistance != 0)
                {
                    agent.ResetPath();
                }
            }

            if (attackingTask)
            {
                RotateTowards(enemyUnit.transform.position, -90);
                if (taskT < buildingAttackSpeed)
                {
                    taskT += Time.deltaTime;
                }
                else
                {
                    taskT = 0;
                    currentTask.TakeDamage(null, buildingAttackDamage);
                }
            }
        

            

            
        }
        #endregion


        #region Attacking Enemies
        if(state == VikingState.attackingEnemy)
        {
            if(enemyUnit == null || enemyUnit.isDead == true)
            {
                GetNewEnemy();
            }

            if(enemyUnit != null)
            {
                if (Vector3.Distance(transform.position, enemyUnit.transform.position) > 2)
                {
                    agent.SetDestination(enemyUnit.transform.position);
                    
                }
                else
                {
                    if (agent.remainingDistance != 0)
                    {
                        agent.ResetPath();
                    }
                    
                }

                if(Vector3.Distance(transform.position, enemyUnit.transform.position) < attackRange)
                {
                    attackingEnemy = true;
                }
                else
                {
                    attackingEnemy = false;
                    attackT = 0;
                }

                if (attackingEnemy)
                {
                    Quaternion needRotation = Quaternion.LookRotation(enemyUnit.transform.position - transform.position);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, needRotation, lookAtEnemySpeed * Time.deltaTime);

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
                
            }
            else
            {
                //RaidManager.Instance.GetClosestEnemyUnit(this);
            }

        }
        else
        {
            attackT = 0;
        }
        #endregion

        if(state == VikingState.idle)
        {
           
            if (Vector3.Distance(transform.position, curDestination) < 1)
            {
                agent.ResetPath();
            }
            
        }
        

       
    }
     void RotateTowards(Vector3 target, float yOffset)
    {
        Vector3 vectorToTarget = target - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - yOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }

    void EnableWeapons()
    {
        if (myData.hasSword)
        {
            GameObject go = Instantiate(swordPrefab, swordParent);
            hasSword = true;
        }
        if (myData.hasShield)
        {
            GameObject go = Instantiate(shieldPrefab, shieldParent);
            hasShield = true;
        }
        if (myData.hasHelmet)
        {
            GameObject go = Instantiate(helmetPrefab, helmetParent);
            hasHelmet = true;
        }
    }

    void GetNewEnemy()
    {
        // = RaidManager.Instance.VikingGetEnemy(this);
        if(enemyUnit != null)
        {
            attackT = 0;  

            state = VikingState.attackingEnemy;
        }
        else
        {
            GoToIdle();
        }
    }
    void GoToIdle()
    {
        agent.ResetPath();
        state = VikingState.idle;
    }

    #region Combat

    float CombatAttackSpeed()
    {
        return currentCombatLevel.baseAttackSpeed;
    }
    void SetCombatLevel()
    {
        combatLevelIndex = myData.combatLevel;
        currentCombatLevel = combatLevels[combatLevelIndex];
        currentXp = myData.combatXP;
        myData.goalCombatXP = currentCombatLevel.xpToLevelUp;
    }

    void UpdateCombatLevel()
    {
        myData.combatLevel = combatLevelIndex;
        myData.combatXP = currentXp;
        myData.goalCombatXP = currentCombatLevel.xpToLevelUp;
    }

    public void LevelUpCombat()
    {
        combatLevelIndex += 1;
        if(combatLevelIndex >= combatLevels.Length)
        {
            combatLevelIndex = combatLevels.Length;
        }

        currentXp = 0;
        currentCombatLevel = combatLevels[combatLevelIndex];
        health = currentCombatLevel.maxHealth + BonusHealth();
        UpdateCombatLevel();
    }

    float BonusHealth()
    {
        float t = 0;
        if (hasHelmet)
        {
            t += currentCombatLevel.helmetBonusHealth;
        }

        return t;
    }
    public void AttackEnemy()
    {
        enemyUnit.TakeDamage(CurrentAttackDamage(), null);
    }

    public void KilledEnemy()
    {
        currentXp += xpFromEnemy;

        if(currentXp >= currentCombatLevel.xpToLevelUp)
        {
            LevelUpCombat();
        }
    }

    public float CurrentAttackDamage()
    {
        float dmg = currentCombatLevel.baseAttackDamage;

        if (hasSword)
        {
            dmg += currentCombatLevel.swordBonusAttackDamage;
        }

        return dmg;
    }

    public void GetEnemyUnit(EnemyUnit unit)
    {
        if (forcedIdle)
        {
            return;
        }

        if (unit == null)
        {
            GoToIdle();
            return;
        }
        enemyUnit = unit;

        state = VikingState.attackingEnemy;

    }

    #endregion

    #region Equipment
    public void GetWeapon(WeaponType t, GameObject weaponPrefab)
    {
        if(t == WeaponType.helmet)
        {
            if (!hasHelmet)
            {
                ActivateWeapon(t, weaponPrefab);
            }
        }else if (t == WeaponType.sword)
        {
            if (!hasSword)
            {
                ActivateWeapon(t, weaponPrefab);
            }
        }else if (t == WeaponType.shield)
        {
            if (!hasShield)
            {
                ActivateWeapon(t, weaponPrefab);
            }
        }

    }
    void ActivateWeapon(WeaponType t, GameObject weaponPrefab)
    {
        if(t == WeaponType.sword)
        {
            GameObject go = Instantiate(weaponPrefab, swordParent);
            hasSword = true;
        }else if (t == WeaponType.shield)
        {
            GameObject go = Instantiate(weaponPrefab, shieldParent);
            hasShield = true;
        }else if (t == WeaponType.helmet)
        {
            GameObject go = Instantiate(weaponPrefab, helmetParent);
            hasHelmet = true;
        }

    }

    #endregion

    #region Destroying Buildings
    public void GetRaidTask(RaidTask task)
    {
        if (forcedIdle)
        {
            return;
        }
        currentTask = task;
       
        standPoint = currentTask.GetStandPoint();
        state = VikingState.destroyingBuilding;
    }


    #endregion

    #region GetGold
    public void GetGold(float amount, bool giveNotification)
    {
        storedGold += amount;

        if (giveNotification)
        {
            GoldNotificationManager.Instance.GiveMeGoldNotification(this.gameObject, amount, 0.5f);
        }

    }
    #endregion

    #region Walking
    public void GetDestination(Vector3 pos)
    {
        if (forcedIdle)
        {
            return;
        }
        curDestination = pos;

        agent.SetDestination(curDestination);
        state = VikingState.idle;
       
    }
    #endregion

    #region Taking damage and dying

    public void TakeDamage(float amount, EnemyUnit unit)
    {
        amount -= DamageToMidigate();
        health -= amount;

        //Maybe change to if "enemy unit == null";
        if(unit != null && enemyUnit == null)
        {
            GetEnemyUnit(unit);
        }
        if(unit != null)
        {
            DamageNotificationManager.Instance.GiveMeDamageNotification(this.gameObject, amount, 0.5f);
        }
        if(myProgressBar == null)
        {
            myProgressBar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.3f);
        }
        myProgressBar.bar.fillAmount = health / 100;

        if (health <= 0)
        {
            Die();
        }
    }

    float DamageToMidigate()
    {
        float t = 0;

        if (hasShield)
        {
            t = currentCombatLevel.shieldDamageMidigation;
        }

        return t;
    }
    public void Die()
    {
        Destroy(myProgressBar.gameObject);
        //RaidManager.Instance.RemoveViking(this);
        Destroy(gameObject);
    }
    #endregion

    #region Selection

    public void Selected()
    {
        selectionGraphic.SetActive(true);
    }
    public void Deselected()
    {
        selectionGraphic.SetActive(false);
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
