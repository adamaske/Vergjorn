using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableFood : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    public List<Worker> workers = new List<Worker>();

    public Transform[] standPoints;
    public int index;

    ProgressBar bar;
    public float foodFromThis;

    public GatheringArea gatheringArea;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void GetGatheringArea(GatheringArea g)
    {
        gatheringArea = g;
    }
    public void GetWorker(Worker w)
    {
        if (!workers.Contains(w))
        {
            workers.Add(w);
        }
    }
    public void RemoveWorker(Worker w)
    {
        if (workers.Contains(w))
        {
            
            workers.Remove(w);
        }
        if(workers.Count == 0 && bar != null)
        {
            index = 0;
            Destroy(bar.gameObject);
            bar = null;
        }
    }
    public void TakeDamage(float amount, Worker w)
    {
        currentHealth -= amount;

        if (bar == null)
        {
            Debug.Log("Spawned progessbar");
            bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.5f);
        }
        bar.bar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            w.GatheredFood(foodFromThis);

            currentHealth = maxHealth;
            if(bar != null)
            {
                Destroy(bar.gameObject);
            }
            
        }
    }

    public Vector3 GetStandPoint()
    {
        Vector3 p = standPoints[index].position;


        index += 1;
        if (index >= standPoints.Length)
        {
            index = 0;
        }

        return p;
    }
}
