using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BabyMaking : MonoBehaviour
{
    public Worker worker1;
    public Worker worker2;

    public bool makingBaby;

    public Transform[] returnPoints;

    public Transform[] standPoints;

    public GameObject workerPrefab;

    public Transform babySpawnPoint;
    float t;
    public float timeForBabyMake;

    public ParticleSystem particleEffect;
    ProgressBar bar;

    public FloatVariable food;
    

    public float babyFoodCost;
    public bool purchaseBabyWithFood;
    public bool waitingForComfirmation;
    public TextMeshProUGUI foodCostText;

    public GameObject comfirmButtonGO;

    public FloatVariable foodCount;
    public BoolVariable useFoodCounter;

    public Color canAffordColor;
    public Color cantAffordColor;
    public TextMeshProUGUI buttonText;
    public Image buttonImage;

    [Header("Audio")]

    public AudioClip makingBabySounds;
    public AudioClip babySpawnSound;
    public bool babySpawnSoundAtPos;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        comfirmButtonGO.SetActive(false);
    }
    private void Update()
    {
        purchaseBabyWithFood = useFoodCounter.value;
        if(worker1 != null && worker2 != null && !makingBaby && worker1.atBabyMakerStandPoint && worker2.atBabyMakerStandPoint)
        {
            if (purchaseBabyWithFood)
            {
                waitingForComfirmation = true;
            }
            else
            {
                waitingForComfirmation = false;
                StartMakingBaby();

            }
            
        }

        if(worker1 == null || worker2 == null)
        {
            waitingForComfirmation = false;
        }
        if (waitingForComfirmation)
        {
            if (CanAffordBaby())
            {
                buttonImage.color = canAffordColor;
                buttonText.text = "Make baby";
            }
            else
            {
                buttonImage.color = cantAffordColor;
                buttonText.text = "Not enough food";
            }

            if (!comfirmButtonGO.activeSelf)
            {
                comfirmButtonGO.SetActive(true);
            }
            foodCostText.text = "Cost: " + babyFoodCost.ToString();
        }
        else
        {
            if (comfirmButtonGO.activeSelf)
            {
                comfirmButtonGO.SetActive(false);
            }
        }

        if (makingBaby)
        {
            if(particleEffect.isPlaying == false)
            {
                particleEffect.Play();
            }
            if(bar == null)
            {
                bar = ProgressBarManager.Instance.GetProgressBar(this.gameObject, 0.6f);
            }

            bar.bar.fillAmount = t / timeForBabyMake;
            if(t < timeForBabyMake)
            {
                t += Time.deltaTime;
            }
            else
            {
                SpawnBaby();
            }
        }
        else
        {
            if (particleEffect.isPlaying == true)
            {
                particleEffect.Stop();
            }
            if (bar != null)
            {
                Destroy(bar.gameObject);
                bar = null;
            }
        }
    }

    public void PurchaseBaby()
    {
        if (waitingForComfirmation)
        {
            if (CanAffordBaby())
            {
                foodCount.value -= babyFoodCost;

                
                StartMakingBaby();
            }
        }
        
    }

    public bool CanAffordBaby()
    {
        if((foodCount.value - babyFoodCost) >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    void StartMakingBaby()
    {
        
        makingBaby = true;
        waitingForComfirmation = false;
        t = 0;
        worker2.forcedIdle = true;
        worker1.forcedIdle = true;
    }

    public void SpawnBaby()
    {
        makingBaby = false;
        FinishedMakingBaby();


        GameObject go = Instantiate(workerPrefab, babySpawnPoint.position, babySpawnPoint.rotation, null);
        Worker baby = go.GetComponent<Worker>();
        WorkerData data = baby.myData;

        baby.workerType = WorkerType.Child;
        
        if(babySpawnSound != null)
        {
            if (babySpawnSoundAtPos)
            {
                AudioSource.PlayClipAtPoint(babySpawnSound, transform.position, 5f);
            }
            else
            {
                source.PlayOneShot(babySpawnSound, 1f);
            }
        }
        
    }
    public void GetWorker(Worker worker)
    {
        if(worker1 == null && worker != worker2)
        {
            worker1 = worker;
            worker1.babyMakerHasMe = true;
            worker1.GetBabyMaker(this);
            return;
        }
        if(worker2 == null && worker != worker1)
        {
            worker2 = worker;
            worker2.babyMakerHasMe = true;
            worker2.GetBabyMaker(this);
            return;
        }

        
    }


    public void FinishedMakingBaby()
    {
        worker1.forcedIdle = false;
        worker2.forcedIdle = false;

        worker1.GetDestination(returnPoints[0].position);
        worker2.GetDestination(returnPoints[1].position);

        worker1 = null;
        worker2 = null;
    }

    public bool CanMakeBaby()
    {
        if (makingBaby)
        {
            return false;
        }

        if(worker1 != null && worker2 != null)
        {
            return false;
        }

        if(worker1 == null || worker2 == null)
        {
            return true;
        }

        return true;
    }

    public bool EnoughFoodForBaby()
    {
        if (purchaseBabyWithFood)
        {
            return true;
        }
        

        if((food.value + FoodUpkeep.Instance.foodUsedPerWorker) <= food.capacity)
        {
            return true;
        }
        else
        {
            NotificationManager.Instance.OpenNotification("Not enough food", "You dont have enough food for making babies!", 5f, null);
            return false;
        }


    }

    public Vector3 MyStandPoint(Worker w)
    {
        if(w == worker1)
        {
            return standPoints[0].position;
        }

        if(w == worker2)
        {
            return standPoints[1].position;
        }

        return Vector3.zero;
    }

    public void RemoveWorker(Worker w)
    {
        if(w == worker1)
        {
            worker1.forcedIdle = false;
            worker1 = null;
        }
        if (w == worker2)
        {
            worker2.forcedIdle = false;
            worker2 = null;
        }
    }
}
