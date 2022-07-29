using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AttackMap))]
public class AttackMapEditor : Editor
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


    AttackMap map => (AttackMap)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        defaultcolor = GUI.backgroundColor;
        GUILayout.Space(5);


        map.RegenList();
        ButtonGrid(map.gridSizeX, map.gridSizeY);
    }



    void ButtonGrid(int sizeX, int sizeY)//Nested loop...
    {
        for (int z = 0, alpha = sizeY - 1; z < sizeY || alpha >= 0; z++, alpha--)//For each row
        {
            GUILayout.BeginHorizontal();//Everything Below this will be on the same line... *********************
            for (int x = 0; x < sizeX; x++)//For each button on that row
            {
                Vector2Int id = new Vector2Int(x, z);//Get ID from loop

                Button(id);//This void spawns 
            }
            GUILayout.EndHorizontal();// *** ...until here *******************************************************
        }
        GUI.backgroundColor = defaultcolor;//Resets the GUI Color.
    }


    void Button(Vector2Int id)//Creates a button
    {
        GUI.backgroundColor = map.map[id.x + (id.y * map.gridSizeX)]._selected ? onColor : offColor;//Set the color, same as writing -> if(buttons[id]) GUI.backgroundColor = onColor; else GUI.backgroundColor = offColor;

        if (GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))//Spawn a button with no text & with our size
        {
            map.map[id.x + (id.y * map.gridSizeX)]._selected = !map.map[id.x + (id.y * map.gridSizeX)]._selected;// if button pressed, invert state
            Debug.Log(id.x + (id.y) * map.gridSizeX);
            Debug.Log(id);
            EditorUtility.SetDirty(target);//Tells it stuff has changed so it can save it
        }
    }
}
#endif