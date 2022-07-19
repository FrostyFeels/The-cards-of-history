using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public List<Vector3> path;
    [SerializeField] private Vector3 lastTile;
    [SerializeField] private Vector3 curTile;

    [SerializeField] private LayerMask tileMask;

    [SerializeField] private CameraMovement cam;

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DragPath();
        }          
        else
        {
            cam.canMove = true;
        }
    }

    public void DragPath()
    {

        cam.canMove = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, tileMask))
        {
            Vector3 tile = hit.collider.GetComponent<TileStats>()._ID;

            if (curTile == null)
            {
                curTile = tile;
                path.Add(curTile);
            }
                

            if (curTile == tile)
            {
                return;
            }


            if (tile == lastTile)
            {
                path.Remove(curTile);

                if (path.Count > 1)
                {
                    lastTile = path[path.Count - 2];
                }
                
                curTile = tile;
                return;
            }

            lastTile = curTile;
            curTile = tile;
            path.Add(curTile);
            
            





            
                

        }
    }

}
