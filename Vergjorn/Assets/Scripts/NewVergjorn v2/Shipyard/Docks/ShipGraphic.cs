using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGraphic : MonoBehaviour
{
    public Ship myShip;

    public void GetShip(Ship ship)
    {
        myShip = ship;
    }
    //Do logic for setting the right graphic depending on level
}
