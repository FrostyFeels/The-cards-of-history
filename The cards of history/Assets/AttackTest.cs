using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [SerializeField] private TileSelecter tileSelecter;
    [SerializeField] private GameObject startTile;
    [SerializeField] private GameObject[][] JaggedMapArray;
    [SerializeField] private SOmap attackMap;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private MapGenerator map;

    void Start()
    {
        
        tileSelecter = GetComponent<TileSelecter>();
        map = GetComponent<MapGenerator>();


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            startTile = tileSelecter.ReturnTile();
            attack();
        }
    }

    public void attack()
    {
        JaggedMapArray = GetComponent<MapGenerator>().jaggedMapArray;
        Vector2 start = startTile.GetComponent<TileStats>()._ID;

        
        Vector2 realStart = start - attackMap.midPoint;

        if(realStart.x < 0)
        {
            realStart.x = 0;
        }

        if(realStart.y < 0)
        {
            realStart.y = 0;
        }



        foreach (MapData _data in attackMap.map)
        {
            if(_data.selected)
            {
                Vector2 _START = new Vector2(_data.xPos, _data.yPos); //0,0 
          
                Vector2 _REALSTART = _START - attackMap.midPoint;
                Vector2 _FINALPOSITION = start + _REALSTART;

                Debug.Log(_REALSTART);
                JaggedMapArray[(int)_FINALPOSITION.x][(int)_FINALPOSITION.y].GetComponent<Renderer>().material = previewMaterial;
            }
        }

    }



}
