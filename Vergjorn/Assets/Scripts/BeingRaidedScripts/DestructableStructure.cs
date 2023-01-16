using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableStructure : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public Transform[] standPoints;
    int index;

    public bool isDead;

    ProgressBar bar;

    public float rebuildWoodCost;
    public float rebuildMetalCost;

    public StructurePrice structurePrice;

    StructureData myData;
    private void Start()
    {
        structurePrice = GetComponent<GivePriceInfo>().structurePrice;

        rebuildWoodCost = structurePrice.woodCost;
        rebuildMetalCost = structurePrice.metalCost;

        currentHealth = maxHealth;

        

        myData = GetComponent<Structure>().myData;

        if (myData.dead)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    public Vector3 StandPoint()
    {
        Vector3 pos = standPoints[index].position;

        if(index >= standPoints.Length)
        {
            index = 0;
        }
        return pos;

    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if(bar == null)
        {
            bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.5f);
        }

        bar.bar.fillAmount = currentHealth / maxHealth;
        if(currentHealth <= 0)
        {
            Destroy(bar.gameObject);
            NotificationManager.Instance.OpenNotification("Building destroyed!", "Your " + structurePrice.structureName + " has been destroyed, and needs to be rebuilt!", 5f, structurePrice.structureIcon);
            isDead = true;
        }
    }

    public void Rebuild()
    {
        NotificationManager.Instance.OpenNotification("Building repaired!", "Your " + structurePrice.structureName + " has been fixed!", 5f, structurePrice.structureIcon);
        //GetComponent<BuildingStructures>().Unbuild();
        isDead = false;
        myData.dead = false;
    }

}
