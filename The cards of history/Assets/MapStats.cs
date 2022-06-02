using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStats : MonoBehaviour
{
    public List<SOmap> map = new List<SOmap>();
    public List<GameObject> tiles = new List<GameObject>();
    public int[,,] heightmap;

    public TileStats[] playerSpots = new TileStats[5];
    public List<TileStats> attackSpots = new List<TileStats>();

    public void Start()
    {
        if(map.Count != 0) 
        {
            SetStats();
        }
    }



    public void SetStats()
    {

        for (int i = 0; i < map.Count; i++)
        {
            map[i].gridArray = new GameObject[map[i].gridSizeX, map[i].gridSizeY];
            heightmap = new int[map[0].gridSizeX, map.Count, map[0].gridSizeY];
        }

        int count2 = 0;

        for (int i = 0; i < map.Count; i++)
        {
            GameObject[] quickArray = new GameObject[map[i].gridSizeX];

            int yLevel = 0;
            int count = 0;
       

            foreach (MapData _data in map[i].map)
            {
                _data.height = i;
                if (_data.selected)
                {
                    heightmap[_data.xPos, i, _data.zPos] = 1;
                }
                if (!_data.selected)
                {
                    heightmap[_data.xPos, i, _data.zPos] = 0;
                }




                if (count == map[i].gridSizeX)
                {
                    FillJaggedArray(yLevel, quickArray, i);
                    yLevel = _data.xPos;

                    count = 0;


                    for (int j = 0; j < quickArray.Length; j++)
                    {
                        quickArray[j] = null;
                    }
                }

                
                quickArray[count] = tiles[count2];
                count++;
                count2++;


                if (yLevel == map[i].gridSizeY - 1 && count == map[i].gridSizeX)
                {
                    FillJaggedArray(yLevel, quickArray, i);
                    yLevel = _data.xPos;

                    count = 0;
                    

                    for (int j = 0; j < quickArray.Length; j++)
                    {
                        quickArray[j] = null;
                    }
                }
            }
        }
    }

    public void FillJaggedArray(int yLevel, GameObject[] array, int mapLevel)
    {
        for (int i = 0; i < array.Length; i++)
        {

            map[mapLevel].gridArray[yLevel, i] = array[i];
        }
    }

}
