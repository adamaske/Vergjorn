using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SmieButton : MonoBehaviour
{
    public WeaponType thisType;
    public Image mySprite;

    public Smie currentSmie;

    public TextMeshProUGUI myGoldCost;
    public TextMeshProUGUI myMetalCost;

    public Color canAffordColor;
    public Color cantAffordColor;

    public FloatVariable gold;
    public FloatVariable metal;
    private void Start()
    {
        //GetComponent<Button>().onClick.AddListener(Pressed);
        Debug.Log("Added listener");
    }


    private void Update()
    {
        if(currentSmie != null)
        {
            SetCostText();
        }
       
    }
    void SetCostText()
    {
        if(thisType == WeaponType.shield)
        {
            myGoldCost.SetText(currentSmie.currentSmieLevel.shieldGold.ToString());
            myMetalCost.SetText(currentSmie.currentSmieLevel.shieldMetal.ToString());
        }

        if (thisType == WeaponType.sword)
        {
            myGoldCost.SetText(currentSmie.currentSmieLevel.swordGold.ToString());
            myMetalCost.SetText(currentSmie.currentSmieLevel.swordMetal.ToString());
        }

        if (thisType == WeaponType.helmet)
        {
            myGoldCost.SetText(currentSmie.currentSmieLevel.helmetGold.ToString());
            myMetalCost.SetText(currentSmie.currentSmieLevel.helmetMetal.ToString());
        }

        if (currentSmie.CanAffordWeaponGoldProduction(thisType))
        {
            myGoldCost.color = canAffordColor;
        }
        else
        {
            myGoldCost.color = cantAffordColor;
        }

        if (currentSmie.CanAffordWeaponMetalProduction(thisType))
        {
            myMetalCost.color = canAffordColor;
        }
        else
        {
            myMetalCost.color = cantAffordColor;
        }
    }

    public bool CanAfford(float amount, float cost)
    {
        if((amount - cost) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void Pressed()
    {
        SmieInteracter.Instance.ProduceWeapon(thisType);
        
    }

    public void GetSprite(Sprite s)
    {
        mySprite.sprite = s;
    }
}
