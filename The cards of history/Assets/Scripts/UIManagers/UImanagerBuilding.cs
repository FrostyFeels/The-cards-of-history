using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanagerBuilding : MonoBehaviour
{
    [Header("Building")]
    [SerializeField] private GameObject _BuildCanvas;
    [SerializeField] private GameObject failSafeBtn;
    [SerializeField] private Button resetYes;

    [Header("draw")]
    [SerializeField] private GameObject _DrawCanvas;
    [SerializeField] private GameObject _DrawMenu;

    [Header("Change size")]
    [SerializeField] private GameObject _ChangeSizeButtons;
    [SerializeField] private GameObject _ChangeSizeCanvas;


    [Header("Tile effects")]
    [SerializeField] private GameObject _TileEffectCanvas;
    [SerializeField] private GameObject _TileEffectMenu;

    [Header("Save")]
    [SerializeField] private GameObject _SaveCanvas;

    [Header("Spawns")]
    [SerializeField] private GameObject _SpawnCanvas;


    [SerializeField] private List<GameObject> _ActiveObjects;

    [SerializeField] private MapGen gen;

    private bool firstRun = true;



    public void setMode(int mode)
    {

        foreach (var _button in _ActiveObjects)
        {
            _button.SetActive(false);
        }

        gen.mapEditor.enabled = false;
        gen.mapPlayerSelect.enabled = false;

        _ActiveObjects.Clear();

        switch (mode)
        {
            case 0:
                gen.mode = MapGen.Mode._BUILDING;              
                _ActiveObjects.Add(_BuildCanvas);
                gen.mapEditor.enabled = true;
                break;
            case 1:
                gen.mode = MapGen.Mode._DRAWING;
                _ActiveObjects.Add(_DrawCanvas);
                gen.fillColors();
                gen.mapEditor.enabled = true;
                break;
            case 2:
                gen.mode = MapGen.Mode._ChangeSize;
                _ActiveObjects.Add(_ChangeSizeCanvas);
                _ActiveObjects.Add(_ChangeSizeButtons);
                gen.mapsizeEditor.enabled = true;
                break;
            case 3:
                gen.mode = MapGen.Mode._SETSPAWNS;
                _ActiveObjects.Add(_SpawnCanvas);
                gen.mapPlayerSelect.enabled = true;
                break;
            case 4:
                gen.mode = MapGen.Mode._SETTILEEFFECTS;
                _ActiveObjects.Add(_TileEffectCanvas);
                break;
            case 5:
                gen.mode = MapGen.Mode._SAVE;
                _ActiveObjects.Add(_SaveCanvas);
                break;

        }


        if(gen.mode == MapGen.Mode._BUILDING)
        {
            if(!firstRun)
            {
                gen.EnableBuilding();
            }
            firstRun = false;
            
        }
        else
        {
            gen.DisableBuilding();
        }

        foreach (var _button in _ActiveObjects)
        {
            _button.SetActive(true);
        }
    }
    public void setDrawMode(int mode)
    {
        switch (mode)
        {
            case 0:
                gen.mapEditor.mode = MapEditor.FillMode._NOFILL;
                gen.mapEditor._3D = false;
                break;
            case 1:
                gen.mapEditor.mode = MapEditor.FillMode._FILLGROUND;
                gen.mapEditor._3D = false;
                break;
            case 2:
                gen.mapEditor.mode = MapEditor.FillMode._FILLWALLS;
                gen.mapEditor._3D = true;
                break;
            case 3:
                gen.mapEditor.mode = MapEditor.FillMode._FILLWALLGROUND;
                gen.mapEditor._3D = true;
                break;
            case 4:
                gen.mapEditor.mode = MapEditor.FillMode._FILLFULL;
                gen.mapEditor._3D = true;
                break;
        }
    }
    public void Draw()
    {
        gen.mapEditor._Fill = true;
        gen.mapEditor._Erase = false;
    }
    public void Erase()
    {
        gen.mapEditor._Erase = true;
        gen.mapEditor._Fill = false;
    }



    //This actives the failsafe button for if the player accidently click the clear all button
    public void failSafeFullReset()
    {
        failSafeBtn.SetActive(true);
        resetYes.onClick.RemoveAllListeners();
        resetYes.onClick.AddListener(gen.mapEditor.ResetAll);
    }

    //This actives the failsafe button for if the player accidently click the clear floor button
    public void failSafeReset()
    {
        failSafeBtn.SetActive(true);
        resetYes.onClick.RemoveAllListeners();
        resetYes.onClick.AddListener(gen.mapEditor.resetFloor);
    }
    public void UIToggle(GameObject button)
    {
        button.SetActive(false);
    }
    public void EnableMenu()
    {
        _DrawMenu.SetActive(!_DrawMenu.activeSelf);
    }

    //For increasing the size of the map
    public void SetMapChangeMode(bool increase)
    {
        if(increase)
        {
            gen.mapsizeEditor.increase = true;
        }
        else
        {
            gen.mapsizeEditor.increase = false;
        }
    }


}
