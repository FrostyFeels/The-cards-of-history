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


    SOmap map => (SOmap)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        defaultcolor = GUI.backgroundColor;
        GUILayout.Space(5);

        RegenList();

        ButtonGrid(map.gridSizeX, map.gridSizeY);
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
                    data.yPos = y;
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
