using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyUnit : MonoBehaviour
{
    [Header("Health")]
    public float health;
    [Space]

    [Header("Killing vikings")]
    public Worker vikingUnit;

    public bool inBattle;
    float attackT;
    public bool attacking;
    public float attackSpeed;
    public float attackDamage;

    private RaidManager raidManager;
    private NavMeshAgent agent;

    ProgressBar myProgressBar;
    public float rotateSpeed = 100;

    public bool isDead;
    public bool destroyOnDeath = true;
    private void Start()
    {
        raidManager = RaidManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        raidManager.GetEnemyUnit(this);
    }

    private void Update()
    {
        
        if(vikingUnit == null || vikingUnit.health <= 0)
        {
            inBattle = false;
            GetViking();
        }
       

        if (inBattle && vikingUnit != null)
        {
            if(Vector3.Distance(transform.position, vikingUnit.transform.position) > 1.5f)
            {
                agent.isStopped = false;
                agent.SetDestination(vikingUnit.transform.position);
                attacking = false;
                attackT = 0;
            }
            else
            {
                agent.ResetPath();
                agent.isStopped = true;
                attacking = true;
            }

            if (attacking)
            {
                RotateTowards(vikingUnit.transform.position, -90);
                if (attackT < attackSpeed)
                {
                    attackT += Time.deltaTime;
                }
                else
                {
                    attackT = 0;
                    AttackViking();
                }
            }

            //Look at viking
            
            
        }
    }
    public void RotateTowards(Vector3 target, float yOffset)
    {
        Vector3 vectorToTarget = target - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - yOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }
    void GetViking()
    {
        
        List<Worker> vikings = new List<Worker>();

        for (int i = 0; i < RaidManager.Instance.vikings.Count; i++)
        {
            if(RaidManager.Instance.vikings[i].health > 0)
            {
                vikings.Add(RaidManager.Instance.vikings[i]);
            }
        }

        if (vikings.Count == 0)
        {
            
            return;

        }
        


        Worker unit = vikings[0];

        for (int i = 0; i < vikings.Count; i++)
        {
            
            float dist = Vector3.Distance(transform.position, vikings[i].transform.position);
            if (dist < Vector3.Distance(unit.transform.position, transform.position))
            {
                unit = vikings[i];
            }
        }

        vikingUnit = unit;
        inBattle = true;
    }

    void AttackViking()
    {
        vikingUnit.TakeDamage(this, attackDamage);
    }
    public void TakeDamage(float amount, Worker unit)
    {
        health -= amount;
        if (myProgressBar == null)
        {
            myProgressBar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.5f);
        }
        if(unit != null)
        {
            DamageNotificationManager.Instance.GiveMeDamageNotification(this.gameObject, amount, 0.3f);
        }

        myProgressBar.bar.fillAmount = health / 100;
        if (health <= 0)
        {
            unit.KilledEnemy();
            Die();
        }
    }

    public void Die()
    {
        Destroy(myProgressBar.gameObject);
        
        isDead = true;
        if (destroyOnDeath)
        {
            raidManager.RemoveEnemyUnit(this);
            Destroy(gameObject);
        }
        
    }
}
