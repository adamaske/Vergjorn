using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrondCheats : MonoBehaviour
{
    public FloatVariable wood;
    public FloatVariable metal;
    public FloatVariable gold;

    public bool cheatsEnabled;

    public StructureButton[] buttonsToSetDespiteElse;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            cheatsEnabled = !cheatsEnabled;
        }

        if (cheatsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GiveResources();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeSpeed(-1);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                ChangeSpeed(1);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                Set();
            }
        }
        
    }

    void Set()
    {
        foreach(StructureButton b in buttonsToSetDespiteElse)
        {
            b.canPurchaseDespiteElse = !b.canPurchaseDespiteElse;
        }
    }

    public void ChangeSpeed(float value)
    {
        float k = Time.timeScale;

        k += value;

        Debug.Log(k.ToString());
        Time.timeScale = k;
    }

    public void GiveResources()
    {
        wood.value += 100;
        metal.value += 100;
        gold.value += 100;
    }
}
