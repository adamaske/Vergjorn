using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InputFieldTest : MonoBehaviour
{
    public TMP_InputField inputField;


    public void Pressed()
    {
        Debug.Log(float.Parse(inputField.text).ToString());
    }
}
