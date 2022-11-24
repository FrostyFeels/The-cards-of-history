using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeInceaseButton : MonoBehaviour
{
    [SerializeField] private MapGen gen;


    public bool up, down, left, right;
    public float tileSize = 0.2f;

    private void start()
    {
        gen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
        tileSize = (float)gen.map[0].tileSize / 5;
        Debug.Log(tileSize);
        Debug.Log((float)gen.map[0].tileSize / 5);
    }


    //TODO: again figure out the method instead of (-1 * .5f and 1.5f * tilesize)
    public void setPositions()
    {
        if (gen == null)
        {
            gen = GameObject.Find("MapGenerator").GetComponent<MapGen>();
        }

        //This divides the tilesize so that the ui fits nicely
        tileSize = (float)gen.map[0].tileSize / 5;


        if (up)
        {
            transform.localScale = new Vector2(gen.map[gen.currentMapLevel].gridSizeX * tileSize, transform.localScale.y);
            transform.position = new Vector3((gen.map[gen.currentMapLevel].gridSizeX - 1) * .5f, 0, 1.5f) * gen.map[0].tileSize;
        }
        if (down)
        {
            transform.localScale = new Vector2(gen.map[0].gridSizeX * tileSize, transform.localScale.y);
            transform.position = new Vector3((gen.map[gen.currentMapLevel].gridSizeX - 1) * .5f, 0, (-gen.map[gen.currentMapLevel].gridSizeY) - .5f) * gen.map[0].tileSize;
        }

        if (right)
        {

            transform.localScale = new Vector2(transform.localScale.x, gen.map[gen.currentMapLevel].gridSizeY * tileSize);
            transform.position = new Vector3((gen.map[gen.currentMapLevel].gridSizeX) + .5f, 0, -(gen.map[gen.currentMapLevel].gridSizeY - 1) * .5f) * gen.map[0].tileSize;
        }
        if (left)
        {

            transform.localScale = new Vector2(transform.localScale.x, gen.map[gen.currentMapLevel].gridSizeY * tileSize);
            transform.position = new Vector3(-1.5f, 0, -(gen.map[gen.currentMapLevel].gridSizeY - 1) * .5f) * gen.map[0].tileSize;
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
