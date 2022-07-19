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
            ListTo3DArray();
        }
    }

    public void ListTo3DArray()
    {
        for (int i = 0; i < map.Count; i++)
        {
            foreach (MapData _data in map[i].map)
            {
                mapData[_data.xPos, i, _data.zPos] = _data;
            }
        }
    }
}
