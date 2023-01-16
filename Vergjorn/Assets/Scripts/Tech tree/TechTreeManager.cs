using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TechTreeManager : MonoBehaviour
{
    public StructureTreeButton[] buttons;
    StructureTreeButton currentStructureButton;

    public TextMeshProUGUI structureName;
    public TextMeshProUGUI structureInfo;
    public TextMeshProUGUI structureWoodCost;
    public TextMeshProUGUI structureMetalCost;

    public GameObject treeObject;

    public StructurePlacer structurePlacer;

    public bool testIgnorePrice;

    public FloatVariable wood;
    public FloatVariable metal;
    private void Start()
    {
        //HideNotPlaceable();
        treeObject.SetActive(false);
    }

   

    public void StructureSelected(StructureTreeButton structureTree)
    {
        treeObject.SetActive(true);
        
        currentStructureButton = structureTree;
        DisplayStructureInfo();
    }

    public void CloseMenu()
    {
        treeObject.SetActive(false);
        
    }


    void DisplayStructureInfo()
    {
        structureName.text = currentStructureButton.structureInfo.structureName;
        structureInfo.text = currentStructureButton.structureInfo.structureInfo;
        structureWoodCost.text = currentStructureButton.structureInfo.woodCost.ToString();
        structureMetalCost.text = currentStructureButton.structureInfo.metalCost.ToString();
    }


    public void PurchaseStructure()
    {
        if (currentStructureButton.CanPurchase() && CanAfford())
        {
            metal.value -=(currentStructureButton.structureInfo.metalCost);
            wood.value -= (currentStructureButton.structureInfo.woodCost);

            structurePlacer.GetStructure(currentStructureButton.structurePrefab);
            

            //HideNotPlaceable();
        }
    }
    #region Can Afford
    bool CanAfford()
    {
        if (testIgnorePrice)
        {
            return true;
        }
        float woodAmount = wood.value;
        float metalAMount = metal.value;

        float woodCost = currentStructureButton.structureInfo.woodCost;
        float metalCost = currentStructureButton.structureInfo.metalCost;

        if((woodAmount - woodCost) < 0)
        {
            return false;
        }
        if((metalAMount - metalCost) < 0)
        {
            return false;
        }

        return true;
    }

    
    #endregion

    void HideNotPlaceable()
    {
        foreach(StructureTreeButton button in buttons)
        {
            if(button.Unlocked() == false)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
            }
        }
    }
}
