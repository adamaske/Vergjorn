using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class ShipyardInteracter : MonoBehaviour
{
    public Shipyard currentShipyard;

    public GameObject tab;
    bool menuOpen;
    //TextMeshes
    [Header("TextMeshes")]
    public TextMeshProUGUI levelText;
    [Space]
    public GameObject upgradingGO;
    public TextMeshProUGUI upgradeGoldCost;
    public TextMeshProUGUI upgradeWoodCost;
    public TextMeshProUGUI upgradeMetalCost;
    [Space]
    public GameObject productionGO;
    public Transform noUpgradePos;
    public Transform upgradePos;
    public TextMeshProUGUI buildGoldCost;
    public TextMeshProUGUI buildWoodCost;
    public TextMeshProUGUI buildMetalCost;

    public int clickButton = 0;

    public FloatVariable gold;
    public FloatVariable wood;
    public FloatVariable metal;

    public Color canAffordColor;
    public Color cantAffordColor;

    public bool checkForDead;

    public bool freeProdction;
    public bool freeUpgrading;
    private void Start()
    {
        CloseShipyardMenu();
    }
    //Info
    private void Update()
    {
        if (Input.GetMouseButtonDown(clickButton))
        {
            CheckForShipyardHit();
        }

        if (menuOpen)
        {
            SetTextes();
        }
    }

    void CheckForShipyardHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000))
        {
            Shipyard s = hit.collider.GetComponent<Shipyard>();
            if(s != null)
            {
                if (!MosueOverUI())
                {
                    if (checkForDead)
                    {
                        if(s.GetComponent<DestructableStructure>().isDead == true)
                        {
                            CloseShipyardMenu();
                        }
                        else
                        {
                            OpenShipyardMenu(s);
                        }
                    }
                    else
                    {
                        OpenShipyardMenu(s);
                    }
                    
                }
               
            }
            else
            {
                if (!MosueOverUI())
                {
                    CloseShipyardMenu();
                }
                
            }
        }

        if (menuOpen)
        {
            SetTextes();
        }
    }

    void CloseShipyardMenu()
    {
        tab.SetActive(false);
        currentShipyard = null;

        menuOpen = false;
    }
    void OpenShipyardMenu(Shipyard s)
    {
        currentShipyard = s;

        tab.SetActive(true);

        SetTextes();

        menuOpen = true;
    }

    public void UpgradeShip()
    {
        if (freeUpgrading)
        {
            currentShipyard.UpgradeShipyard();
            return;
        }
        if (CanAffordShipUpgrade())
        {
            wood.value -= currentShipyard.currentShipyardLevel.woodUpgradeCost;
            metal.value -= currentShipyard.currentShipyardLevel.metalUpgradeCost;
            gold.value -= currentShipyard.currentShipyardLevel.goldUpgradeCost;
            currentShipyard.UpgradeShipyard();
        }
        
    }

    public void ProduceShip()
    {
        if (freeProdction)
        {
            currentShipyard.StartProducing();
            return;
        }
        if (CanAffordShipProduction())
        {
            wood.value -= currentShipyard.currentShipyardLevel.woodShipProduceCost;
            metal.value -= currentShipyard.currentShipyardLevel.metalShipProduceCost;
            gold.value -= currentShipyard.currentShipyardLevel.goldShipProduceCost;

            currentShipyard.StartProducing();

        }
        else
        {
            NotificationManager.Instance.OpenNotification("Unsufficent funds", "You dont have enough resources to build that ship", 5f, null);
        }


    }
    public bool CanAffordShipUpgrade()
    {
        if (!CanAfford(gold.value, currentShipyard.currentShipyardLevel.goldShipProduceCost))
        {
            return false;
        }
        if (!CanAfford(metal.value, currentShipyard.currentShipyardLevel.metalShipProduceCost))
        {
            return false;
        }
        if (!CanAfford(wood.value, currentShipyard.currentShipyardLevel.woodShipProduceCost))
        {
            return false;
        }

        return true;
    }

    public bool CanAffordShipProduction()
    {
        if (!CanAfford(gold.value, currentShipyard.currentShipyardLevel.goldShipProduceCost))
        {
            return false;
        }
        if (!CanAfford(metal.value, currentShipyard.currentShipyardLevel.metalShipProduceCost))
        {
            return false;
        }
        if (!CanAfford(wood.value, currentShipyard.currentShipyardLevel.woodShipProduceCost))
        {
            return false;
        }

        return true;
    }
    void SetTextes()
    {
        levelText.text = currentShipyard.currentShipyardLevel.levelName;

        if (currentShipyard.canBeUpgraded)
        {
            upgradingGO.SetActive(true);
            productionGO.transform.position = upgradePos.position;
        }
        else
        {
            productionGO.transform.position = noUpgradePos.position;
            upgradingGO.SetActive(false);
        }

        upgradeGoldCost.SetText(currentShipyard.currentShipyardLevel.goldUpgradeCost.ToString());
        upgradeWoodCost.SetText(currentShipyard.currentShipyardLevel.woodUpgradeCost.ToString());
        upgradeMetalCost.SetText(currentShipyard.currentShipyardLevel.metalUpgradeCost.ToString());

        //
        buildGoldCost.SetText(currentShipyard.currentShipyardLevel.goldShipProduceCost.ToString());
        buildWoodCost.SetText(currentShipyard.currentShipyardLevel.woodShipProduceCost.ToString());
        buildMetalCost.SetText(currentShipyard.currentShipyardLevel.metalShipProduceCost.ToString());

        //set text
        #region Production color
        if (CanAfford(gold.value, currentShipyard.currentShipyardLevel.goldShipProduceCost))
        {
            buildGoldCost.color = canAffordColor;
        }
        else
        {
            buildGoldCost.color = cantAffordColor;
        }

        if (CanAfford(metal.value, currentShipyard.currentShipyardLevel.metalShipProduceCost))
        {
            buildMetalCost.color = canAffordColor;
        }
        else
        {
            buildMetalCost.color = cantAffordColor;
        }

        if (CanAfford(wood.value, currentShipyard.currentShipyardLevel.woodShipProduceCost))
        {
            buildWoodCost.color = canAffordColor;
        }
        else
        {
            buildWoodCost.color = cantAffordColor;
        }
        #endregion

        #region Upgrade color
        if (CanAfford(gold.value, currentShipyard.currentShipyardLevel.goldShipProduceCost))
        {
            buildGoldCost.color = canAffordColor;
        }
        else
        {
            buildGoldCost.color = cantAffordColor;
        }

        if (CanAfford(metal.value, currentShipyard.currentShipyardLevel.metalShipProduceCost))
        {
            buildMetalCost.color = canAffordColor;
        }
        else
        {
            buildMetalCost.color = cantAffordColor;
        }
        if (CanAfford(wood.value, currentShipyard.currentShipyardLevel.woodShipProduceCost))
        {
            buildWoodCost.color = canAffordColor;
        }
        else
        {
            buildWoodCost.color = cantAffordColor;
        }
        #endregion
    }
    public bool CanAfford(float amount, float cost)
    {
        if ((amount - cost) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    bool MosueOverUI()
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
