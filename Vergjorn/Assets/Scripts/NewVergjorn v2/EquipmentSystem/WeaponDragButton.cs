using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDragButton : MonoBehaviour
{
    public DragNDropWeapons dragN;

    public WeaponType type;
    public void Pressed()
    {
        Debug.Log("Button pressed");
        dragN.EquipWeapon(type);
    }
}
