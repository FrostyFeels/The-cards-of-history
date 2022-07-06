using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Map", menuName = "SO/map")]
public class SOmap : ScriptableObject
{
    [SerializeField]
    public int gridSizeX;
    public int gridSizeY;
    public int tileSize;
    public Vector2 midPoint;
    public List<MapData> map = new List<MapData>();
    public GameObject[,] gridArray;
    
}

[Serializable]
public class MapData
{
    [SerializeField]
    public bool selected;
    public int xPos, zPos, height;
    public Material currentMat;
}
