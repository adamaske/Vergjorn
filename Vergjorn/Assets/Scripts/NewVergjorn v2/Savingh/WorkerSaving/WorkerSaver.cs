using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class WorkerSaver 
{

    
    private static WorkerSaver _current;
    public static WorkerSaver Instance
    {
        get
        {
            if (_current == null)
            {
                _current = new WorkerSaver();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    public List<WorkerData> workers = new List<WorkerData>();
}
