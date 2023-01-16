using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RebuildManager : MonoBehaviour
{
    public FloatVariable wood;

    public FloatVariable metal;

    public static RebuildManager Instance;

    bool menuOpen;
    public GameObject menuGO;
    DestructableStructure currentStructure;

    public float woodCost;
    public float metalCost;

    public TextMeshProUGUI woodCostText;
    public TextMeshProUGUI metalCostText;

    public TextMeshProUGUI structureNameText;

    public Image icon;

    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CloseMenu();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit();
        }

        if (menuOpen)
        {
            SetInfo();
        }
    }

    void SetInfo()
    {
        woodCost = currentStructure.rebuildWoodCost;
        metalCost = currentStructure.rebuildMetalCost;

        woodCostText.SetText(woodCost.ToString());
        metalCostText.SetText(metalCost.ToString());

        structureNameText.SetText(currentStructure.structurePrice.structureName);

        icon.sprite = currentStructure.structurePrice.structureIcon;
    }
    void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            DestructableStructure s = hit.collider.GetComponent<DestructableStructure>();
            if(s != null)
            {
                if(s.isDead == true)
                {
                    if (!menuOpen && !MouseOverUI())
                    {
                        OpenMenu();
                    }

                    currentStructure = s;
                }
            }
            else
            {
                if (!MouseOverUI())
                {
                    if (menuOpen)
                    {
                        CloseMenu();
                    }
                }
                
            }
        }
        else
        {
            if (!MouseOverUI())
            {
                if (menuOpen)
                {
                    CloseMenu();
                }
            }
            
        }
    }

    public void OpenMenu()
    {
        menuOpen = true;
        menuGO.SetActive(true);
    }

    public void CloseMenu()
    {
        menuOpen = false;
        menuGO.SetActive(false);
    }

    public void RebuildCurrent()
    {
        if (CanAffordRebuild())
        {
            wood.value -= woodCost;
            metal.value -= metalCost;

            currentStructure.Rebuild();

            currentStructure = null;

            CloseMenu();
        }
    }

    public bool CanAffordRebuild()
    {
       if((wood.value - woodCost) < 0)
        {
            return false;
        }

       if((metal.value - metalCost) < 0)
        {
            return false;
        }

        return true;
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
