using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _ActiveMenu; 

    [SerializeField] private GameObject OptionMenu;
    [SerializeField] private GameObject newMapMenu;
    [SerializeField] private GameObject loadedMapsMenu;

    public void Start()
    {
        ToggleMenu(_ActiveMenu);
    }

    public void ToggleMenu(GameObject menu)
    {
        _ActiveMenu.SetActive(false);
        menu.SetActive(true);
        _ActiveMenu = menu;
    }


    //Makes a new map
    public void MakeNewMap()
    {
        SceneSwap.Map map = SceneSwap._instance.map;

        if(map.gridsizeY != 0 && map.gridsizeX != 0 && map.mapName != "")
        {
            SceneSwap._instance.swapScenes();
        }
        else
        {
            Debug.Log(map.gridsizeX);
            Debug.Log(map.gridsizeY);
            Debug.Log(map.mapName);
        }
 
    }



}
