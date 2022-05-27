using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelecter : MonoBehaviour
{

    [SerializeField] private Material selectMaterial, defaultmaterial;
    [SerializeField] private GameObject selectedTile;
    [SerializeField] private Renderer selectedRenderer;

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectObject();
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

}
