using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public SOmap map;
    public GameObject tile;
    public Material mat;

    public List<MapData> mapArray;

    public GameObject[][] jaggedMapArray;

    public void Start()
    {
        jaggedMapArray = new GameObject[map.gridSizeY][];

        for (int i = 0; i < jaggedMapArray.Length; i++)
        {
            jaggedMapArray[i] = new GameObject[map.gridSizeX];
        }
        ConstructArea();
    }

    public void Awake()
    {
        mapArray = map.map;
    }
    public void ConstructArea()
    {
        GameObject[] quickArray = new GameObject[map.gridSizeX];
   
        int yLevel = 0;
        int count = 0;
        foreach (MapData _data in mapArray)
        {
            GameObject _tile = Instantiate(tile);
            _tile.transform.position = new Vector3(_data.xPos, 0, _data.yPos) * map.tileSize;
            _tile.transform.SetParent(transform);


            _tile.GetComponent<TileStats>()._ID = new Vector2(_data.xPos, _data.yPos);

            Vector2 vertexLocation = new Vector2(_data.xPos, _data.yPos);

            if (map.midPoint == vertexLocation)
            {
                _tile.GetComponent<MeshRenderer>().material = mat;
            }
            if (!_data.selected)
            {
                _tile.SetActive(false);
            }


            if (count == map.gridSizeX)
            {

                FillJaggedArray(yLevel, quickArray);
                yLevel = _data.xPos;
            
                count = 0;


                for (int i = 0; i < quickArray.Length; i++)
                {
                    quickArray[i] = null;
                }
            }

            quickArray[count] = _tile;
            count++;


        }
        transform.Rotate(0, 90, 0);
    }

    public void FillJaggedArray(int yLevel, GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            jaggedMapArray[yLevel][i] = array[i];
        }
    }



}
