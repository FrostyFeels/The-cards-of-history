using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStats : MonoBehaviour
{
    public List<SOmap> map = new List<SOmap>();
    public MapData[,,] mapData;



    public TileStats[] playerSpots = new TileStats[5];
    public TileStats[] enemySpots = new TileStats[5];


    public void Start()
    {
        if(map.Count != 0)
        {
            mapData = new MapData[map[0].gridSizeX, map.Count, map[0].gridSizeY];
            ListTo3DArray();        
        }
    }


    //List are saveable but a 3D array is needed to access movement easier
    //Turns the saved list into a 3D array at startup
    public void ListTo3DArray()
    {
        TileStats[] _Tiles = GetComponentsInChildren<TileStats>();
        for (int i = 0; i < map.Count; i++)
        {
            foreach (MapData _data in map[i].map)
            {
                _data._stoodOn = false;
                mapData[_data.xPos, i, _data.zPos] = _data;    
                
                if(_data._selected)
                {
                    
                }
            }
        }

        foreach (TileStats _Tile in _Tiles)
        {
            MapData _data = mapData[(int)_Tile._ID.x, (int)_Tile._ID.y, (int)_Tile._ID.z];

            _data.SetGameobject(_Tile.gameObject);
            _data.SetRender(_Tile.gameObject.GetComponent<Renderer>());

            if (!_data._selected)
                Destroy(_data.GetGameobject());
        }
    }
}
