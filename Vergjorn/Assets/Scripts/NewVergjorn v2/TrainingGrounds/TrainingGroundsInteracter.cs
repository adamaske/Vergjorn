using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class TrainingGroundsInteracter : MonoBehaviour
{
    public TrainingGrounds trainingGrounds;

    public GameObject panelGO;

    public bool menuOpen;

    public GameObject comfirmGO;
    public GameObject progressBarGO;
    public Image progressBar;

    public TextMeshProUGUI goldCostText;

    public GameObject infoGO;

    public TextMeshProUGUI workerInfoText;

    public FloatVariable gold;

    public Color cantAffordColor;
    public Color canAffordColor;
    private void Start()
    {
        CloseMenu();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !MouseOverUI())
        {
            CheckHit();
        }

        if (menuOpen)
        {
            SetInfo();
        }
    }

    public bool CanAfford()
    {
        if((gold.value - trainingGrounds.trainVikingGoldCost) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void SetInfo()
    {


        if(trainingGrounds.training == false && trainingGrounds.waitingForComfirmation == false)
        {
            infoGO.SetActive(true);
        }
        else
        {
            infoGO.SetActive(false);
        }

        if (CanAfford())
        {
            goldCostText.color = canAffordColor;
        }
        else
        {
            goldCostText.color = cantAffordColor;
        }
        goldCostText.SetText(trainingGrounds.trainVikingGoldCost.ToString());

        if (trainingGrounds.training)
        {
            progressBarGO.SetActive(true);
            progressBar.fillAmount = trainingGrounds.currentT / trainingGrounds.timeToTrainViking;
            workerInfoText.gameObject.SetActive(true);
            workerInfoText.text = "Currently training " + trainingGrounds.currentWorker.myName.nameString;
        }
        else
        {
            workerInfoText.gameObject.SetActive(false);
            progressBarGO.SetActive(false);
        }

        if (trainingGrounds.waitingForComfirmation)
        {
            comfirmGO.SetActive(true);
        }
        else
        {
            comfirmGO.SetActive(false);
        }
    }

    public void UpgradeTrainingGrounds()
    {

    }

    public void Comfirm()
    {
        if (CanAfford())
        {
            gold.value -= trainingGrounds.trainVikingGoldCost;
            trainingGrounds.ComfirmTraining();
        }
        else
        {
            NotificationManager.Instance.OpenNotification("No gold", "You dont have enough gold", 3, null);
        }
        
    }
    void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            TrainingGrounds tg = hit.collider.GetComponent<TrainingGrounds>();
            if(tg != null)
            {
                trainingGrounds = tg;
                if(!menuOpen)
                {
                    OpenMenu();
                }
            }
            else
            {
                if(!MouseOverUI() && menuOpen)
                {
                    CloseMenu();
                }
            }
        }
        else
        {
            if(!MouseOverUI() && menuOpen)
            {
                CloseMenu();
            }
        }
    }

    public void OpenMenu()
    {
        menuOpen = true;
        panelGO.SetActive(true);
    }

    public void CloseMenu()
    {
        menuOpen = false;
        panelGO.SetActive(false);
    }

    bool MouseOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
