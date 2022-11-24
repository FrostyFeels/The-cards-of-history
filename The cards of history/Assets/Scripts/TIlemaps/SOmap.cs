using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Map", menuName = "SO/map")]
public class SOmap : ScriptableObject
{
    [SerializeField]
    public int gridSizeX;
    public int gridSizeY;
    public int tileSize;
    [SerializeField] public List<MapData> map = new List<MapData>();


    public void RegenList()
    {
        if (map.Count != (gridSizeX * gridSizeY))
        {
            map.Clear();
            for (int z = 0, alpha = gridSizeY - 1; z < gridSizeY || alpha >= 0; z++, alpha--)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    MapData data = new MapData();
                    data._selected = false;
                    data._highLighted = false;
                    data._edgeTile = false;
                    data.xPos = x;
                    data.zPos = z;
                    map.Add(data);
                }
            }
        }
    }

}


//I can't save gameobjects or renderes so theyre excluded when saving MapData
[Serializable]
public class MapData
{

    [SerializeField] public bool _selected;
    [SerializeField] public int xPos, zPos, height;
    [SerializeField] public string _materialID;
    [SerializeField] public bool _edgeTile;
    [SerializeField] public bool _highLighted;
    [SerializeField] public bool _stoodOn;
    [NonSerialized] GameObject _Gameobject;
    [NonSerialized] Renderer _Renderer;



    public Renderer GetRender()
    {
        return _Renderer;
    }

    public GameObject GetGameobject()
    {
        return _Gameobject;
    }

    public void SetGameobject(GameObject obj)
    {
        _Gameobject = obj; 
    }

    public void SetRender(Renderer render)
    {
        _Renderer = render;
    }

  
}
