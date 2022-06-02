using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private MapStats stats;

    public Vector3 startPoint;
    public Vector3 currentLocation;



    void Start()
    {
        stats = GameObject.Find("Map").GetComponent<MapStats>();
        startPoint.x = stats.playerSpots[0]._ID.x;
        startPoint.z = stats.playerSpots[0]._ID.z;
        startPoint.y = stats.playerSpots[0]._ID.y + 1;

        currentLocation = startPoint;

        transform.position = currentLocation;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            ChangePosition(currentLocation + new Vector3(0, 0, -1));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangePosition(currentLocation + new Vector3(0, 0, 1));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangePosition(currentLocation + new Vector3(-1, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangePosition(currentLocation + new Vector3(1, 0, 0));
        }
    }


    public void ChangePosition(Vector3 newPosition)
    {
        currentLocation = newPosition;
        transform.position = currentLocation;
    }
}
