using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class MapGen : MonoBehaviour, ISaveable
{
    [Header("Maps")]
    public List<SOmap> map = new List<SOmap>();

    [Header("Prefab")]
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject mapHolder;

    [Header("Materials")]
    public Material filledMat;
    public Material nonFilledMat;
    public Material playerSpotMat;
    public Material enemySpotMat;
    public Material otherLayerMat;

    [Header("Mode")]
    public Mode mode;

    [Header("SubMapEditor")]
    public MapEditor mapEditor;
    public MapSpawnSelect mapPlayerSelect;
    public MapSizeEditor mapsizeEditor;
    public MapStats stats;

    public MapData[,,] mapData;

    public SizeInceaseButton[] buttons;

    public string mapName;
    public string maplevel;
    
    public enum Mode
    {
        _BUILDING,
        _DRAWING,
        _ChangeSize,
        _SETSPAWNS,
        _SETTILEEFFECTS,
        _SAVE
    }


    [HideInInspector] public int currentMapLevel, previousMapLevel;

    public int mapcount;

    public UImanagerBuilding uiMan;

    public void Awake()
    {
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:SOmap", new[] { "Assets/Map/" + maplevel });

        foreach (string _guid in guids)
        {
            string _path = AssetDatabase.GUIDToAssetPath(_guid);
            SOmap _map = AssetDatabase.LoadAssetAtPath(_path, typeof(SOmap)) as SOmap;
            map.Add(_map);

            EditorUtility.SetDirty(_map);
        }
#endif

        SetArrays();
        return;




        //TODO: for when creating your own maps in game

/*        if (SceneSwap._instance._NewGame)
        {
            map.Clear();
            NewGame();
        }
        else
        {
            map.Clear();
            LoadGame();
        }*/

        //SetArrays();

        
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            SendMapStats();
        }
    }

    public void NewGame()
    {
        SOmap _NewMap = new SOmap();
        _NewMap.gridSizeX = SceneSwap._instance.map.gridsizeX;
        _NewMap.gridSizeY = SceneSwap._instance.map.gridsizeY;
        _NewMap.tileSize = SceneSwap._instance.map.tilesize;
        mapName = SceneSwap._instance.map.mapName;
        _NewMap.RegenList();
        map.Add(_NewMap);


        

        SetArrays();
    }

    public void LoadGame()
    {
        mapName = SceneSwap._instance._LoadedMap.Replace(".txt", "");
        GameObject.Find("Load").GetComponent<MapSaveLoad>().Load(SceneSwap._instance._LoadedMap);

        SetArrays();
    }

    public void SetArrays()
    {
        for (int i = 0; i < map.Count; i++)
        {
            mapData = new MapData[map[i].gridSizeX, map.Count, map[i].gridSizeY];
        }
 
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].setPositions();
        }
        
        ConstructMap();
        uiMan.setMode(0);
        EnableBuilding();

        
    }

    public void SetArraysAfterSizeIncrease(int gridY, int gridX, int previousMapCount)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].setPositions();
        }

        for (int i = 0; i < previousMapCount; i++)
        {
            for (int y = 0; y < gridY; y++)
            {
                for (int x = 0; x < gridX; x++)
                {
                    Destroy(mapData[x, i, y].GetGameobject());
                    Debug.Log("Happens");
                }
            }     
        }

        for (int i = 0; i < map.Count; i++)
        {
            mapData = new MapData[map[i].gridSizeX, map.Count, map[i].gridSizeY];
        }

       
        mapEditor.highlighted.Clear();
        mapEditor.materials.Clear();


        ConstructMap();
        
        EnableBuilding();
        mapEditor.edgeTileRenderer.Clear();
        mapEditor.DrawOutline();
    }

    //This constructs the map gives the the positions
    //Fills the 3 needs MapData, Renderes and Gameobjects to do logic with
    //Colors the start of the map
    public void ConstructMap()
    {      
        for (int i = 0; i < map.Count; i++)
        {         
            foreach (MapData _data in map[i].map)
            {
                Debug.Log("OwO");
                GameObject _tile = Instantiate(tile);

                _tile.transform.position = new Vector3(_data.xPos, i, -_data.zPos) * map[i].tileSize;
                _tile.transform.localScale = Vector3.one * map[i].tileSize;
                _tile.transform.SetParent(mapHolder.transform);
             
                _tile.GetComponent<TileStats>()._ID = new Vector3(_data.xPos, i, _data.zPos);

                _data.SetRender(_tile.GetComponent<Renderer>());
                _data.SetGameobject(_tile);
      
                mapData[_data.xPos, i, _data.zPos] = _data;

                if(_data._materialID == "")
                {
                    Debug.Log("OwO");
                    _data._materialID = filledMat.name;
                }

                if (_data._materialID == null)
                {
                    Debug.Log("UwU");
                    _data._materialID = filledMat.name;
                }

                if (i != currentMapLevel)
                {
                    _tile.SetActive(false);
                }
            }
        }
    }

    //I hate this fucking function I want to improve but its probally not possible
    public void changeMapLevel(bool up)
    {

        previousMapLevel = currentMapLevel;

        //Checks if there is a spot to go up or down in the map
        if (up && currentMapLevel + 1 < map.Count)
        {
            currentMapLevel++;
        }
        else if (!up && currentMapLevel > 0)
        {
            currentMapLevel--;
        }

        if(mode != Mode._BUILDING)
        {
            DisableBuilding();
        }
        else
        {
            EnableBuilding();
        }

        mapEditor.edgeTileRenderer.Clear();
        mapEditor.DrawOutline();

    }

    //Turns off the invisible prefabs for when you are building
    public void EnableBuilding()
    {
        for (int level = 0; level < map.Count; level++)
        {
            for (int i = 0; i < map[level].gridSizeY; i++)
            {
                for (int j = 0; j < map[level].gridSizeX; j++)
                {        
                    
                    //This checks if the map is below the map and if its selected to color it otherlayer
                    if(level < currentMapLevel && !mapData[j, level, i]._selected)
                    {
                        mapData[j, level, i].GetGameobject().SetActive(false);
                    }
                    else if(level < currentMapLevel && mapData[j, level, i]._selected)
                    {
                        MaterialManager.SetMaterial(mapData[j, level, i].GetRender(), otherLayerMat.name);
                    }


                    //This checks if the map is selected or not
                    if (mapData[j, currentMapLevel, i]._selected)
                    {
                        mapData[j, currentMapLevel, i].GetGameobject().SetActive(true);
                        //mapData[j, currentMapLevel, i]._materialID = filledMat.name;
                        MaterialManager.SetMaterial(mapData[j, currentMapLevel, i].GetRender(), filledMat.name);
                    }
                    else if(!mapData[j, currentMapLevel, i]._selected)
                    {
                        mapData[j, currentMapLevel, i].GetGameobject().SetActive(true);
                        //mapData[j, currentMapLevel, i]._materialID = nonFilledMat.name;
                        MaterialManager.SetMaterial(mapData[j, currentMapLevel, i].GetRender(), nonFilledMat.name);
                    }


                    //this hides the map if its above
                    if(level > currentMapLevel)
                    {
                        mapData[j, level, i].GetGameobject().SetActive(false);
                    }

                    mapEditor.setColor(mapData[j, currentMapLevel, i], mapData[j, currentMapLevel, i].GetRender().material.color);
                }
            }
        }
    }
    public void fillColors()
    {
        MapData data;
        for (int level = 0; level < map.Count; level++)
        {
            for (int i = 0; i < map[level].gridSizeY; i++)
            {
                for (int j = 0; j < map[level].gridSizeX; j++)
                {
                    data = mapData[j, level, i];
                    if(mapData[j,level,i]._selected)
                    {
                        data.GetGameobject().SetActive(true);
                        data.GetRender().material = MaterialManager.getMaterial(data._materialID);
                    }
                }
            }
        }
    }

    //Turns off the invisible prefabs when you are not building
    public void DisableBuilding()
    {
        for (int x = 0; x < map.Count; x++)
        {
            for (int i = 0; i < map[x].gridSizeY; i++)
            {
                for (int j = 0; j < map[x].gridSizeX; j++)
                {
                    if (!mapData[j,x,i]._selected)
                    {
                        mapData[j, x, i].GetGameobject().SetActive(false);
                    }
                }
            }
        }
    }

    public void SendMapStats()
    {
        stats.playerSpots = mapPlayerSelect.playerSpots;
        stats.enemySpots = mapPlayerSelect.enemySpots;
        stats.map = map;
    }


    //This is how the maps get saved
    public object SaveState()
    {

        List<MapHolderData> listmaps = new List<MapHolderData>();
        foreach (var _map in map)
        {
            MapHolderData mapHolderData = new MapHolderData();
            mapHolderData.gridSizeX = _map.gridSizeX;
            mapHolderData.gridSizeY = _map.gridSizeY;
            mapHolderData.tileSize = _map.tileSize;
            mapHolderData.map = _map.map;


            listmaps.Add(mapHolderData);
        }

        return new SaveData
        {
            OwO = listmaps
        };
    }
    public void LoadState(object state)
    {
        int i = 1;
        var saveData = (SaveData)state;

        
        foreach (var _map in saveData.OwO)
        {
            if(i >= map.Count)
            {
                SOmap newMap = new SOmap();
                newMap.gridSizeX = _map.gridSizeX;
                newMap.gridSizeY = _map.gridSizeY;
                newMap.tileSize = _map.tileSize;
                newMap.map = _map.map;

                map.Add(newMap);
            }
            else
            {
                map[i].gridSizeX = _map.gridSizeX;
                map[i].gridSizeY = _map.gridSizeY;
                map[i].tileSize = _map.tileSize;
                map[i].map = _map.map;
            }          
            i++;
        }
    }

    [Serializable]
    private struct SaveData
    {
        public List<MapHolderData> OwO;
    }
}

[Serializable]
public class MapHolderData
{
    [SerializeField]
    public int gridSizeX;
    public int gridSizeY;
    public int tileSize;
    public List<MapData> map = new List<MapData>();
}
