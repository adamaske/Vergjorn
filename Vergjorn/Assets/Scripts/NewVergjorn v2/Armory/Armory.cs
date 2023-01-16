using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour
{
    public Transform walkPoint;

    public Transform walkBackPoint;

    public void GiveMeWeapons(Worker w)
    {
        bool needSword = !w.myData.hasSword;
        bool needShield = !w.myData.hasShield;
        bool needHelmet = !w.myData.hasHelmet;

        if(needSword && WeaponsManager.Instance.swords.Count != 0)
        {
            w.GetWeapon(WeaponType.sword);
            WeaponsManager.Instance.RemoveWeapon(WeaponType.sword);
        }
        if (needShield && WeaponsManager.Instance.shields.Count != 0)
        {
            w.GetWeapon(WeaponType.shield);
            WeaponsManager.Instance.RemoveWeapon(WeaponType.shield);
        }
        if (needHelmet && WeaponsManager.Instance.helmets.Count != 0)
        {
            w.GetWeapon(WeaponType.helmet);
            WeaponsManager.Instance.RemoveWeapon(WeaponType.helmet);
        }
    }
    public bool HasWeaponsForThisWorker(Worker w)
    {
        bool tempBool = true;

        bool needSword = !w.myData.hasSword;
        bool needShield = !w.myData.hasShield;
        bool needHelmet = !w.myData.hasHelmet;

        
        if(needSword)
        {
            if(WeaponsManager.Instance.swords.Count == 0)
            {
                tempBool = false;
            }
            else
            {
                tempBool = true;
            }
            
        }

        if (needHelmet && tempBool == false)
        {
            if(WeaponsManager.Instance.helmets.Count == 0)
            {
                tempBool = false;
            }
            else
            {
                tempBool = true;
            }
        }
        if(tempBool == false && needShield)
        {
            if(WeaponsManager.Instance.shields.Count == 0)
            {
                tempBool = false;
            }
            else
            {
                tempBool = true;
            }
        }

        
        return tempBool;
    }
}
