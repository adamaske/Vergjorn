using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidStructureSpawner : MonoBehaviour
{
    public GameObject graphicPrefab;

    public GameObject parent;
    public RectTransform parentT;
    public RectTransform background;

    public Vector2 backgroundDiff;

    float amountSpawned = 0;

    public bool useDoubleCheck;
    public List<RaidTask> spawnedTasks = new List<RaidTask>();
    private void Start()
    {
        Vector2 newS = parentT.sizeDelta;
        newS.x = 0;
    }
    public void AddRaidTaskGraphic(RaidTask t)
    {
        if (useDoubleCheck)
        {
            if (spawnedTasks.Contains(t))
            {
                return;
            }
        }
        amountSpawned += 1;
     

        GameObject go = Instantiate(graphicPrefab, parent.transform);
        
        RaidStructureGraphic rt = go.GetComponent<RaidStructureGraphic>();
        rt.GetRaidTask(t);
        Vector2 newS = parentT.sizeDelta;
        newS.x = amountSpawned * 100;
        parentT.sizeDelta = newS;
        background.sizeDelta = parentT.sizeDelta + backgroundDiff;

        spawnedTasks.Add(t);
    }
}
