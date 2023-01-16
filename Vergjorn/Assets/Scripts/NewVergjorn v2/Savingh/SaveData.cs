using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData 
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if(_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if(value != null)
            {
                _current = value;
            }
        }
    }


    public List<WorkerData> workerData =  new List<WorkerData>();

    public List<StructureData> structureData = new List<StructureData>();


    public float metal;
    public float food;
    public float wood;
    public float myrmalm;
    public float ships;
    public float gold;

}
