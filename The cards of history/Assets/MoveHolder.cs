using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHolder : MonoBehaviour
{
    public Character character;
    public Transform[] moveSpots;
    public Transform camera;

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
        Vector3 camRot = new Vector3(camera.rotation.x, camera.rotation.y - 90, camera.rotation.z);
        transform.rotation = camera.rotation;
    }

    public void SetMoves()
    {
    }

}
