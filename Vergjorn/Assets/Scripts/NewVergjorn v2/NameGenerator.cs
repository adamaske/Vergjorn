using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    public static NameGenerator Instance;


    private void Awake()
    {
        Instance = this;
    }
    public List<string> names = new List<string>();

    public Name[] nameList;
    

    public Name GetName()
    {
        return nameList[Random.Range(0, nameList.Length -1)];
    }
}
[System.Serializable]
public class Name
{
    public string nameString;
    public bool male;
}
