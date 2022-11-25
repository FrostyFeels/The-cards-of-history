using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{

    [SerializeField] private SOmap attackMap;
    [SerializeField] private List<SOmap> levelMap;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private MapStats stats;
    //[SerializeField] private List<MapClass> maps;

    public bool melee, air;

    public int maxHeighDiff;
    public bool airAbove;


    //AT THE MOMENT IS NOT IN USE

    void Start()
    {
        //maps = mapGenerator.maps;


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            levelMap = stats.map;
            //attack();
        }
    }
}

/*public void attack()
{

    for (int i = 0; i < stats.attackSpots.Count; i++)
    {
        stats.attackSpots[i].GetComponent<Renderer>().material = stats.attackSpots[i].defaultMaterial;
    }

    Vector3 _CurrentTile = GetComponent<playerMovement>().currentLocation;
    Vector2 start = new Vector2(_CurrentTile.z, _CurrentTile.x);


    foreach (MapData _data in attackMap.map)
    {
        if (_data._selected)
        {
            Vector2 _START = new Vector3(_data.zPos, _data.xPos); //0,0 
            Vector2 _REALSTART = _START - attackMap.midPoint;
            Vector2 _FINALPOSITION = start + _REALSTART;

            Debug.Log(_FINALPOSITION);

            if (_FINALPOSITION.x < 0 || _FINALPOSITION.x >= levelMap[0].gridArray.GetLength(0) || _FINALPOSITION.y < 0 || _FINALPOSITION.y >= levelMap[0].gridArray.GetLength(0)) { }
            else
            {
                int height = 0;
                height = setHeight(height, _CurrentTile, _FINALPOSITION);


                Renderer renderer = levelMap[height].gridArray[(int)_FINALPOSITION.x, (int)_FINALPOSITION.y].GetComponent<Renderer>();

                if (renderer != null)
                {
                    renderer.material = previewMaterial;
                }

                TileStats _tile = levelMap[height].gridArray[(int)_FINALPOSITION.x, (int)_FINALPOSITION.y].GetComponent<TileStats>();

                stats.attackSpots.Add(_tile);
                //tileSelecter.selectedTile = null;
            }

        }
    }

}*/



/*    public int setHeight(int height, Vector3 _tile, Vector2 _FINALPOSITION)
    {
        if (air)
        {
            for (int i = 0; i < maxHeighDiff && i < levelMap.Count; i++)
            {
                if (stats.heightmap[(int)_FINALPOSITION.x, i, (int)_FINALPOSITION.y] == 1)
                {
                    if (i + 1 < levelMap.Count)
                    {
                        if (stats.heightmap[(int)_FINALPOSITION.x, i + 1, (int)_FINALPOSITION.y] == 0)
                        {
                            height = i;
                        }
                    }
                }
            }
        }

        if (melee)
        {
            height = (int)_tile.y - 1;
        }

        return height;
    }
}*/
