using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
[System.Serializable]
public class SerializationManager 
{
   

    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if(!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = Application.persistentDataPath + "/saves/" + saveName + ".save";

        FileStream file = File.Create(path);

        
        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }
    public static object Load(string path)
    {
        
        if (!File.Exists(path))
        {
           
            Debug.Log("Directory dont exist");
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);
        
        try
        {
            object save = formatter.Deserialize(file);
            
            file.Close();
            return save;
        }
        catch
        {
            Debug.Log("Found no file");
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();

        Vector3Serialization vector3surrogate = new Vector3Serialization();
        QuaternionSerialization quaternionSerialization = new QuaternionSerialization();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSerialization);

        formatter.SurrogateSelector = selector;



        return formatter;
    }

    public static void DeleteAllSaves()
    {
        string path = Application.persistentDataPath + "/saves/";

        DirectoryInfo directory = new DirectoryInfo(path);
        SerializationManager.DeleteDirectory(path);
    }

    public static void DeleteDirectory(string target_dir)
    {
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(target_dir, false);
    }
}
