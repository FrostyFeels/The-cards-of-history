using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;





public class MapSizeEditor : MonoBehaviour
{
    [SerializeField] private MapGen gen;
    [SerializeField] private List<MapData> maps;
    [SerializeField] private int previousGridX, previousGridY;
    public bool increase = true;

    private int level;

    public void Start()
    {
        gen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
    }

    public void changeHeight()
    {
        if(increase)
        {
            IncreaseLevel();
        }
        else
        {
            DecreaseMap();
        }
    }

    //Add another layer to the map
    public void IncreaseLevel()
    {

        SOmap map = ScriptableObject.CreateInstance<SOmap>();

        //takes the base map so that it will stay the same x,y and tile size
        map.gridSizeX = gen.map[0].gridSizeX;
        map.gridSizeY = gen.map[0].gridSizeY;
        map.tileSize = gen.map[0].tileSize;
        map.RegenList();

        
        gen.map.Add(map);



        previousGridX = gen.map[0].gridSizeX;
        previousGridY = gen.map[0].gridSizeY;

        gen.SetArraysAfterSizeIncrease(previousGridY, previousGridX, gen.map.Count - 1);



        //This is the part that saves the map if you are in Unity
#if UNITY_EDITOR
        if (AssetDatabase.IsValidFolder("Assets/Map/" + gen.maplevel))
        {
            
        }
        else
        {
            Debug.Log("Assets/Map/" + gen.maplevel);
            AssetDatabase.CreateFolder("Assets/Map", gen.maplevel);
            
        }
        map.name =  gen.maplevel + "." + (gen.map.Count);

        AssetDatabase.CreateAsset(map, "Assets/Map/" + gen.maplevel + "/" + map.name + ".asset");

        EditorUtility.SetDirty(map);
        EditorUtility.SetDirty(gen);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();     
#endif
    }

    //Makes the map smaller
    public void DecreaseMap()
    {
        previousGridX = gen.map[0].gridSizeX;
        previousGridY = gen.map[0].gridSizeY;

        if (gen.map.Count > 1)
        {
            gen.map.Remove(gen.map[gen.map.Count - 1]);
            gen.SetArraysAfterSizeIncrease(previousGridY, previousGridX, gen.map.Count + 1);

#if UNITY_EDITOR

            string mapname = gen.maplevel + "." + (gen.map.Count + 1);

            AssetDatabase.DeleteAsset("Assets/Map/" + gen.maplevel + "/" + mapname + ".asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

#endif
        }


    }

    public void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseLevel();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {        
            CopyMap(false, false, true, false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CopyMap(false, true, false, false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CopyMap(true, false, false, false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CopyMap(false,false,false,true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Load").GetComponent<MapSaveLoad>().Save();
            Application.Quit();
        }
    }

    //This copies the map for refrence sake
    public void CopyMap(bool up, bool down, bool left, bool right)     
    {
        previousGridX = gen.map[gen.currentMapLevel].gridSizeX;
        previousGridY = gen.map[gen.currentMapLevel].gridSizeY;

        for (int i = 0; i < gen.map.Count; i++)
        {
            for (int y = 0; y < gen.map[i].gridSizeY; y++)
            {
                for (int x = 0; x < gen.map[i].gridSizeX; x++)
                {
                    MapData data = new MapData();
                    data._selected = gen.mapData[x, i, y]._selected;
                    data.xPos = gen.mapData[x, i, y].xPos;
                    data.zPos = gen.mapData[x, i, y].zPos;                   
                    data._materialID = gen.mapData[x, i, y]._materialID;
                    data._edgeTile = gen.mapData[x, i, y]._edgeTile;
                    data.SetRender(gen.mapData[x, i, y].GetRender());
                    data._highLighted = gen.mapData[x, i, y]._highLighted;
                    maps.Add(data);
                }
            }

            if(down && increase)
            {
                Addcolumm(i);
            }
            else if(down && !increase && gen.map[i].gridSizeY > 1)
            {
                RemoveColum(i);
            }
            if(right && increase)
            {
                Addrow(i);
            }
            else if (right && !increase && gen.map[i].gridSizeX > 1)
            {
                RemoveRow(i);
            }
            if (up && increase)
            {
                AddcolummUp(i);
            }
            else if (up && !increase && gen.map[i].gridSizeY > 1)
            {
                RemoveColumUp(i);
            }
            if (left && increase)
            {
                AddrowLeft(i);
            }
            else if (left && !increase && gen.map[i].gridSizeX > 1)
            {
                RemoveRowLeft(i);
            }
        }
         gen.SetArraysAfterSizeIncrease(previousGridY, previousGridX, gen.map.Count);
    }

    //these are all the diffrent ways you can increase the map
    public void Addcolumm(int height)
    {
        previousGridY = gen.map[height].gridSizeY;

         gen.map[height].gridSizeY++;
         gen.map[height].RegenList();
    

            for (int y = 0, alpha = 0; y < previousGridY; y++, alpha++)
            {
                for (int x = 0; x < gen.map[height].gridSizeX; x++)
                {
                    //gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._selected = maps[x + (y * gen.map[height].gridSizeX)]._selected;
                    gen.map[height].map[x + (y * gen.map[height].gridSizeX)] = maps[x + (y * gen.map[height].gridSizeX)];
                }
            }       
        maps.Clear();
    }

    public void RemoveColum(int height)
    {
        Debug.Log("removed colum down");
        previousGridY = gen.map[height].gridSizeY;

        gen.map[height].gridSizeY--;
        gen.map[height].RegenList();


        for (int y = 0, alpha = 0; y < gen.map[height].gridSizeY; y++, alpha++)
        {
            for (int x = 0; x < gen.map[height].gridSizeX; x++)
            {
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._selected = maps[x + (y * gen.map[height].gridSizeX)]._selected;
            }
        }
        maps.Clear();
    }

    public void AddcolummUp(int height)
    {
        previousGridY = gen.map[height].gridSizeY;

        gen.map[height].gridSizeY++;
        gen.map[height].RegenList();


        for (int y = 0, alpha = 0; y < previousGridY; y++, alpha++)
        {
            for (int x = 0; x < gen.map[height].gridSizeX; x++)
            {
                gen.map[height].map[x + gen.map[height].gridSizeX + (y * gen.map[height].gridSizeX)]._selected = maps[x + (y * gen.map[height].gridSizeX)]._selected;
            }
        }
        maps.Clear();
    }

    public void RemoveColumUp(int height)
    {
        Debug.Log("removed colum up");
        previousGridY = gen.map[height].gridSizeY;

        gen.map[height].gridSizeY--;
        gen.map[height].RegenList();


        for (int y = 0, alpha = 0; y < gen.map[height].gridSizeY; y++, alpha++)
        {
            for (int x = 0; x < gen.map[height].gridSizeX; x++)
            {
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._selected = maps[x + gen.map[height].gridSizeX + (y * gen.map[height].gridSizeX)]._selected;
            }
        }
        maps.Clear();
    }

    public void Addrow(int height)
    {
        previousGridX = gen.map[height].gridSizeX;

        gen.map[height].gridSizeX++;
        gen.map[height].RegenList();

        for (int y = 0; y < gen.map[height].gridSizeY; y++)
        {
            for (int x = 0, alpha = 0; x < previousGridX; x++, alpha++)
            {
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._selected = maps[x + (y * previousGridX)]._selected;
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._materialID = maps[x + (y * previousGridX)]._materialID;
            }
        }
        maps.Clear();
    }

    public void RemoveRow(int height)
    {
        previousGridX = gen.map[height].gridSizeX;

        gen.map[height].gridSizeX--;
        gen.map[height].RegenList();

        for (int y = 0; y < gen.map[height].gridSizeY; y++)
        {
            for (int x = 0, alpha = 0; x < gen.map[height].gridSizeX; x++, alpha++)
            {
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._selected = maps[x + (y * previousGridX)]._selected;
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._materialID = maps[x + (y * previousGridX)]._materialID;
            }
        }
        maps.Clear();
    }

    public void AddrowLeft(int height)
    {
        previousGridX = gen.map[height].gridSizeX;

        gen.map[height].gridSizeX++;
        gen.map[height].RegenList();

        for (int y = 0; y < gen.map[height].gridSizeY; y++)
        {
            for (int x = 0, alpha = 1; x < previousGridX; x++, alpha++)
            {
                gen.map[height].map[alpha + (y * gen.map[height].gridSizeX)]._selected = maps[x + (y * previousGridX)]._selected;
                gen.map[height].map[alpha + (y * gen.map[height].gridSizeX)]._materialID = maps[x + (y * previousGridX)]._materialID;

            }
        }
        maps.Clear();
    }

    public void RemoveRowLeft(int height)
    {
        previousGridX = gen.map[height].gridSizeX;

        gen.map[height].gridSizeX--;
        gen.map[height].RegenList();

        for (int y = 0; y < gen.map[height].gridSizeY; y++)
        {
            for (int x = 0, alpha = 1; x < gen.map[height].gridSizeX; x++, alpha++)
            {
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._selected = maps[alpha + (y * previousGridX)]._selected;
                gen.map[height].map[x + (y * gen.map[height].gridSizeX)]._materialID = maps[alpha + (y * previousGridX)]._materialID;
            }
        }
        maps.Clear();
    }
}
