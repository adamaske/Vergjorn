using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidTask : MonoBehaviour
{
    [Header("Health")]
    public float health;
    public float maxHealth;

    [Header("Gold Giving")]
    public float goldToGiveOnDeath = 5;
    public bool giveGoldOnDeath = true;
    [Space]
    public bool giveGoldOnHit;
    public float goldToGivePerReturn = 1f;

    [Header("Stand Points")]
    public List<Transform> standPoint = new List<Transform>();
    int pointIndex = 0;

    [Header("Progress bar")]
    public bool useHealthProgressBar;
    private ProgressBarManager progressBarManager;
    ProgressBar currentBar;

    public bool dead;
    public GameObject graphic;
    public GameObject brokenGraphic;
    public bool changeGraphics;

    public RaidStructureType myType;
    public RaidStructureData myData;

    //graphic
    public Sprite mySprite;

    [Header("Enemy spawning")]
    public SpawnEnemiesStructure spawnEnemies;
    public bool spawnEnemiesOnFirstHit;
    bool hasSpawned;
    private void Start()
    {
        spawnEnemies = GetComponent<SpawnEnemiesStructure>();
        progressBarManager = ProgressBarManager.Instance;

        RaidManager.Instance.GetRaidTask(this);

        SetGrahic();
    }
    private void Update()
    {
        SetData();
    }
    void SetData()
    {
        myData.position = transform.position;
        myData.rotation = transform.rotation;
        myData.type = myType;
        myData.dead = dead;
        myData.health = health;
    }
   
    public Vector3 GetStandPoint()
    {
        Vector3 pos;
        pos = standPoint[pointIndex].position;
        pointIndex += 1;
        if (pointIndex >= standPoint.Count)
        {
            pointIndex = 0;
        }
        return pos;
    }

    public void Attacked()
    {

    }

    void SetGrahic()
    {
        if (dead)
        {
            brokenGraphic.SetActive(true);
            graphic.SetActive(false);
        }
        else
        {
            graphic.SetActive(true);
            brokenGraphic.SetActive(false);
        }
    }
    public void TakeDamage(Worker unit, float amount)
    {
        
        if (dead)
        {
            return;
        }
        health -= amount;

        
        if (spawnEnemiesOnFirstHit && !hasSpawned)
        {
            hasSpawned = true;
            spawnEnemies.SpawnEnemies();
        }


        if (useHealthProgressBar)
        {
            if (currentBar == null)
            {
                currentBar = progressBarManager.GetProgressBar(this.gameObject, 0.8f);
            }
            currentBar.bar.fillAmount = health / maxHealth;
        }


        

        if (health <= 0)
        {
            if (giveGoldOnDeath)
            {
                GoldNotificationManager.Instance.GiveMeGoldNotification(this.gameObject, goldToGiveOnDeath, 1.5f);
                unit.GetGold(goldToGiveOnDeath, false);
            }
            Die();
        }
    }

    public void DestroyThis()
    {
        RaidManager.Instance.RemoveRaidTask(this);
        Destroy(this.gameObject);
    }

    void Die()
    {
        if(currentBar != null)
        {
            Destroy(currentBar.gameObject);
        }

        
        dead = true;

        SetGrahic();

    }
}
