using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGen : MonoBehaviour
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
    [SerializeField] private Material badZoneMat;

    [Header("Mode")]
    public Mode mode;

    [Header("SubMapEditor")]
    [SerializeField] private MapEditor mapEditor;
    [SerializeField] private MapPlayerSelect mapPlayerSelect;

    [Header("UI")]
    public GameObject assetsMenu;
    public GameObject enviromentMenu;
    public GameObject menuButton;



    public GameObject buildButtonMode, spriteButtonMode, enviromentButtonMode, placeCharacterButtonMode;
    public GameObject resetBtn, failSafeBtn;

    public int size;
    public enum Mode
    {
        _BUILDING,
        _ADDINGENVIROMENT,
        _PLACINGCHARACTERS,
        _ADDINGSPRITES
    }

    public int currentMapLevel, previousMapLevel;


    public void Start()
    {
        for (int i = 0; i < map.Count; i++)
        {
            map[i].gridArray = new GameObject[map[i].gridSizeX, map[i].gridSizeY];
            map[i].gridSizeX = size;
            map[i].gridSizeY = size;
          
        }

        ConstructMap();
        DoModeLogic();
    }
    public void ConstructMap()
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
                


                if (_data.selected)
                {
                    if(_data.currentMat == null)
                    {
                        _data.currentMat = filledMat;
                    }
                    _tile.GetComponent<MeshRenderer>().material = _data.currentMat;
                }

                if (i != currentMapLevel)
                {
                    _tile.SetActive(false);
                }
            }
        }
    }
    public void changeMapLevel(bool up)
    {

        previousMapLevel = currentMapLevel;
        if (up && currentMapLevel + 1 < map.Count)
        {
            currentMapLevel++;
        }
        else if (!up && currentMapLevel > 0)
        {
            currentMapLevel--;
        }

        if (previousMapLevel > currentMapLevel)
        {
            for (int i = 0; i < map[previousMapLevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[previousMapLevel].gridSizeX; j++)
                {
                    map[previousMapLevel].gridArray[j, i].SetActive(false);
                }
            }
        }

        if (previousMapLevel < currentMapLevel)
        {
            for (int i = 0; i < map[previousMapLevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[previousMapLevel].gridSizeX; j++)
                {
                    if (!map[previousMapLevel].map[j + (i * map[previousMapLevel].gridSizeY)].selected)
                    {
                        map[previousMapLevel].gridArray[j, i].SetActive(false);
                    }
                }
            }
        }

        if (currentMapLevel != previousMapLevel)
        {
            for (int i = 0; i < map[previousMapLevel].gridSizeY; i++)
            {
                for (int j = 0; j < map[previousMapLevel].gridSizeX; j++)
                {
                    map[currentMapLevel].gridArray[j, i].SetActive(true);
                }
            }
        }

        if(mode != Mode._BUILDING)
        {
            DisableBuilding();
        }

    }
    public void SetMode(int val)
    {
        switch (val)
        {
            case 0:
                mode = Mode._BUILDING;
                break;
            case 1:
                mode = Mode._ADDINGSPRITES;
                break;
            case 2:
                mode = Mode._PLACINGCHARACTERS;
                break;
            case 3:
                mode = Mode._ADDINGENVIROMENT;
         
                break;

        }
        Debug.Log(val);
        DoModeLogic();
    }
    public void DoModeLogic()
    {
        switch (mode)
        {
            case Mode._BUILDING:
                EnableBuilding();
                mapPlayerSelect.enabled = false;    
                mapEditor.enabled = true;
                assetsMenu.SetActive(false);
                menuButton.SetActive(false);
                resetBtn.SetActive(true);

                break;
            case Mode._ADDINGENVIROMENT:
                DisableBuilding();
                mapPlayerSelect.enabled = false;
                mapEditor.enabled = false;
                assetsMenu.SetActive(false);
                menuButton.SetActive(true);
                resetBtn.SetActive(false);
                break;
            case Mode._PLACINGCHARACTERS:
                DisableBuilding();
                mapPlayerSelect.enabled = true;
                mapEditor.enabled = false;
                assetsMenu.SetActive(false);
                menuButton.SetActive(false);
                resetBtn.SetActive(false);
                break;
            case Mode._ADDINGSPRITES:
                DisableBuilding();
                mapPlayerSelect.enabled = false;
                menuButton.SetActive(true);
                enviromentMenu.SetActive(false);
                resetBtn.SetActive(false);
                break;
        }
    }
    public void EnableBuilding()
    {
        for (int i = 0; i < map[currentMapLevel].gridSizeY; i++)
        {
            for (int j = 0; j < map[currentMapLevel].gridSizeX; j++)
            {
                if (!map[currentMapLevel].map[j + (i * map[currentMapLevel].gridSizeY)].selected)
                {
                    map[currentMapLevel].gridArray[j, i].SetActive(true);
                }
            }
        }
    }
    public void DisableBuilding()
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
}
