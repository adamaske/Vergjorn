using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayResources : MonoBehaviour
{
    [Header("Top bar")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI metalText;
    
    public TextMeshProUGUI myrmalmText;
    public TextMeshProUGUI goldText;
   
    //public Myrmalm myrmalm;
    public FloatVariable wood;
    public FloatVariable food;
    public FloatVariable metal;
    
    public FloatVariable myrmalm;
    public FloatVariable gold;
    public FloatVariable foodCount;

    public BoolVariable useFoodCount;
    private void Update()
    {
        woodText.text = wood.value.ToString() + " / " + wood.capacity.ToString();
        if (!useFoodCount)
        {
            foodText.text = food.value.ToString() + " / " + food.capacity.ToString();
        }
        else
        {
            foodText.text = foodCount.value + " / " + foodCount.capacity.ToString();
        }
        
        metalText.text = metal.value.ToString() + " / " + metal.capacity.ToString();
        
        goldText.text = gold.value.ToString();
        myrmalmText.text = myrmalm.value.ToString() + " / " + myrmalm.capacity.ToString();
      
        //myrmalmText.text = myrmalm.myrmalm.ToString() +  " / " + myrmalm.myrmalmCapacity.ToString();

        
    }

    
}
