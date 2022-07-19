using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;

    [SerializeField] float distanceFromCamera;

    private Vector3 previousPosition;

    public float scrollSpeed;
    public float speed;

    public bool canMove;

    [SerializeField] private MapStats map;

    // Update is called once per frame

    public void Start()
    {
        target.transform.position = new Vector3(map.map[0].gridSizeX, 0, -map.map[0].gridSizeY);
        previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

        cam.transform.position = target.transform.position;

        cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
        cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
        cam.transform.Translate(new Vector3(0, 0, distanceFromCamera));

        previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
    }
    void Update()
    {

        Transform camTransform = Camera.main.transform;
        Vector3 camPosition = new Vector3(camTransform.position.x, transform.position.y, camTransform.position.z);
        Vector3 direction = (transform.position - camPosition).normalized;


        if(canMove)
        {
            doCameraThingy();
        }
        else
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        
        


        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Camera.main.transform.right * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Camera.main.transform.right * Time.deltaTime * speed;
        }


        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += direction * Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition -= direction * Time.deltaTime * speed;
        }
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
    }
}
