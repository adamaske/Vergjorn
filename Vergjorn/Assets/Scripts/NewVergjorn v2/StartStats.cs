using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStats : MonoBehaviour
{
    public static StartStats Instance;
    private void Awake()
    {
        Instance = this;
    }
    public FloatVariable gold;

    public FloatVariable food;

    public FloatVariable metal;

    public FloatVariable wood;

    public FloatVariable myrmalm;

    public float startGold;
    public float startFood;
    public float startMetal;
    public float startWood;
    public float startMyrmalm;
    public void SetNewGameStats()
    {
        gold.value = startGold;
        food.value = startFood;
        metal.value = startMetal;
        wood.value = startWood;
        myrmalm.value = startMyrmalm;
    }
}
