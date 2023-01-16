using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager Instance;

    public List<Weapon> weapons = new List<Weapon>();

    public List<Weapon> swords = new List<Weapon>();

    public List<Weapon> shields = new List<Weapon>();

    public List<Weapon> helmets = new List<Weapon>();
    private void Awake()
    {
        Instance = this;
    }

    
    public void RemoveWeapon(WeaponType t)
    {
        
        if(t == WeaponType.sword)
        {
            Weapon w = swords[0];
            swords.Remove(w);
            if (weapons.Contains(w))
            {
                weapons.Remove(w);
            }
        }
        if (t == WeaponType.shield)
        {
            Weapon w = shields[0];
            shields.Remove(w);
            if (weapons.Contains(w))
            {
                weapons.Remove(w);
            }
        }
        if (t == WeaponType.helmet)
        {
            Weapon w = helmets[0];
            helmets.Remove(w);
            if (weapons.Contains(w))
            {
                weapons.Remove(w);
            }
        }

        
    }
    public void GetWeapon(WeaponType type)
    {
        Weapon w = new Weapon();
        w.myType = type;

        if(w.myType == WeaponType.sword)
        {
            swords.Add(w);
        }
        if (w.myType == WeaponType.shield)
        {
            shields.Add(w);
        }
        if (w.myType == WeaponType.helmet)
        {
            helmets.Add(w);
        }

        weapons.Add(w);
    }

    public void SaveWeapons()
    {
        WeaponSave s = new WeaponSave();
        s.weapons = weapons;

        SerializationManager.Save("weapons", s);
    }

    public void LoadWeapons()
    {
        FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/weapons.save");
        if(!info.Exists)
        {
            weapons = new List<Weapon>();
            return;
        }

        WeaponSave s = (WeaponSave)SerializationManager.Load(Application.persistentDataPath + "/saves/weapons.save");
        weapons = s.weapons;
    }

    public float WeaponCapacity()
    {
        return 100f;
    }

    public float CurrentAmountOfWeapons()
    {
        return weapons.Count;
    }
}

[System.Serializable]
public class WeaponSave
{
    public List<Weapon> weapons = new List<Weapon>();
}

[System.Serializable]
public class Weapon
{
    public WeaponType myType;


}
[System.Serializable]
public enum WeaponType
{
    sword, shield, helmet
}
