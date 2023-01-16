using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public GameObject progressBarPrefab;

    public ProgressBar GetProgressBar(GameObject data, float sizeFactor)
    {
        GameObject go = Instantiate(progressBarPrefab, this.transform.position, this.transform.rotation, this.transform);
        ProgressBar pb = go.GetComponent<ProgressBar>();
        pb.GetUnit(data, sizeFactor);
        return pb;


    }
}
