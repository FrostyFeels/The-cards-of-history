using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapCreator : MonoBehaviour
{
    //This class is used for when people want to create a map and want to give it a starting width, lenght, height and name
    public void SetWidth(string s)
    {
        SceneSwap._instance.map.gridsizeX = int.Parse(s);
        Debug.Log(SceneSwap._instance.map.gridsizeX);
    }

    public void SetLenght(string s)
    {
        SceneSwap._instance.map.gridsizeY = int.Parse(s);
        Debug.Log(SceneSwap._instance.map.gridsizeY);
    }

    public void SetHeight(string s)
    {
        SceneSwap._instance.map.gridsizeX = int.Parse(s);
    }

    public void SetName(string s)
    {
        SceneSwap._instance.map.mapName = s;
    }


}
