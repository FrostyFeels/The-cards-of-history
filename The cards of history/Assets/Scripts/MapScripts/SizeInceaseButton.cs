using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeInceaseButton : MonoBehaviour
{
    [SerializeField] private MapGen gen;


    public bool up, down, left, right;
    public float tileSize = 0.2f;

    private void Awake()
    {
        gen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
    }

    public void setPositions()
    {
        if(gen == null)
        {
            gen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
        }
        

        if (up)
        {
            transform.localScale = new Vector2(gen.map[gen.currentMapLevel].gridSizeX * tileSize, transform.localScale.y);
            transform.position = new Vector3((gen.map[gen.currentMapLevel].gridSizeX - 1) * .5f, 0, 1.5f);
        }
        if (down)
        {
            transform.localScale = new Vector2(gen.map[0].gridSizeX * tileSize, transform.localScale.y);
            transform.position = new Vector3((gen.map[gen.currentMapLevel].gridSizeX - 1) * .5f, 0, (-gen.map[gen.currentMapLevel].gridSizeY) - .5f);
        }

        if (right)
        {
            
            transform.localScale = new Vector2(transform.localScale.x, gen.map[gen.currentMapLevel].gridSizeY * tileSize);
            transform.position = new Vector3((gen.map[gen.currentMapLevel].gridSizeX) + .5f, 0, -(gen.map[gen.currentMapLevel].gridSizeY - 1) * .5f);
        }
        if (left)
        {

            transform.localScale = new Vector2(transform.localScale.x, gen.map[gen.currentMapLevel].gridSizeY * tileSize);
            transform.position = new Vector3(-1.5f, 0, -(gen.map[gen.currentMapLevel].gridSizeY - 1) * .5f);
        }
    }

    public void OnMouseDown()
    {
        if (up)
        {
            gen.mapsizeEditor.CopyMap(true, false, false, false);
        }
        if (down)
        {
            gen.mapsizeEditor.CopyMap(false, true, false, false);
        }
        if (right)
        {
            gen.mapsizeEditor.CopyMap(false, false, false, true);
        }
        if (left)
        {
            gen.mapsizeEditor.CopyMap(false, false, true, false);

        }
    }


}
