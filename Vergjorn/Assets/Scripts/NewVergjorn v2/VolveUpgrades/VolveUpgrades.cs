using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolveUpgrades : MonoBehaviour
{
    public static VolveUpgrades Instance;

    private void Awake()
    {
        Instance = this;
    }
}
