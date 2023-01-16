using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrovn : MonoBehaviour
{
    private StructureManager structureManager;

    public Transform standPointsParent;
    public List<Transform> standPoints;
    int standPointIndex;

    public List<Worker> workersAtMe;

    public bool producing;

    public float baseProduceTime;
    public float perWorkerMultiplier = 0.9f;
    float t;
    public float metalProducePer;

    public FloatVariable metal;
    public FloatVariable myrmalm;

    ProgressBar bar;

    [Header("Audio")]
    public AudioClip workingSounds;
    public AudioClip finsihedAMyrmalmClip;
    public bool playCompleteAtPosistion;
    private AudioSource source;
    void Start()
    {
        structureManager = StructureManager.Instance;
        source = GetComponent<AudioSource>();
        StructureManager.Instance.GetMyrovn(this);

        foreach(Transform child in standPointsParent)
        {
            standPoints.Add(child);
        }
    }

    private void Update()
    {
        if(workersAtMe.Count > 0 && EnoughMyrmalmToMakeMetal())
        {
            producing = true;
            
        }
        else
        {
            producing = false;
            t = 0;
        }

        if (producing)
        {
            if(bar == null)
            {
                bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.4f);
            }
            bar.bar.fillAmount = t / ProduceTime();
            if(t < ProduceTime())
            {
                t += Time.deltaTime;
            }
            else
            {
                ProduceMetal();
                t = 0;
            }

            if(workingSounds != null)
            {
                if (!source.isPlaying)
                {
                    source.clip = workingSounds;
                    source.Play();
                }
            }
            
        }
        else
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
            if(bar != null)
            {
                Destroy(bar.gameObject);
                bar = null;
            }
        }
    }

    public bool EnoughMyrmalmToMakeMetal()
    {
        if(myrmalm.value > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float ProduceTime()
    {
        float amount = workersAtMe.Count;

        float time = baseProduceTime;

        for (int i = 0; i < amount; i++)
        {
            time *= perWorkerMultiplier;
        }
        return time;
    }
    public void ProduceMetal()
    {
        myrmalm.value -= 1f;
        
        if(finsihedAMyrmalmClip != null)
        {
            if (playCompleteAtPosistion)
            {
                AudioSource.PlayClipAtPoint(finsihedAMyrmalmClip, transform.position, 5f);
            }
            else
            {
                source.PlayOneShot(finsihedAMyrmalmClip, 1f);
            }
        }

        metal.value += metalProducePer;
    }
    public Vector3 WorkingAtmyrovnStandPoint()
    {
        return Vector3.zero;
    }
    
    public Vector3 GetStandPoint()
    {
        Vector3 pos = standPoints[standPointIndex].position;

        standPointIndex += 1;
        if(standPointIndex >= standPoints.Count)
        {
            standPointIndex = 0;
        }

        return pos;
    }

    public void GetWorker(Worker worker)
    {
        if (!workersAtMe.Contains(worker))
        {
            workersAtMe.Add(worker);
        }
    }

    public void RemoveWorker(Worker worker)
    {
        if (workersAtMe.Contains(worker))
        {
            workersAtMe.Remove(worker);
        }
    }
}
