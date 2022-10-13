using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildRefrences
{
    public static TileStats OnTileSelect(float buildRange, int layers)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, buildRange, layers))
        {
            return hit.collider.GetComponent<TileStats>();
        }
        else
        {
            return null;
        }

    }
    public static Vector3[] GetStart(Vector3 start, Vector3 end)
    {
        Vector3 realStart;
        Vector3 realEnd;

        if (start.x > end.x)
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
            realStart.z = end.z;
            realEnd.z = start.z;
        }
        else
        {
            realStart.z = start.z;
            realEnd.z = end.z;
        }

        if (start.y > end.y)
        {
            realStart.y = end.y;
            realEnd.y = start.y;
        }
        else
        {
            realStart.y = start.y;
            realEnd.y = end.y;
        }


        Vector3[] pos = new Vector3[2];
        pos[0] = realStart;
        pos[1] = realEnd;

        return pos;
    }  
    public static List<MapData> GetEdges(Vector3 start, Vector3 end, MapData[,,] list, int height)
    {
        List<MapData> dataList = new List<MapData>();
        MapData data;
        for (int y = (int)start.z; y <= end.z; y++)
        {
            for (int x = (int)start.x; x <= end.x; x++)
            {
                data = list[x, height, y];
                if (((x == start.x || x == end.x) || (y == start.z || y == end.z)))
                {
                    dataList.Add(data);
                }
            }
        }

        return dataList;
    }
    public static List<MapData> GetMiddle(Vector3 start, Vector3 end, MapData[,,] list, int height)
    {
        List<MapData> dataList = new List<MapData>();
        MapData data;
        for (int y = (int)start.z; y <= end.z; y++)
        {
            for (int x = (int)start.x; x <= end.x; x++)
            {
                data = list[x, height, y];
                if (((x != start.x && x != end.x) && (y != start.z && y != end.z)))
                {
                    dataList.Add(data);
                }
                
            }
        }

        return dataList;
    }

}
