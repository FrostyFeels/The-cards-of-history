using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public List<SOmap> map = new List<SOmap>();

    public GameObject tile;
    public Material mat;
    public GameObject mapHolder;
    [SerializeField] private MapStats mapStats;

    public int[,,] _3DHeightMap;

    

    public void Start()
    {

        for (int i = 0; i < map.Count; i++)
        {
            map[i].gridArray = new GameObject[map[i].gridSizeX, map[i].gridSizeY];
            _3DHeightMap = new int[map[0].gridSizeX, map.Count, map[0].gridSizeY];
        }
        
        ConstructArea();
    }

    public void ConstructArea()
    {
        for (int i = 0; i < map.Count; i++)
        {
            foreach (MapData _data in map[i].map)
            {
                GameObject _tile = Instantiate(tile);


                _tile.transform.position = new Vector3(_data.zPos, i, _data.xPos) * map[i].tileSize;
                _tile.transform.SetParent(mapHolder.transform);

                _tile.GetComponent<TileStats>()._ID = new Vector3(_data.zPos, i ,_data.xPos);

                mapStats.tiles.Add(_tile);

                Vector2 vertexLocation = new Vector3(_data.xPos,_data.zPos);

                if (map[i].midPoint == vertexLocation)
                {
                    _tile.GetComponent<MeshRenderer>().material = mat;
                }
           
                if (!_data.selected)
                {
                    _tile.SetActive(false);
                }
            }
        }

        mapStats.map = map;
        mapStats.SetStats();


    }


}

