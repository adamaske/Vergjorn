using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class LoggingHutManager : MonoBehaviour
{
    public static LoggingHutManager Instance;
    
    public LoggingHut currentLoggingHut;
    public GameObject LoggingHutInfo;


    public TextMeshProUGUI levelText;

    public TextMeshProUGUI currentFoodCapacityBonusText;

    public TextMeshProUGUI upgradeWoodCostText;
    public TextMeshProUGUI upgradeMetalCostText;
    bool open;

    public Vector3 offset;

    private void Start()
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

    void DisplayInfo()
    {
        PlaceDisplay();
        levelText.text = currentLoggingHut.currentLoggingHutLevel.levelName + " / " + currentLoggingHut.LoggingHutLevels.Length.ToString();

        currentFoodCapacityBonusText.text = "Current metal capacity bonus: " + currentLoggingHut.currentLoggingHutLevel.capacityBonus.ToString();


        upgradeMetalCostText.text = currentLoggingHut.currentLoggingHutLevel.woodUpgradeCost.ToString();
        upgradeWoodCostText.text = currentLoggingHut.currentLoggingHutLevel.metalUpgradeCost.ToString();


    }

    void PlaceDisplay()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(currentLoggingHut.transform.position);
        pos += offset;

        LoggingHutInfo.transform.position = pos;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HitShip();
        }
        if (open)
        {
            LoggingHutInfo.SetActive(true);
            DisplayInfo();
        }
        else
        {
            LoggingHutInfo.SetActive(false);
        }
    }
    void HitShip()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            LoggingHut ship = hit.collider.gameObject.GetComponent<LoggingHut>();
            if (ship != null)
            {
                currentLoggingHut = ship;
                open = true;
            }
            if (ship == null && MouseOverUI() && open)
            {
                open = true;
            }
            else if (ship == null && !MouseOverUI())
            {
                open = false;
            }

        }
    }


    public void UpgradeLoggingHut()
    {
        currentLoggingHut.UpgradeLoggingHut();
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



