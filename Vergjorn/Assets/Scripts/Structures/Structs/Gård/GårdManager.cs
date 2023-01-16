using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class GårdManager : MonoBehaviour
{
    public Gård currentGård;
    public GameObject gårdInfo;

    public TextMeshProUGUI gårdDescText;

    public TextMeshProUGUI levelText;

   
    public TextMeshProUGUI animalsText;
    public TextMeshProUGUI animalFoodPerDay;

    public TextMeshProUGUI timeToGatherText;

    public TextMeshProUGUI woodCostText;
    public TextMeshProUGUI metalCostText;

    public TextMeshProUGUI daysToCullingText;
    public TextMeshProUGUI expectedReturnText;


    public string gårdDesc;

    bool open;
    
    public Vector3 offset;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            HitGård();
        }
        if (open)
        {
            
            gårdInfo.SetActive(true);
            DisplayInfo();
        }
        else
        {
            gårdInfo.SetActive(false);
        }
    }

    void HitGård()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Gård gård = hit.collider.gameObject.GetComponent<Gård>();
            if (gård != null)
            {
                currentGård = gård;
                open = true;
            }
           
            if(gård == null && !MouseOverUI())
            {
                open = false;
            }
            
        }
    }
    void DisplayInfo()
    {
        PlaceDisplay();
        gårdDescText.text = gårdDesc;
        animalsText.text = currentGård.currentAnimal.ToString() + " / " + currentGård.currentGårdLevel.animalCapacity.ToString();
        levelText.text = currentGård.currentGårdLevel.levelName + " / " + currentGård.gårdLevels.Length.ToString();
        timeToGatherText.text = currentGård.currentGårdLevel.timeToGather.ToString() ;
        animalFoodPerDay.text = "Food to animal per day: " + currentGård.currentGårdLevel.animalFoodCostPerDay.ToString();
        woodCostText.text = currentGård.currentGårdLevel.woodCostToUpgrade.ToString();
        metalCostText.text = currentGård.currentGårdLevel.metalCostToUpgrade.ToString();
        daysToCullingText.text = "Days left to culling: " + currentGård.daysLeftToCulling.ToString();
        expectedReturnText.text = "Expected return: " + (currentGård.foodSpentForCulling * currentGård.currentGårdLevel.cullingReturnPerSpent).ToString();

    }

    void PlaceDisplay()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(currentGård.transform.position);
        pos += offset;

        gårdInfo.transform.position = pos;
    }

    public void UpgradeGård()
    {
        currentGård.UpgradeGård();
    }

    public void AqquireAnimals()
    {
        currentGård.AqquireAnimals();
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
