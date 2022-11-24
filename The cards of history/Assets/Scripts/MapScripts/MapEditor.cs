using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapEditor : MonoBehaviour
{
    [SerializeField] private MapGen gen;
    private Vector3 startTile, endTile;

    [SerializeField] private Material currentMaterial;

    [SerializeField] private CameraController camera2;


    public List<Material> materials = new List<Material>();
    public List<Renderer> highlighted = new List<Renderer>();

    public List<MapData> edgeTileRenderer = new List<MapData>();
    public List<MapData> highlightedMaps = new List<MapData>();
    public List<MapData> selected = new List<MapData>();

    public List<MapData> walls = new List<MapData>();

    private bool firstTile;
    public bool _Fill;
    public bool _Erase;
    public bool _3D;
    public bool building;
    public bool _ChoosingHeight;
    public bool finishHeight;
    public bool clickedHeight;
    private int range;

    private TileStats id;
    [SerializeField] private float colorFallOff;

    [SerializeField] private float edgeColor, fillColor, fullColor;

    [SerializeField] private LayerMask tiles;

    public int buildRange = 1000;


    public enum FillMode
    {
        _NOFILL,
        _FILLGROUND,
        _FILLWALLS,
        _FILLWALLGROUND,
        _FILLFULL
    }

    public FillMode mode;
    public void Start()
    {
        DrawOutline();
    }

    private void Update()
    {

        if(clickedHeight && Input.GetMouseButtonUp(0))
        {
            _ChoosingHeight = false;
            clickedHeight = false;
        }


        if (Input.GetMouseButton(0) && !_ChoosingHeight)
        {
            MapSelecter();
        }
        else if (Input.GetMouseButton(0) && _ChoosingHeight)
        {
            fillHeight();
        }


        if (Input.GetMouseButtonUp(0))
        {
            firstTile = false;
            startTile = -Vector3.one;
            endTile = -Vector3.one;
            if (!_3D)
            {
                EmptySelectedTiles();
            }
            else if (building && !_ChoosingHeight && selected.Count > 0)
            {
                DrawHeight();
                _ChoosingHeight = true;
            }
            building = false;
        }

        if (building)
        {
            camera2.canMove = false;
        }

        HighlightTiles();
    }


    //Selects the tile
    public void MapSelecter()
    {
        //Runs the code for an raycast to see which tile is selected
        TileStats tile = BuildRefrences.OnTileSelect(buildRange, tiles);

        if (tile == null)
            return;
        
        if (endTile == tile._ID || camera2.isMoving)
            return;

        building = true;

        if (gen.mode == MapGen.Mode._BUILDING)
            ResetBuilding();
        if (gen.mode == MapGen.Mode._DRAWING)
            ResetDrawing();


        if (!firstTile)
        {
            firstTile = true;
            startTile = tile._ID;
        }

        endTile = tile._ID;
        MapBuilder();
    }

    //calls the diffrent functions to fill the map
    public void MapBuilder()
    {
        Vector3[] startEnd = BuildRefrences.GetStart(startTile, endTile);

        Vector3 realStart = startEnd[0];
        Vector3 realEnd = startEnd[1];

        //Switch statement that calls buildrefrences(which holds the diffrent ways of building the map
        if (gen.mode == MapGen.Mode._BUILDING)
        {
            switch (mode)
            {
                case FillMode._NOFILL:
                    //NoFill(realStart, realEnd);
                    BuildLogic(BuildRefrences.GetEdges(realStart, realEnd, gen.mapData, gen.currentMapLevel), false);
                    break;
                case FillMode._FILLGROUND:
                    BuildLogic(BuildRefrences.GetEdges(realStart, realEnd, gen.mapData, gen.currentMapLevel), false);
                    BuildLogic(BuildRefrences.GetMiddle(realStart, realEnd, gen.mapData, gen.currentMapLevel), false);
                    //FillGround(realStart, realEnd);
                    break;
                case FillMode._FILLWALLS:
                    BuildLogic(BuildRefrences.GetEdges(realStart, realEnd, gen.mapData, gen.currentMapLevel), true);
                    _3D = true;
                    //FillWalls(realStart, realEnd);
                    break;
                case FillMode._FILLWALLGROUND:
                    BuildLogic(BuildRefrences.GetEdges(realStart, realEnd, gen.mapData, gen.currentMapLevel), true);
                    BuildLogic(BuildRefrences.GetMiddle(realStart, realEnd, gen.mapData, gen.currentMapLevel), false);
                    _3D = true;
                    //FillGround(realStart, realEnd);
                    //FillWalls(realStart, realEnd);
                    break;
                case FillMode._FILLFULL:
                    BuildLogic(BuildRefrences.GetEdges(realStart, realEnd, gen.mapData, gen.currentMapLevel), true);
                    BuildLogic(BuildRefrences.GetMiddle(realStart, realEnd, gen.mapData, gen.currentMapLevel), true);
                    _3D = true;
                    //fillAll(realStart, realEnd);
                    break;
                default:
                    break;
            }

        }

        if (currentMaterial == null)
            return;

        if (gen.mode == MapGen.Mode._DRAWING)
        {
            for (int level = (int)realStart.y; level <= realEnd.y; level++)
            {
                Debug.Log("runs");
                DrawLogic(BuildRefrences.GetEdges(realStart, realEnd, gen.mapData, level));
                DrawLogic(BuildRefrences.GetMiddle(realStart, realEnd, gen.mapData, level));
            }
        }
    }

    //fills or erases the map nodes based on fill or erase bool
    public void BuildLogic(List<MapData> dataList, bool fill)
    {
        foreach (MapData _data in dataList)
        {
            if (!_data._selected && _Fill)
            {
                selected.Add(_data);
                _data.GetRender().material = gen.filledMat;
                _data._selected = true;

                if (fill)
                    walls.Add(_data);
            }

            if(_data._selected && _Erase)
            {
                selected.Add(_data);
                _data.GetRender().material = gen.nonFilledMat;
                _data._selected = false;
            }
        }
    }

    //Draws or undraws the map nodes based on fill or erase bool
    public void DrawLogic(List<MapData> dataList)
    {
        foreach (MapData _data in dataList)
        {
            if (_Fill)
            {
                selected.Add(_data);

                Material previousMaterial = MaterialManager.getMaterial(_data._materialID);
                materials.Add(previousMaterial);
                _data._materialID = currentMaterial.name;
                _data.GetRender().material = currentMaterial;
            }

            if (_Erase)
            {
                selected.Add(_data);

                Material previousMaterial = MaterialManager.getMaterial(_data._materialID);
                materials.Add(previousMaterial);

                _data._materialID = gen.filledMat.name;
                _data.GetRender().material = gen.filledMat;
            }
        }

    }

    //clears the list with tiles selected
    public void EmptySelectedTiles()
    {
        selected.Clear();
        materials.Clear();
        walls.Clear();
    }

    //clears the tiles highlighted
    public void ResetHighLightedTiles()
    {
        if (highlightedMaps.Count > 0)
        {
            Color resetColor;
            foreach (MapData _data in highlightedMaps)
            {
                resetColor = _data.GetRender().material.color;

                if (!_data._edgeTile && !_data._selected)
                {
                    setColor(_data, resetColor);
                }
                else if(_data._selected)
                {
                    setColor(_data, resetColor);
                }

                if(_data._edgeTile && !_data._selected)
                {
                    setColor(_data, resetColor);
                }
            }          
        }

        highlightedMaps.Clear();

    }

    //Sets the color of the tile
    public void setColor(MapData data, Color color)
    {
        
        if(data._edgeTile && !data._selected)
        {
            color.a = edgeColor;
        }

        if(!data._edgeTile && !data._selected)
        {
            color.a = fillColor;
        }

        if(data._selected)
        {
            color.a = fullColor;
        }

        data.GetRender().material.color = color;
    }

    //erases or fills based on fill or erase bool
    public void ResetBuilding()
    {
        for (int i = 0; i < selected.Count; i++)
        {
            if(_Fill)
            {
                selected[i]._selected = false;
                selected[i].GetRender().material = gen.nonFilledMat;
                setColor(selected[i], selected[i].GetRender().material.color);
            }
            else if(_Erase)
            {
                selected[i]._selected = true;
                selected[i].GetRender().material = gen.filledMat;
                setColor(selected[i], selected[i].GetRender().material.color);
            }          
        }
        walls.Clear();
        selected.Clear();
    }
    //Undraws or draws based on fill or erase bool
    public void ResetDrawing()
    {
        for (int i = 0; i < materials.Count && i < selected.Count; i++)
        {
            selected[i].GetRender().material = materials[i];
            selected[i]._materialID = materials[i].name;
        }
        materials.Clear();
        selected.Clear();
    }

    //Gets the material currently Useless
    public void GetMaterial(Material mat)
    {
        currentMaterial = MaterialManager.SetCurrentMaterial(mat.name);
    }

    //Removes the current floor from the map
    public void resetFloor()
    {
            for (int y = 0; y < gen.map[gen.currentMapLevel].gridSizeY; y++)
            {
                for (int x = 0; x < gen.map[gen.currentMapLevel].gridSizeX; x++)
                {
                    gen.map[gen.currentMapLevel].map[x + (y * gen.map[gen.currentMapLevel].gridSizeX)]._selected = false;                    
                    gen.map[gen.currentMapLevel].map[x + (y * gen.map[gen.currentMapLevel].gridSizeX)]._materialID = gen.nonFilledMat.name;
                    gen.mapData[x, gen.currentMapLevel, y].GetRender().material = gen.nonFilledMat;
                    setColor(gen.mapData[x, gen.currentMapLevel, y], gen.mapData[x, gen.currentMapLevel, y].GetRender().material.color);
                }
            }
    }
    
    //Removes the entire build
    public void ResetAll()
    {
        for (int i = 0; i < gen.map.Count; i++)
        {
            for (int y = 0; y < gen.map[i].gridSizeY; y++)
            {
                for (int x = 0; x < gen.map[i].gridSizeX; x++)
                {
                    gen.map[i].map[x + (y * gen.map[i].gridSizeX)]._selected = false;
                    gen.map[i].map[x + (y * gen.map[i].gridSizeX)]._materialID = gen.nonFilledMat.name;
                    gen.mapData[x, i, y].GetRender().material = gen.nonFilledMat;
                    gen.mapData[x, i, y].GetGameobject().SetActive(false);
                    gen.mapData[x, gen.currentMapLevel, y].GetGameobject().SetActive(true);


                    setColor(gen.mapData[x, i, y], gen.mapData[x, i, y].GetRender().material.color);
                }
            }
        }
    }

    //Highlights tiles to see what you can select
    public void HighlightTiles()
    {

        TileStats tile = BuildRefrences.OnTileSelect(buildRange, tiles);

        if (tile == null)
        {
            ResetHighLightedTiles();

            if (!building)
            {
                camera2.canMove = true;
            }

            return;
        }
          
            //checks if camera is moving
            if (camera2.isMoving)
                return;


            camera2.canMove = false;

            //Makes sure it doesn't run if nothing has changed
            if (id == tile)
                return;

            //Empty the highlights before running the code to highlight again
            ResetHighLightedTiles();

   
            id = tile;

            Vector3 tileID = id._ID;
            Vector3 start = new Vector3((int)tileID.x - range, tileID.y, (int)tileID.z - range);



            int stepsX = 0;
            int stepsZ = 0;

            //logic for making the start position correct and the steps
            if (start.x < 0)
            {        
                stepsX = (int)(range * 2.5f) + (int)start.x;

                start.x = 0;
            }
            else stepsX = (int)(range * 2f + 1f);

            if (start.z < 0)
            {
                stepsZ = (int)(range * 2.5f) + (int)start.z;

                start.z = 0;              
            }
            else stepsZ = (int)(range * 2f + 1);



            //Logic for falloff of the selection area
            for (int i = (int)start.z; i < start.z + stepsZ && i < gen.map[gen.currentMapLevel].gridSizeY; i++)
            {
                for (int j = (int)start.x; j < start.x + stepsX && j < gen.map[gen.currentMapLevel].gridSizeX; j++)
                {
                    if (!gen.mapData[j,gen.currentMapLevel,i]._selected)
                    {
                        Color color = gen.mapData[j,gen.currentMapLevel,i].GetRender().material.color;
                      
                        float distance = Vector2.Distance(new Vector2(j, i), new Vector2(tileID.x, tileID.z));
                        color.a = 1 - distance * colorFallOff;
                        if (color.a < fillColor)
                        {
                            color.a = fillColor;
                        }
                        gen.mapData[j, gen.currentMapLevel, i].GetRender().material.color = color;


                        highlightedMaps.Add(gen.mapData[j, gen.currentMapLevel, i]);
                    }                                        
                }
            }

    }

    //The edges of how far you can build
    public void DrawOutline()
    {
        Color color;

        for (int x = 0; x < gen.map[gen.currentMapLevel].gridSizeX; x++)
        {
             edgeTileRenderer.Add(gen.mapData[x, gen.currentMapLevel, 0]);
             edgeTileRenderer.Add(gen.mapData[x, gen.currentMapLevel, gen.map[gen.currentMapLevel].gridSizeY - 1]);       
        }

        for (int y = 1; y < gen.map[gen.currentMapLevel].gridSizeY - 1 ; y++)
        {
             edgeTileRenderer.Add(gen.mapData[0, gen.currentMapLevel, y]);          
             edgeTileRenderer.Add(gen.mapData[gen.map[gen.currentMapLevel].gridSizeX - 1, gen.currentMapLevel, y]);      
        }

        for (int i = 0; i < edgeTileRenderer.Count; i++)
        {       
            edgeTileRenderer[i]._edgeTile = true;
            if (!edgeTileRenderer[i]._selected)
            {
                color = edgeTileRenderer[i].GetRender().material.color;
                color.a = edgeColor;
                edgeTileRenderer[i].GetRender().material.color = color;
            }
        }
    }

    //Draws the height for 3D map making
    public void DrawHeight()
    {
        Color color = gen.nonFilledMat.color;
        color.a = 1;
        for (int level = 0; level < gen.map.Count; level++)
        {            
            gen.mapData[selected[0].xPos, level, selected[0].zPos].GetGameobject().SetActive(true);
            gen.mapData[selected[0].xPos, level, selected[0].zPos].GetRender().material.color = color;      
        }

        finishHeight = true;
    }

    //Fills the height
    public void fillHeight()
    {
        Color color = gen.nonFilledMat.color;
        color.a = .5f;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, tiles))
        {
            int height = (int)hit.collider.GetComponent<TileStats>()._ID.y;
     
            for (int level = 0; level <= height; level++)
            {
                if(mode == FillMode._FILLWALLGROUND || mode == FillMode._FILLWALLS )
                {
                    foreach (MapData _data in walls)
                    {
                        gen.mapData[_data.xPos, level, _data.zPos]._selected = true;
                        gen.mapData[_data.xPos, level, _data.zPos].GetRender().material = gen.filledMat;
                        gen.mapData[_data.xPos, level, _data.zPos].GetGameobject().SetActive(true);
                    }
                }
                else
                {
                    foreach (MapData _data in selected)
                    {
                        gen.mapData[_data.xPos, level, _data.zPos]._selected = true;
                        gen.mapData[_data.xPos, level, _data.zPos].GetRender().material = gen.filledMat;
                        gen.mapData[_data.xPos, level, _data.zPos].GetGameobject().SetActive(true);
                    }
                }
            }

            for (int i = 0; i < gen.map.Count; i++)
            {
                if(i > height && selected.Count > 0)
                {
                    gen.mapData[selected[0].xPos, i, selected[0].zPos].GetGameobject().SetActive(false);
                    gen.mapData[selected[0].xPos, i, selected[0].zPos].GetRender().material.color = color;
                }
            }
            selected.Clear();
            walls.Clear();

            finishHeight = false;
            clickedHeight = true;
            building = false;
            _3D = false;          
        }
    }
}
