using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHolder : MonoBehaviour
{
    public Character character;
    public Transform[] moveSpots;
    public Transform _camera;


    //NOT IN USE RIGHT NOW
    public void Start()
    {
        SetMoves();
    }

    public void Update()
    {
        SetRotation();
    }

    public void SetRotation()
    {
        Vector3 camRot = new Vector3(_camera.rotation.x, _camera.rotation.y - 90, _camera.rotation.z);
        transform.rotation = _camera.rotation;
    }

    public void SetMoves()
    {
    }

}
