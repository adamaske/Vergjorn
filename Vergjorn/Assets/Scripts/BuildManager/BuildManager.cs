using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BuildManager : MonoBehaviour
{
    [Header("UI management")]
    public StructureButton currentStructureButton;

    public TextMeshProUGUI structureName;
    public TextMeshProUGUI structureInfo;
    public TextMeshProUGUI structureWoodCost;
    public TextMeshProUGUI structureMetalCost;
    public TextMeshProUGUI structureGoldCost;
    public Image structureIconImage;

    public Color cantAffordTextColor;
    public Color canAffordTextColor;
    public FloatVariable wood;
    public FloatVariable metal;
    public FloatVariable gold;
   
    public GameObject structureInfoGO;
    public GameObject structureListGO;

    bool isOpen;
    bool infoOpen;

    [Header("Placing")]
    public LayerMask groundLayer;
    public LayerMask structureLayer;
    bool isPlacing;

    GameObject dummyGraphic;
    GameObject currentDummyGraphic;

    public Transform rotationRefrence;
    GameObject currentStructurePrefab;

    public int placeButtonID;
    public int quitPlacingButtonID = 1;

    public bool useGrid;
    public Grid grid;
    public bool closeMenuAfterPlaced;

    [Header("Visuals")]
    public StructureButton[] buttons;
    public Color notUnlockedColor;
    public Color unlockedColor;
    public Color nextUnlockedColor;

    public bool useColors;

    public float checkGroundRange;
    public float checkGroundRadius;

    [Header("Requiering raid text")]
    public TextMeshProUGUI requireRaidText;
    public GameObject requireRaidPanelGO;

    private void Start()
    {
        structureInfoGO.SetActive(false);
        structureListGO.SetActive(false);
        requireRaidPanelGO.SetActive(false);
    }

    private void Update()
    {
        if (useColors && isOpen)
        {
            UpdateUnlocekdVisuals();
        }
        
        if (isPlacing)
        {
            PlaceDummy();
        }
        if (isPlacing && MouseOverUI() && Input.GetMouseButtonDown(placeButtonID))
        {
            QuitPlacing();
        }

        if (isPlacing && Input.GetMouseButtonDown(placeButtonID) && !MouseOverUI())
        {
            Place(GetMouseWorldPos());
        }

        if (isPlacing && (Input.GetMouseButtonDown(quitPlacingButtonID) || Input.GetKeyDown(KeyCode.Escape)))
        {
            QuitPlacing();
        }

        if (infoOpen)
        {
            ColorTextIfCanAfford();
            DisplayStructureInfo();
        }
    }

    #region BuildManagment

    void ColorTextIfCanAfford()
    {       
          float woodAmount = wood.value;
          float metalAMount = metal.value;
        float goldAmount = gold.value;
          float woodCost = currentStructureButton.structureInfo.woodCost;
          float metalCost = currentStructureButton.structureInfo.metalCost;
        float goldCost = currentStructureButton.structureInfo.goldCost;
        if ((woodAmount - woodCost) < 0)
        {
            structureWoodCost.color = cantAffordTextColor;
        }
        else
        {
            structureWoodCost.color = canAffordTextColor;
        }
        if ((metalAMount - metalCost) < 0)
        {
            structureMetalCost.color = cantAffordTextColor;
        }
        else
        {
            structureMetalCost.color = canAffordTextColor;
        }
        if((goldAmount - goldCost) < 0)
        {
            structureGoldCost.color = cantAffordTextColor;
        }
        else
        {
            structureGoldCost.color = canAffordTextColor;
        }
        
    }
    public void StructureSelected(StructureButton but)
    {
        currentStructureButton = but;
        DisplayStructureInfo();
        if (!infoOpen)
        {
            OpenStructureInfo();
        }

        
    }

    public void BuildSelectedStructure()
    {
        if (CanAfford() && CurrentStructureUnlocked())
        {
            GetStructure(currentStructureButton.structurePrefab);
        }
    }

    bool CurrentStructureUnlocked()
    {
        if (currentStructureButton.canPurchaseDespiteElse)
        {
            return true;
        }
        if (currentStructureButton.Unlocked())
        {
            Debug.Log("Structure unlocked");
            return true;
        }
        else
        {
            Debug.Log("Not unlocked");
            return false;
        }
    }
    public void OpenStructureInfo()
    {
        if (!infoOpen)
        {
            structureInfoGO.SetActive(true);
            infoOpen = true;
            DisplayStructureInfo();
        }
        else
        {
            structureInfoGO.SetActive(false);
            infoOpen = false;
            DisplayStructureInfo();
        }
        
    }
    
    public void OpenListOfStructures()
    {
        if (!isOpen)
        {
            structureListGO.SetActive(true);
            isOpen = true;
        }
        else
        {
            if (infoOpen)
            {
                OpenStructureInfo();
            }
            structureListGO.SetActive(false);
            isOpen = false;
        }
    }

    public void DisplayStructureInfo()
    {
        structureName.text = currentStructureButton.structureInfo.structureName;
        structureInfo.text = currentStructureButton.structureInfo.structureInfo;
        structureWoodCost.text = currentStructureButton.structureInfo.woodCost.ToString();
        structureMetalCost.text = currentStructureButton.structureInfo.metalCost.ToString();
        structureGoldCost.text = currentStructureButton.structureInfo.goldCost.ToString();
        structureIconImage.sprite = currentStructureButton.structureInfo.structureIcon;

        if (currentStructureButton.canPurchaseDespiteElse)
        {
            requireRaidPanelGO.SetActive(false);
        }
        else
        {
            if (currentStructureButton.RequiredRaidCompleted())
            {
                requireRaidPanelGO.SetActive(false);
            }
            else
            {
                requireRaidPanelGO.SetActive(true);
                requireRaidText.text = "Requires " + currentStructureButton.requiredRaidDisplayString + " raid to be completed";
            }
        }

       
    }

    public void PurchaseStructure()
    {
        if (currentStructureButton.Unlocked() && CanAfford())
        {
            metal.value -= (currentStructureButton.structureInfo.metalCost);
            wood.value -= (currentStructureButton.structureInfo.woodCost);
            gold.value -= (currentStructureButton.structureInfo.goldCost);
            GetStructure(currentStructureButton.structurePrefab);
            

            
        }
    }
    
    bool CanAfford()
    {
        
        float woodAmount = wood.value;
        float metalAMount = metal.value;
        float goldAmount = gold.value; 

        float woodCost = currentStructureButton.structureInfo.woodCost;
        float metalCost = currentStructureButton.structureInfo.metalCost;
        float goldCost = currentStructureButton.structureInfo.goldCost;

        if ((woodAmount - woodCost) < 0)
        {
            return false;
        }
        if ((metalAMount - metalCost) < 0)
        {
            return false;
        }
        if((goldAmount - goldCost) < 0)
        {
            return false;
        }

        return true;
    }
    #endregion


    #region Placing
    public bool CanPlace(Vector3 point, GameObject structurePrefab, Quaternion rotation)
    {
        BoxCollider collider = structurePrefab.GetComponent<BoxCollider>();
        Vector3 size = collider.transform.TransformVector(collider.bounds.size);
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        size.z = Mathf.Abs(size.z);


        Collider[] hitColliders = Physics.OverlapBox(point + collider.center, size, rotationRefrence.rotation, structureLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders.Length != 0)
            {
               
                return false;
            }
        }

        if (!NoOverlapAndGround(point))
        {
            Debug.Log("Overlap or didnt hit ground");
            return false;
        }
        return true;
    }
    public bool NoOverlapAndGround(Vector3 point)
    {
        Structure s = currentStructurePrefab.GetComponent<Structure>();
        GameObject go = Instantiate(s.checkOverlapAndGroundParent);
        go.transform.position = point;

        List<Transform> t = new List<Transform>();
        foreach (Transform child in go.transform)
        {
            t.Add(child);
        }

        foreach (Transform to in t)
        {
            Collider[] colls = Physics.OverlapSphere(to.position, checkGroundRadius, structureLayer);
            foreach (Collider coll in colls)
            {
                Structure g = coll.GetComponent<Structure>();
                if (g != null && g != s)
                {
                    Destroy(go);
                    return false;
                }
            }
        }
        foreach (Transform tk in t)
        {
            Vector3 pok = tk.transform.position;
            pok.y += 10;
            RaycastHit hit;
            if (!Physics.Raycast(pok, Vector3.down, out hit, checkGroundRange, groundLayer))
            {
                Destroy(go);
                return false;
            }
        }



        Destroy(go);
        return true;
    }

    void Place(Vector3 point)
    {

        if (CanPlace(point, currentStructurePrefab, rotationRefrence.rotation) && CanAfford() && HitGround())
        {
            UpdateUnlocekdVisuals();
            DestroyDummy();

            point.y = 0;
            PurchaseStructure();
            Instantiate(currentStructurePrefab, point, rotationRefrence.rotation, null);
            
            QuitPlacing();
            if (closeMenuAfterPlaced)
            {
                OpenListOfStructures();
            }
        }
        else

        {

            DestroyDummy();
            QuitPlacing();
        }

        //QuitPlacing();


    }

    void QuitPlacing()
    {
        isPlacing = false;
        DestroyDummy();
        //dummyGraphic = null;
        currentStructurePrefab = null;
    }
    public void GetStructure(GameObject structurePrefab)
    {
        
        currentStructurePrefab = structurePrefab;
        dummyGraphic = structurePrefab.GetComponent<StructureGraphic>().thisGraphic;
        isPlacing = true;

        //SpawnDummy
        SpawnDummy();
    }

    bool HitGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, groundLayer))
        {
            if (useGrid)
            {
              
                return grid.GetNearestAllowedPoint(hit.point);
            }
            else
            {
               
                return hit.point;
            }

        }
        return Vector3.zero;
    }

    void PlaceDummy()
    {
        Vector3 pos = GetMouseWorldPos();
        pos.y = 0;
        currentDummyGraphic.transform.position = pos;
    }

    void SpawnDummy()
    {
        currentDummyGraphic = Instantiate(dummyGraphic, GetMouseWorldPos(), rotationRefrence.rotation);
    }

    void DestroyDummy()
    {
        Destroy(currentDummyGraphic.gameObject);

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
    #endregion


    #region Visuals

    void UpdateUnlocekdVisuals()
    {
        foreach(StructureButton button in buttons)
        {
            if (button.Unlocked())
            {
                button.GetComponent<Image>().color = unlockedColor;
            }
            else
            {
                button.GetComponent<Image>().color = notUnlockedColor;
            }
        }
    }

    #endregion
}
