using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStats : MonoBehaviour
{
    public List<SOmap> map = new List<SOmap>();
    public MapData[,,] mapData;



    public TileStats[] playerSpots = new TileStats[5];


    public void Start()
    {
        if(map.Count != 0)
        {
            mapData = new MapData[map[0].gridSizeX, map.Count, map[0].gridSizeY];
            ListTo3DArray();
            
        }
    }

    public void ListTo3DArray()
    {
        for (int i = 0; i < map.Count; i++)
        {
            foreach (MapData _data in map[i].map)
            {
                Debug.Log(_data.xPos + " :x " + i + " :i " + _data.zPos + " :z");
                mapData[_data.xPos, i, _data.zPos] = _data;
            }
        }
    }
}
