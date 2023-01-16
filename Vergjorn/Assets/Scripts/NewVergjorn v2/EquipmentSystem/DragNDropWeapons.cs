using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragNDropWeapons : MonoBehaviour
{
    public GameObject swordPrefab;
    public GameObject shieldPrefab;
    public GameObject helmetPrefab;

    public List<Weapon> weapons = new List<Weapon>();

   
    public float rayRadius;

    public VikingUnit currentViking;
    public Transform menuGO;
    bool menuOpen;
    private void Start()
    {
        SetWeapons();
        
    }
    void SetWeapons()
    {
        weapons = WeaponsManager.Instance.weapons;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckVikingHit();
        }

        
    }

   
    void CheckVikingHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            VikingUnit unit = hit.collider.GetComponent<VikingUnit>();
            if(unit != null)
            {
                currentViking = unit;
                if (!menuOpen)
                {
                    OpenMenu();
                }
            }
            else
            {
                if (menuOpen && !MouseOverUI())
                {
                    CloseMenu();
                }
            }
        }
        else
        {
            if (menuOpen && MouseOverUI())
            {
                CloseMenu();
            }
           
        }

    }

    void CloseMenu()
    {
        menuOpen = false;
        menuGO.gameObject.SetActive(false);
    }

    void OpenMenu()
    {
        menuOpen = true;
        menuGO.gameObject.SetActive(true);
    }

    public void EquipWeapon(WeaponType t)
    {
        if (t == WeaponType.helmet)
        {
            if (currentViking.hasHelmet == false)
            {
                currentViking.GetWeapon(t, helmetPrefab);
            }
        }
        else
        if (t == WeaponType.sword)
        {
            if (currentViking.hasSword == false)
            {
                currentViking.GetWeapon(t, swordPrefab);
            }
        }
        else
        if (t == WeaponType.shield)
        {
            if (currentViking.hasShield == false)
            {
                currentViking.GetWeapon(t, shieldPrefab);
            }
        }
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
