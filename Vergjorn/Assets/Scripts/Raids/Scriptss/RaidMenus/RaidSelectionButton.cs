using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RaidSelectionButton : MonoBehaviour
{
    public enum ButtonType { ship, worker}

    public ButtonType type;

    public WorkerData myWorker;
    public Ship myShip;

    public bool iAmSelected;

    public Image myImage;
    public Sprite workerSprite;
    public Sprite shipSprite;

    public Image checkMark;

    public TextMeshProUGUI nameText;
    private void Start()
    {
        iAmSelected = true;
        Pressed();
    }
    public void GetType(ButtonType t)
    {
        type = t;
    }

    public void GetWorkerData(WorkerData w)
    {
        myWorker = w;

        myImage.sprite = workerSprite;
        type = ButtonType.worker;

        nameText.SetText(myWorker.workerName);
    }

    public void GetShipData(Ship s)
    {
        myShip = s;
        nameText.SetText("");
        myImage.sprite = shipSprite;
        type = ButtonType.ship;
    }

    public void Pressed()
    {
        if (!iAmSelected)
        {
            if (type == ButtonType.ship)
            {
                RaidInfoManager.Instance.SelectShip(myShip);
            }
            if (type == ButtonType.worker)
            {
                RaidInfoManager.Instance.SelectWorker(myWorker);
            }
            iAmSelected = true;
            checkMark.gameObject.SetActive(true);
        }
        else
        {
            if (type == ButtonType.ship)
            {
                RaidInfoManager.Instance.DeselectShip(myShip);
            }
            if (type == ButtonType.worker)
            {
                RaidInfoManager.Instance.DeselectWorker(myWorker);
            }
            iAmSelected = false;
            checkMark.gameObject.SetActive(false);
        }
        
    }
}
