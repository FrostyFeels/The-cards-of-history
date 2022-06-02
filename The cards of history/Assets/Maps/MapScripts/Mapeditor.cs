using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOmap))]
public class Mapeditor : Editor
{


    int buttonSize = 50;

    Color defaultcolor;
    Color onColor = Color.green;
    Color offColor = Color.grey;

    [SerializeField]
    int row = 5;
    [SerializeField]
    int colum = 5;

    [SerializeField]
    Vector2 start, end;


    SOmap map => (SOmap)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        defaultcolor = GUI.backgroundColor;
        GUILayout.Space(5);

        RegenList();

        ButtonGrid(map.gridSizeX, map.gridSizeY);


        GUILayout.Space(25);
        if (GUILayout.Button("FillGrid"))
        {
            fillGrid();
        }
        GUILayout.Space(25);
        row = EditorGUILayout.IntSlider("Row",row, 0, map.gridSizeX);
        GUILayout.Space(5);
        colum = EditorGUILayout.IntSlider("Colum", colum, 0, map.gridSizeY);
        GUILayout.Space(5);
        if (GUILayout.Button("FillRow"))
        {
            FillRow(row);
        }
        GUILayout.Space(5);
        if (GUILayout.Button("FillColumm"))
        {
            FillColumm(colum);
        }
        GUILayout.Space(50);
        start = EditorGUILayout.Vector2Field("start", start);
        end = EditorGUILayout.Vector2Field("end", end);
        GUILayout.Space(5);
        if (GUILayout.Button("FillArea"))
        {
            FillArea(start ,end);
        }


    }


    void fillGrid()
    {
        for (int i = 0; i < map.map.Count; i++)
        {
            map.map[i].selected = !map.map[i].selected;
        }
    }

    void FillArea(Vector2 start, Vector2 end)
    {
        for (int i = (int)start.y; i < (int)end.y; i++)
        {
            for (int j = (int)start.x; j < (int)end.x; j++)
            {
                map.map[j + (i * map.gridSizeX)].selected = !map.map[j + (i * map.gridSizeX)].selected;
            }
        }
    }

    void FillColumm(int row)
    {
        for (int i = 0; i < map.gridSizeX; i++)
        {
            map.map[i + (row * map.gridSizeX)].selected = !map.map[i + (row * map.gridSizeX)].selected;
        }
    }

    void FillRow(int colum)
    {
        for (int i = 0; i < map.gridSizeX; i++)
        {
            map.map[colum + (i * map.gridSizeX)].selected = !map.map[colum + (i * map.gridSizeX)].selected;
        }
    }

    void RegenList()
    {
        if (map.map.Count != (map.gridSizeX * map.gridSizeY))
        {
            map.map.Clear();
            for (int x = 0; x < map.gridSizeY; x++)
            {
                for (int y = 0; y < map.gridSizeX; y++)
                {
                    MapData data = new MapData();
                    data.selected = false;
                    data.xPos = x;
                    data.zPos = y;
                    map.map.Add(data);
                }
            }
        }
    }

    void ButtonGrid(int sizeY, int sizeX)//Nested loop...
    {
        for (int x = 0; x < sizeX; x++)//For each row
        {
            GUILayout.BeginHorizontal();//Everything Below this will be on the same line... *********************
            for (int y = 0; y < sizeY; y++)//For each button on that row
            {
                Vector2Int id = new Vector2Int(x, y);//Get ID from loop

                Button(id);//This void spawns 
            }
            GUILayout.EndHorizontal();// *** ...until here *******************************************************
        }
        GUI.backgroundColor = defaultcolor;//Resets the GUI Color.
    }


    void Button(Vector2Int id)//Creates a button
    {
        GUI.backgroundColor = map.map[id.x + (id.y * map.gridSizeY)].selected ? onColor : offColor;//Set the color, same as writing -> if(buttons[id]) GUI.backgroundColor = onColor; else GUI.backgroundColor = offColor;

        if (GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))//Spawn a button with no text & with our size
        {
            map.map[id.x + (id.y * map.gridSizeY)].selected = !map.map[id.x + (id.y * map.gridSizeY)].selected;// if button pressed, invert state
            Debug.Log(id.x + (id.y) * map.gridSizeY);
            EditorUtility.SetDirty(target);//Tells it stuff has changed so it can save it
        }
    }




}
