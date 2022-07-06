using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileSelecter : MonoBehaviour
{

    [SerializeField] private Material selectMaterial, defaultmaterial, playerSelected;
    [SerializeField] public GameObject selectedTile;
    [SerializeField] private Renderer selectedRenderer;
    [SerializeField] private MapStats stats;

    [SerializeField] private int characterIndex = 1;

    private TextMeshPro text;

    public enum Mode
    {
        Bulding,
        PlaceCharacters
    }

    public Mode mode;
    public void Update()
    {
        if(Input.GetMouseButtonDown(0) && mode != Mode.Bulding)
        {
            SelectObject();
        }

        if(mode == Mode.PlaceCharacters)
        {
            placeCharacters();
        }


    }


    public void SelectObject()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();


            if (selection.gameObject == selectedTile && mode == Mode.PlaceCharacters)
            {

                if(stats.playerSpots[characterIndex - 1] != null)
                {
                    stats.playerSpots[characterIndex - 1].gameObject.GetComponentInParent<Renderer>().material = defaultmaterial;
                    text = stats.playerSpots[characterIndex - 1].GetComponentInChildren<TextMeshPro>();
                    text.enabled = false;
                    
                }
                                                  
                text = selection.GetComponentInChildren<TextMeshPro>();

                text.enabled = true;

                selectedRenderer.material = playerSelected;

                selectedTile = null;

                
                switch (characterIndex)
                {
                    case 1:
                        text.text = "1";
                        stats.playerSpots[0] = selection.gameObject.GetComponent<TileStats>();
                        return;
                    case 2:
                        text.text = "2";
                        stats.playerSpots[1] = selection.gameObject.GetComponent<TileStats>();
                        return;
                    case 3:
                        text.text = "3";
                        stats.playerSpots[2] = selection.gameObject.GetComponent<TileStats>();
                        return;
                    case 4:
                        text.text = "4";
                        stats.playerSpots[3] = selection.gameObject.GetComponent<TileStats>();
                        return;
                    case 5:
                        text.text = "5";
                        stats.playerSpots[4] = selection.gameObject.GetComponent<TileStats>();
                        return;
                }

                return;
            }

            if (selectionRenderer != null)
            {
                if (selectedTile == null)
                {
                    selectedTile = selection.gameObject;
                    selectedRenderer = selectionRenderer;
                    defaultmaterial = selectedRenderer.material;
                    selectedRenderer.material = selectMaterial;
                }

                if (selection.gameObject != selectedTile)
                {
                    selectedRenderer.material = defaultmaterial;

                    selectedTile = selection.gameObject;
                    selectedRenderer = selectionRenderer;
                    defaultmaterial = selectedRenderer.material;

                    selectedRenderer.material = selectMaterial;
                }

            }                 
        }
    }

    public GameObject ReturnTile()
    {
        return selectedTile;
    }

    public Material defaultMaterial()
    {
        return defaultmaterial;
    }

    public void placeCharacters()
    {
        mode = Mode.PlaceCharacters;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            characterIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            characterIndex = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            characterIndex = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            characterIndex = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            characterIndex = 5;
        }
    }


}
