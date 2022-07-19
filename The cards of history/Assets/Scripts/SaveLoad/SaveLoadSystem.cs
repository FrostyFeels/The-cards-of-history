
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class SaveLoadSystem : MonoBehaviour
{
    private string SavePath => $"{Application.persistentDataPath}/";



    [ContextMenu("Save")]
    public void Save()
    {
        var state = Loadfile();
        SaveState(state);
        SaveFile(state);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        var state = Loadfile();
        LoadState(state);        
    }

    public void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    Dictionary<string, object> Loadfile()
    {
        if(!File.Exists(SavePath))
        {
            Debug.Log(SavePath + " not found");
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            Debug.Log("Nya");
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    void SaveState(Dictionary<string,object> state)
    {
        
        foreach (var saveable in FindObjectsOfType<SaveAbleEntitiy>())
        {
           state[saveable.Id] = saveable.SaveState();
        }
    }
    void LoadState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveAbleEntitiy>())
        {
            if (state.TryGetValue(saveable.Id, out object savedState))
            {
                saveable.LoadState(savedState);
            }
        }
    }
}
