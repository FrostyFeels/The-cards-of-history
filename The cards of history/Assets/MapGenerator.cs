using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    [Header("MapList")]
    public List<SOmap> map = new List<SOmap>();

    [Header("Prefab")]
    public GameObject tile;
    public GameObject mapHolder;

    [Header("Materials")]
    public Material mat;
    public Material selectMat;
    public Material unSelect;
    public Material playerSpot;


    [Header("MapStats")]
    [SerializeField] private MapStats mapStats;
    [SerializeField] private List<GameObject> tiles;

    [Header("Mode")]
    public Mode mode;

    [Header("CharacterSpots")]
    public int characterIndex;
    public enum Mode
    {
        Building,
        PlacingCharacters,
        Camera
    }


    private Vector3 start, end;

    private int[,,] _3DHeightMap;

    private int currentMaplevel;
    private int lastMapLevel;


    

    public void Start()
    {
        for (int i = 0; i < map.Count; i++)
        {
            map[i].gridArray = new GameObject[map[i].gridSizeX, map[i].gridSizeY];
        }       
        ConstructArea();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            characterIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            characterIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            characterIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            characterIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            characterIndex = 5;
        }

        switch (mode)
        {
            case Mode.Building:
                if (Input.GetMouseButtonDown(0))
                {
                    MapSelecter(true);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    MapSelecter(false);
                }
                break;
            case Mode.PlacingCharacters:
                if(Input.GetMouseButtonDown(0))
                {
                    SetPlayerSpawns();
                }
                break;
            case Mode.Camera:
                break;
            default:
                break;
        }


    }
    public void ConstructArea()
    {

        for (int i = 0; i < map.Count; i++)
        {
            foreach (MapData _data in map[i].map)
            {
                GameObject _tile = Instantiate(tile);

                _tile.transform.position = new Vector3(_data.xPos, i, -_data.zPos) * map[i].tileSize;
                _tile.transform.SetParent(mapHolder.transform);

                _tile.GetComponent<TileStats>()._ID = new Vector3(_data.xPos, i, _data.zPos);

                map[i].gridArray[_data.xPos, _data.zPos] = _tile;


                Vector2 vertexLocation = new Vector3(_data.xPos, _data.zPos);

                if (map[i].midPoint == vertexLocation)
                {
                    _tile.GetComponent<MeshRenderer>().material = mat;
                }

                if (_data.selected)
                {
                    _tile.GetComponent<MeshRenderer>().material = selectMat;
                }

                if(i != currentMaplevel)
                {
                    _tile.SetActive(false);
                }
            }
        }

        Debug.Log(map[2].gridArray[0,0]);
    }
    public void MapSelecter(bool firstTile)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(firstTile)
            {
                start = hit.collider.GetComponent<TileStats>()._ID;
            }
            else
            {
                end = hit.collider.GetComponent<TileStats>()._ID;
                if (GetComponentInChildren<TileSelecter>().mode == TileSelecter.Mode.Bulding)
                {
                    buildSelect();
                }             
            }            
        }
    }
    public void changeMapLevel(bool up)
    {

        lastMapLevel = currentMaplevel;
        if (up && currentMaplevel + 1 < map.Count)
        {  
            currentMaplevel++;     
        }
        else if(!up && currentMaplevel > 0)
        {
            currentMaplevel--;
        }


        if(lastMapLevel > currentMaplevel)
        {
       
            for (int i = 0; i < map[lastMapLevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[lastMapLevel].gridSizeX; j++)
                {
                    map[lastMapLevel].gridArray[j, i].SetActive(false);
                }
            }
        }

        if(lastMapLevel < currentMaplevel)
        {
            for (int i = 0; i < map[lastMapLevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[lastMapLevel].gridSizeX; j++)
                {
                    if(!map[lastMapLevel].map[j + (i * map[lastMapLevel].gridSizeY)].selected)
                    {
                        map[lastMapLevel].gridArray[j, i].SetActive(false);
                    }
                }
            }
        }

        if(currentMaplevel != lastMapLevel)
        {
            for (int i = 0; i < map[lastMapLevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[lastMapLevel].gridSizeX; j++)
                {                
                    map[currentMaplevel].gridArray[j, i].SetActive(true);
                }
            }
        }

    }
    public void buildSelect()
    {
        Vector2 realStart;
        Vector2 realEnd;

        if(start.x > end.x)
        {
            realStart.x = end.x;
            realEnd.x = start.x;
        }
        else
        {
            realStart.x = start.x;
            realEnd.x = end.x;
        }

        if (start.z > end.z)
        {
            realStart.y = end.z;
            realEnd.y = start.z;
        }
        else
        {
            realStart.y = start.z;
            realEnd.y = end.z;
        }

        for (int i = (int)realStart.y; i <= realEnd.y; i++)
        {
            for (int j = (int)realStart.x; j <= realEnd.x; j++)
            {
                map[currentMaplevel].map[j + (i * map[currentMaplevel].gridSizeX)].selected = !map[currentMaplevel].map[j + (i * map[currentMaplevel].gridSizeX)].selected;

                if(map[currentMaplevel].map[j + (i * map[currentMaplevel].gridSizeX)].selected)
                {
                    map[currentMaplevel].gridArray[j, i].GetComponent<Renderer>().material = selectMat;
                }
                else
                {
                    map[currentMaplevel].gridArray[j, i].GetComponent<Renderer>().material = unSelect;
                }              
            }
        }
    }
    public void ChangeMode(int val)
    {
        switch (val)
        {
            case 0:
                mode = Mode.Building;
                MapEnabler();
                break;
            case 1:
                mode = Mode.PlacingCharacters;
                MapDisabler();
                break;
            case 2:
                mode = Mode.Camera;
                MapDisabler();
                break;

        }

    }
    public void SetPlayerSpawns()
    {
     
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.collider;
            var renderer = selection.GetComponent<Renderer>();

            renderer.material = playerSpot;

            if(mapStats.playerSpots[characterIndex - 1] != null)
            {
                mapStats.playerSpots[characterIndex - 1].gameObject.GetComponent<Renderer>().material = selectMat;
                mapStats.playerSpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;
            }
        

            mapStats.playerSpots[characterIndex - 1] = selection.GetComponent<TileStats>();
            mapStats.playerSpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().text = characterIndex.ToString();
            mapStats.playerSpots[characterIndex - 1].gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
        }
    }
    public void MapDisabler()
    {
        for (int x = 0; x < map.Count; x++)
        {
            for (int i = 0; i < map[x].gridSizeY; i++)
            {
                for (int j = 0; j < map[x].gridSizeX; j++)
                {
                    if (!map[x].map[j + (i * map[x].gridSizeY)].selected)
                    {
                        map[x].gridArray[j, i].SetActive(false);
                    }
                }
            }
        }
    }
    public void MapEnabler()
    {
            for (int i = 0; i < map[currentMaplevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[currentMaplevel].gridSizeX; j++)
                {
                    if (!map[currentMaplevel].map[j + (i * map[currentMaplevel].gridSizeY)].selected)
                    {
                        map[currentMaplevel].gridArray[j, i].SetActive(true);
                    }
                }
            }
    }
}

