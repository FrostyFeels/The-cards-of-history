using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;

    [SerializeField] float distanceFromCamera;

    private Vector3 previousPosition;

    public float scrollSpeed;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        doCameraThingy();
    }


    public void doCameraThingy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = target.transform.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            cam.transform.Translate(new Vector3(0, 0, distanceFromCamera));

            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);


        }



        distanceFromCamera += Input.mouseScrollDelta.y;


        if (Input.GetKey(KeyCode.A))
        {
            target.transform.localPosition += Vector3.left * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            target.transform.localPosition += Vector3.right * Time.deltaTime * speed;
        }


        if (Input.GetKey(KeyCode.W))
        {
            target.transform.localPosition += Vector3.forward * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            target.transform.localPosition += Vector3.back * Time.deltaTime * speed;
        }
    }
}
