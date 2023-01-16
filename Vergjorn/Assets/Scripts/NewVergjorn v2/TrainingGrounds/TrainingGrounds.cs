using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingGrounds : MonoBehaviour
{
    public List<Worker> workerQueue = new List<Worker>();

    public Transform[] queueSpots;
    public float maxQueueLength;

    public Worker currentWorker;

    public float timeToTrainViking;
    public float currentT;

    public Transform walkOutPoint;
    public Transform trainingStandPoint;

    ProgressBar bar;

    public bool training;

    public bool waitingForComfirmation;

    public float trainVikingGoldCost = 1f;
    private void Update()
    {
        CheckWorkers();

        if(currentWorker == null && workerQueue.Count != 0 && !training)
        {
            waitingForComfirmation = true;
        }
        else
        {
            waitingForComfirmation = false;
        }


        if(currentWorker != null)
        {
            if(Vector3.Distance(currentWorker.transform.position, trainingStandPoint.position) < 0.5f)
            {
                training = true;
            }
            else
            {
                training = false;
            }

            if (training)
            {
                if(bar == null)
                {
                    bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.5f);
                }

                bar.bar.fillAmount = currentT / timeToTrainViking;
                if(currentT < timeToTrainViking)
                {
                    currentT += Time.deltaTime;
                }
                else
                {
                    FinishedTrainingWorker();
                    currentT = 0;
                }
            }
            else
            {
                currentT = 0;
            }
        }
    }
    void CheckWorkers()
    {
        List<Worker> workersToRemove = new List<Worker>();
        if(workerQueue.Count != 0)
        {
            for (int i = 0; i < workerQueue.Count; i++)
            {
                if(workerQueue[i].state != Worker.WorkerState.training)
                {
                    workersToRemove.Add(workerQueue[i]);
                }
            }
            //foreach (Worker w in workerQueue)
            //{
            //    if (w.state != Worker.WorkerState.training)
            //    {
            //        workerQueue.Remove(w);
            //    }
            //}
        }
        
        foreach(Worker w in workersToRemove)
        {
            if (workerQueue.Contains(w))
            {
                workerQueue.Remove(w);
            }
        }
    }


    public void AddWorker(Worker w)
    {
        if (!workerQueue.Contains(w))
        {
            workerQueue.Add(w);
            //Give queue spot
            w.GetTrainingGrounds(this, queueSpots[workerQueue.Count].position);
            
        }
    }

    void StartTrainingWorker()
    {
        Worker w = workerQueue[0];
        currentWorker = w;
        currentWorker.StartTraining(trainingStandPoint.position);
        currentWorker.forcedIdle = true;
        workerQueue.Remove(w);
        GiveNewQueueSpots();
        
    }

    public void ComfirmTraining()
    {
        if (waitingForComfirmation)
        {
            StartTrainingWorker();

            waitingForComfirmation = false;
        }
    }

    void FinishedTrainingWorker()
    {
        currentWorker.forcedIdle = false;
        
        currentWorker.EndTraining(walkOutPoint.position);

        currentWorker = null;
        training = false;
        Destroy(bar.gameObject);
        bar = null;
        GiveNewQueueSpots();
    }
    void GiveNewQueueSpots()
    {
        if(workerQueue.Count == 0)
        {
            return;
        }
        for (int i = 0; i < workerQueue.Count; i++)
        {
            workerQueue[i].GetQueueSpot(queueSpots[i].position);
        }
    }

    public bool CanGetNewWorker()
    {
        if(workerQueue.Count < maxQueueLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
