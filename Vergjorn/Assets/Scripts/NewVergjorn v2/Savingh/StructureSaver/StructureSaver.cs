using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureSaver
{
    private static StructureSaver _current;

    public static StructureSaver Instance
    {
        get
        {
            if(_current == null)
            {
                _current = new StructureSaver();
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

    public List<StructureData> structures = new List<StructureData>();
}
