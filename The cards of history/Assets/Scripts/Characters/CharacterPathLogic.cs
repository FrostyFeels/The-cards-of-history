using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPathLogic : MonoBehaviour
{
    public List<Vector3> path;
    [SerializeField] private Vector3 lastTile;
    [SerializeField] private Vector3 curTile;

    [SerializeField] private LayerMask tileMask;

    [SerializeField] private CameraMovement cam;
    [SerializeField] public LineRenderer line;

    [SerializeField] private MapStats stats;

    [SerializeField] private CharacterManager _CharMan;
    public CharacterInfo selected;

    public bool setPath = false;
    public bool settingPath = false;

    public List<MapData> _RangeTiles = new List<MapData>();
    public List<MapData> _ColorPath = new List<MapData>();

    public PathData[] pathHolder;


    public TurnManager turn;

    public void Start()
    {
        pathHolder = new PathData[stats.playerSpots.Length];
        for (int i = 0; i < pathHolder.Length; i++)
        {
            pathHolder[i] = new PathData();
            pathHolder[i].path = new List<Vector3>();
        }
    }
    public void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 200, tileMask))
        {
            if (cam.isMoving)
                return;

            //Makes sure the camera can't move if it isn't already moving
            if (!cam.isMoving)
                cam.canMove = false;
        }
        else
        {
            if (!settingPath)
            {
                cam.canMove = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            DragPath();
            line.enabled = true;
        }  
        
        if(setPath && Input.GetMouseButtonUp(0))
        {
            line.positionCount = 0;
            line.enabled = false;
            setPath = false;
            path.Clear();
            curTile = -Vector3.one;
            lastTile = -Vector3.one;
            selected = null;         
        }

        if(Input.GetMouseButtonUp(0))
        {
            settingPath = false;
        }

        if(settingPath)
        {
            cam.canMove = false;
        }
    }

    public void NewPath()
    {
        line.enabled = true;
        line.positionCount = 0;
        path.Clear();
        path.Add(selected.pos);
        curTile = selected.pos;
        RemoveRangeColors();
        ColorRange(selected.pos.y);
    }
    public void DragPath()
    {
        if (selected == null)
            return;

        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, tileMask))
        {
            if(!cam.isMoving)
            {
                settingPath = true;
            }

            if (hit.collider.CompareTag("Player") || cam.isMoving)
                return;

            Vector3 tile = hit.collider.GetComponent<TileStats>()._ID;

            if (curTile == tile)
            {
                return;
            }

            if (Vector2.Distance(new Vector2(tile.x,tile.z), new Vector2(curTile.x, curTile.z)) > 1)
            {
                return;
            }

            if (tile == lastTile)
            {
                path.RemoveAt(path.Count - 1);

                if (path.Count > 1)
                {
                    lastTile = path[path.Count - 2];
                }
                
                curTile = tile;
                RemovePathColors();
                SetLinePositions(path);          
                return;
            }

            if (selected._Moves + 1 <= path.Count)
                return;
                    
            lastTile = curTile;
            curTile = tile;
            path.Add(curTile);
            SetLinePositions(path);
        }
        else
        {
            cam.canMove = true;
        }

    
    }
    public void SetLinePositions(List<Vector3> _path)
    {
        line.positionCount = _path.Count;
        Vector3[] linePos = new Vector3[_path.Count];
        int count = 0;
        foreach (Vector3 _pos in _path)
        {
            MapData _data = stats.mapData[(int)_pos.x, (int)_pos.y, (int)_pos.z];        
            linePos[count] = new Vector3(_data.xPos, (int)_pos.y + 1, -_data.zPos) * stats.map[0].tileSize;
            count++;
            _ColorPath.Add(_data);
        }
        line.SetPositions(linePos);
        ColorPath();
    }
    public void SetPath()
    {       
        Vector3 _pos = path[path.Count - 1];
        MapData _data = stats.mapData[(int)_pos.x, (int)_pos.y, (int)_pos.z];

        if(_data._stoodOn)
        {
            Debug.Log("Used");
            return;
        }
        else
        {
            if (pathHolder[selected._Index].lastTile != null)
            {
                pathHolder[selected._Index].lastTile._stoodOn = false;
            }
            
            _data._stoodOn = true;
            pathHolder[selected._Index].lastTile = _data;
         
        }

        MoveTurn newTurn = new MoveTurn(selected.gameObject, path, stats);
        newTurn.addTurn(newTurn, turn);
        turn.AddUIElement();

        setPath = true;
        RemovePathColors();
        RemoveRangeColors();
    }
    public void ResetPath()
    {
        path.Clear();
        line.positionCount = 0;
        pathHolder[selected._Index].path.Clear();
        pathHolder[selected._Index].lastTile._stoodOn = false;
    }
    public void StartPath()
    {
        if(turn.turns.Count > 0)
        {
            turn.DoTurn(0);
        }
    }
    public void RemoveRangeColors()
    {
        foreach (MapData _Tile in _RangeTiles)
        {
            _Tile.GetRender().material = MaterialManager.getMaterial(_Tile._materialID);
        }

        _RangeTiles.Clear();
    }
    public void RemovePathColors()
    {
        foreach (MapData _Tile in _ColorPath)
        {
            _Tile.GetRender().material = MaterialManager.getMaterial(_Tile._materialID);
            if(_RangeTiles.Contains(_Tile))
            {
                MaterialManager.SetMaterial(_Tile.GetRender(), "Turqoise");
            }
        }

        _ColorPath.Clear();
    }
    public void ColorRange(int height)
    {
        Vector3Int middle = selected.pos;

        int startX = selected.pos.x - (selected._Moves);
        int startZ = selected.pos.z - (selected._Moves);

        int endX = selected.pos.x + (selected._Moves);
        int endZ = selected.pos.z + (selected._Moves);

        if(startX < 0)
        {
            startX = 0;
        }

        if(startZ < 0)
        {
            startZ = 0;
        }

        if(endX > stats.map[0].gridSizeX)
        {
            endX = stats.map[0].gridSizeX;
        }

        if (endZ > stats.map[0].gridSizeY)
        {
            endZ = stats.map[0].gridSizeY;
        }

        Vector2Int start, end;
        start = new Vector2Int(startX, startZ);
        end = new Vector2Int(endX, endZ);

        int xDis = 0, ydis = 0;
        int fullDiss = 0;

        for (int y = startZ; y < endZ; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                xDis = Mathf.Abs((middle.x - x));
                ydis = Mathf.Abs((middle.z - y));
                fullDiss = xDis + ydis;


                if(fullDiss <= selected._Moves)
                {
                    MaterialManager.SetMaterial(stats.mapData[x, height, y].GetRender(), "Turqoise");
                    _RangeTiles.Add(stats.mapData[x, height, y]);
                }
                    

                xDis = 0;
                ydis = 0;
                fullDiss = 0;
            }
        }
    }  
    public void ColorPath()
    {
        foreach (MapData _Tile in _ColorPath)
        {
            _Tile.GetRender().material = MaterialManager.getMaterial("Red");
        }
    }  
}

public class PathData
{
    public List<Vector3> path;
    public MapData lastTile;
}
