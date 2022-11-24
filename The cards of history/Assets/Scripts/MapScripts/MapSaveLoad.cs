using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
public class MapSaveLoad : MonoBehaviour
{
    private string SavePath;
    public GameObject mapPrefab;
    public GameObject MapHolder;


    [ContextMenu("Save")]
    public void Save()
    {
        var state = Loadfile();
        SaveState(state);
        SaveFile(state);
    }

    //TODO rewatch the video to understand how exactly it works

    [ContextMenu("Load")]
    public void Load(string s)
    {
        SavePath = $"{Application.persistentDataPath}/map/" + s;
        var state = Loadfile();
        LoadState(state, s);
    }

    [ContextMenu("LoadMaps")]
    public void LoadMaps()
    {
        String[] files = Directory.GetFiles($"{Application.persistentDataPath}/map/");

        foreach (var filepath in files)
        {
            string mapName = filepath.Replace($"{Application.persistentDataPath}/map/", "");
            string fullmapName = mapName.Replace(".txt", "");

            GameObject map = Instantiate(mapPrefab, MapHolder.transform);

            Button button = map.GetComponentInChildren<Button>();

            map.GetComponentInChildren<TextMeshProUGUI>().text += fullmapName.ToString();

            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    loadScenes(mapName);
                });
            }
        }
    }

    public void loadScenes(string s)
    {
        SceneSwap._instance.LoadScenes(s);
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
        Debug.Log(SavePath);
        if (!File.Exists(SavePath))
        {
            Debug.Log(SavePath + " not found");
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }
    void SaveState(Dictionary<string, object> state)
    {

        foreach (var saveable in FindObjectsOfType<SaveAbleEntitiy>())
        {
            state[saveable.Id] = saveable.SaveState();
            SavePath = $"{Application.persistentDataPath}/map/" + saveable.Id + ".txt";
        }
    }

    void LoadState(Dictionary<string, object> state, string id)
    {
        foreach (var saveable in FindObjectsOfType<SaveAbleEntitiy>())
        {
            if (state.TryGetValue(id.Replace(".txt", ""), out object savedState))
            {
                saveable.LoadState(savedState);
            }
        }
    }

}
