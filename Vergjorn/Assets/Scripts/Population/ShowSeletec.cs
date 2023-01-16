using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSeletec : MonoBehaviour
{
    
    public Sprite selectedSprite;

    public List<Worker> workers = new List<Worker>();


    public void Deselected()
    {
        
        foreach (Worker workerx in workers)
        {
           
            workerx.GetComponentInChildren<Renderer>().material.color = Color.white;
        }
    }

    public void UpdateSelection(List<Worker> worker)
    {
        workers = worker;

        foreach(Worker workerx in workers)
        {
            workerx.GetComponentInChildren<Renderer>().material.color = Color.blue;
        }
    }

}
