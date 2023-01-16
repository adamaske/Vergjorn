using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingStructures : MonoBehaviour
{
    StructureManager structureManager;

    public bool unbuilt = true;

    public Transform[] standPoints;
    int standPointIndex;
    public float buildGoal;
    public float currentScore;

    public GameObject unbuiltGraphic;
    public GameObject builtGraphic;

    public float baseProductionTime = 30f;
    public float perWorkerMultiplier = 0.9f;
    public float t;

    public NavMeshObstacle obstacle;
    ProgressBar progressBar;

    public List<Worker> workersOnMe = new List<Worker>();

    public bool building;
    //Audio
    private AudioSource source;
    public bool playCompleteSoundAtPosistion = true;
    public float atPosistionVolume;
    public AudioClip completedBuildingClip;
    public AudioClip buildingSounds;
    void Start()
    {
        source = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
        structureManager = StructureManager.Instance;

        if (unbuilt)
        {
            unbuiltGraphic.SetActive(true);
            builtGraphic.SetActive(false);
            structureManager.GetUnbuilt(this);
           
            
        }
        else
        {
            builtGraphic.SetActive(true);
            unbuiltGraphic.SetActive(false);
           
        }
        
    }

    private void Update()
    {
        if(workersOnMe.Count != 0 && unbuilt)
        {
            if(t < ProductionTime())
            {
                t += Time.deltaTime;
            }
            else
            {
                t = 0;
                Built(true);
            }

            if(progressBar == null)
            {
                progressBar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.7f);
            }

            progressBar.bar.fillAmount = t / ProductionTime();

            

            building = true;
        }
        else
        {
            building = false;
        }
        if (building)
        {
            if (!source.isPlaying)
            {
                source.clip = buildingSounds;
                source.Play();
            }
        }
        else
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        
    }

    float ProductionTime()
    {
        float amount = workersOnMe.Count;

        float time = baseProductionTime;

        for (int i = 0; i < amount; i++)
        {
            time *= perWorkerMultiplier;
        }

        return time;
    }
    public void Built(bool playSound)
    {

        unbuilt = false;
        StructureManager.Instance.RemoveUnbuilt(this);
        builtGraphic.SetActive(true);
        unbuiltGraphic.SetActive(false);

        if(progressBar != null)
        {
            Destroy(progressBar.gameObject);
        }

        if(completedBuildingClip != null && playSound)
        {
            if (playCompleteSoundAtPosistion)
            {
                AudioSource.PlayClipAtPoint(completedBuildingClip, transform.position, atPosistionVolume);
            }
            else
            {
                source.PlayOneShot(completedBuildingClip, 1);
            }
          
            
        }

      
        //Destroy(this);
    }

    public Vector3 GetStandPoint()
    {
        Vector3 pos = standPoints[standPointIndex].position;

        standPointIndex += 1;
        if(standPointIndex >= standPoints.Length)
        {
            standPointIndex = 0;
        }

        return pos;
    }

    public void Unbuild()
    {
        currentScore = 0;
        unbuilt = true;

        
    }
    public void GetWorker(Worker w)
    {
        if (!workersOnMe.Contains(w))
        {
            workersOnMe.Add(w);
        }
    }
    public void RemoveWorker(Worker w)
    {
        if (workersOnMe.Contains(w))
        {
            workersOnMe.Remove(w);
        }
    }
}
