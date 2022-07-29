using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "SO/AttackMap")]
public class AttackMap : ScriptableObject
{
    [SerializeField]
    public int gridSizeX;
    public int gridSizeY;
    public Vector2 startPoint;
    [SerializeField] public List<MapData> map = new List<MapData>();


    public void RegenList()
    {
        if (map.Count != (gridSizeX * gridSizeY))
        {
            map.Clear();
            for (int z = 0, alpha = gridSizeY - 1; z < gridSizeY || alpha >= 0; z++, alpha--)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    MapData data = new MapData();
                    data._selected = false;
                    data._highLighted = false;
                    data._edgeTile = false;
                    data.xPos = x;
                    data.zPos = z;
                    map.Add(data);
                }
            }
        }
    }
}
