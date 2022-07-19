using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapEditor : MonoBehaviour
{
    [SerializeField] private MapGen gen;
    public Vector3 startTile, endTile;

    public Material currentMaterial;

    public CameraController camera2;


    public List<Material> materials = new List<Material>();
    public List<Renderer> highlighted = new List<Renderer>();

    public List<MapData> edgeTileRenderer = new List<MapData>();
    public List<MapData> highlightedMaps = new List<MapData>();
    public List<MapData> selected = new List<MapData>();

    public List<MapData> walls = new List<MapData>();

    public bool firstTile;
    public bool _Fill;
    public bool _Erase;
    public bool _3D;
    public bool building;
    public bool _ChoosingHeight;
    public bool finishHeight;
    public bool clickedHeight;
    public int range;

    public TileStats id;
    public float colorFallOff;

    public float edgeColor, fillColor, fullColor;

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
        drawOutLine();
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
            MapHolderSelecter();
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
                emptyList();
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

        VisualizeTiles();
    }


    //This is what calls the builder method
    public void MapHolderSelecter()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, buildRange, tiles))
        {
            if (!camera2.isMoving)
            {
                building = true;
            }


            if (endTile == hit.collider.GetComponent<TileStats>()._ID || camera2.isMoving)
                return;

            if (gen.mode == MapGen.Mode._BUILDING)
                cancelBuild();
            if (gen.mode == MapGen.Mode._DRAWING)
                CancelDraw();


        if(!firstTile)
            {
                firstTile = true;
                startTile = hit.collider.GetComponent<TileStats>()._ID;
            }

            endTile = hit.collider.GetComponent<TileStats>()._ID;
            BuildMapDrag();
        }

    }


    //THIS IS THE REAL BUILDING METHOD FOR NOW
    public void BuildMapDrag()
    {
        Vector3 realStart;
        Vector3 realEnd;

        if (startTile.x > endTile.x)
        {
            realStart.x = endTile.x;
            realEnd.x = startTile.x;
        }
        else
        {
            realStart.x = startTile.x;
            realEnd.x = endTile.x;
        }

        if (startTile.z > endTile.z)
        {
            realStart.z = endTile.z;
            realEnd.z = startTile.z;
        }
        else
        {
            realStart.z = startTile.z;
            realEnd.z = endTile.z;
        }

        if (startTile.y > endTile.y)
        {
            realStart.y = endTile.y;
            realEnd.y = startTile.y;
        }
        else
        {
            realStart.y = startTile.y;
            realEnd.y = endTile.y;
        }

        if(gen.mode == MapGen.Mode._BUILDING)
        {
            switch (mode)
            {
                case FillMode._NOFILL:
                    NoFill(realStart, realEnd);
                    break;
                case FillMode._FILLGROUND:
                    FillGround(realStart, realEnd);
                    break;
                case FillMode._FILLWALLS:
                    FillWalls(realStart, realEnd);
                    break;
                case FillMode._FILLWALLGROUND:
                    FillGround(realStart, realEnd);
                    FillWalls(realStart, realEnd);
                    break;
                case FillMode._FILLFULL:
                    fillAll(realStart, realEnd);
                    break;
                default:
                    break;
            }

        }


        MapData data;

        if (currentMaterial == null)
            return;

        for (int i = (int)realStart.z; i <= realEnd.z; i++)
        {
            for (int j = (int)realStart.x; j <= realEnd.x; j++)
            {
                if (gen.mode == MapGen.Mode._DRAWING)
                {
                    for (int level = (int)realStart.y; level <= realEnd.y; level++)
                    {
                        data = gen.mapData[j, level, i];
                        if (_Fill)
                        {
                            selected.Add(data);

                            Material previousMaterial = MaterialManager.getMaterial(data._materialID);
                            materials.Add(previousMaterial);
                            data._materialID = currentMaterial.name;
                            data.GetRender().material = currentMaterial;
                        }

                        if (_Erase)
                        {
                            selected.Add(gen.mapData[j, level, i]);

                            Material previousMaterial = MaterialManager.getMaterial(data._materialID);
                            materials.Add(previousMaterial);

                            data._materialID = gen.filledMat.name;
                            data.GetRender().material = gen.filledMat;
                        }
                    }

                }
            }
        }
    }
    public void NoFill(Vector3 start, Vector3 end)
    {
        MapData data;
        for (int y = (int)start.z; y <= end.z; y++)
        {
            for (int x = (int)start.x; x <= end.x; x++)
            {
                data = gen.mapData[x, gen.currentMapLevel, y];
                if(((x == start.x || x == end.x) || (y == start.z || y == end.z)) && _Fill && !data._selected)
                {
                    selected.Add(data);
                    data.GetRender().material = gen.filledMat;
                    data._selected = true;
                }

                if (((x == start.x || x == end.x) || (y == start.z || y == end.z)) && _Erase && data._selected)
                {
                    selected.Add(data);
                    data.GetRender().material = gen.nonFilledMat;
                    data._selected = false;
                    Debug.Log("OwO");
                }
            }
        }


    }
    public void FillGround(Vector3 start, Vector3 end)
    {
        MapData data;
        for (int y = (int)start.z; y <= end.z; y++)
        {
            for (int x = (int)start.x; x <= end.x; x++)
            {
                data = gen.mapData[x, gen.currentMapLevel, y];
                if (!data._selected && _Fill)
                {
                     selected.Add(data);
                     data.GetRender().material = gen.filledMat;
                     data._selected = true;

                }
                else if (data._selected && _Erase)
                {
                     selected.Add(data);
                     data.GetRender().material = gen.nonFilledMat;
                     data._selected = false;
                }          
            }
        }

        
    }
    public void FillWalls(Vector3 start, Vector3 end)
    {
        MapData data;
        for (int y = (int)start.z; y <= end.z; y++)
        {
            for (int x = (int)start.x; x <= end.x; x++)
            {
                data = gen.mapData[x, gen.currentMapLevel, y];
                if (((x == start.x || x == end.x) || (y == start.z || y == end.z)) && _Fill)
                {
                    walls.Add(data);
                    selected.Add(data);
                    data.GetRender().material = gen.filledMat;
                    data._selected = true;
                }

                if (((x == start.x || x == end.x) || (y == start.z || y == end.z)) && _Erase)
                {
                    walls.Add(data);
                    selected.Add(data);
                    data.GetRender().material = gen.nonFilledMat;
                    data._selected = false;
                }
            }
        }
        _3D = true;
    }
    public void fillAll(Vector3 start, Vector3 end)
    {
        MapData data;
        for (int y = (int)start.z; y <= end.z; y++)
        {
            for (int x = (int)start.x; x <= end.x; x++)
            {
                data = gen.mapData[x, gen.currentMapLevel, y];
                if (!data._selected && _Fill)
                {
                    selected.Add(data);
                    data.GetRender().material = gen.filledMat;
                    data._selected = true;

                }
                else if (data._selected && _Erase)
                {
                    selected.Add(data);
                    data.GetRender().material = gen.nonFilledMat;
                    data._selected = false;
                }
            }
        }
        _3D = true;
    }



    //This empties the list after player is done building
    public void emptyList()
    {
        selected.Clear();
        materials.Clear();
        walls.Clear();
    }
    //Here we reset the buildable spot prefabs
    public void EmptyHighLight()
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

    //Here we clear the list and remove or add the blocks if the player drags less
    public void cancelBuild()
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
    public void CancelDraw()
    {
        for (int i = 0; i < materials.Count && i < selected.Count; i++)
        {
            selected[i].GetRender().material = materials[i];
            selected[i]._materialID = materials[i].name;
        }
        materials.Clear();
        selected.Clear();
    }

    //This gets the material from the menu
    public void GetMaterial(Material mat)
    {
        currentMaterial = MaterialManager.SetCurrentMaterial(mat.name);
    }

    //reset the currentFloor
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
    
    //Reset the all maps
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

    //Makes the buildable spots visible
    public void VisualizeTiles()
    {        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, buildRange, tiles))
        {
            //checks if camera is moving
            if (camera2.isMoving)
                return;

            //Makes sure the camera can't move if it isn't already moving
            if (!camera2.isMoving)
                camera2.canMove = false;

            //Makes sure it doesn't run if nothing has changed
            if (id == hit.collider.GetComponent<TileStats>())
                return;

            //Empty the highlights before running the code to highlight again
            EmptyHighLight();

   
            id = hit.collider.GetComponent<TileStats>();

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
        else
        {
            EmptyHighLight();

            if(!building)
            {
                camera2.canMove = true;
            }
        }
    }
    public void drawOutLine()
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
