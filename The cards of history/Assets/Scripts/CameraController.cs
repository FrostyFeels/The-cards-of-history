using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;

    [SerializeField] private float distanceFromCamera;

    private Vector3 previousPosition;

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float speed;

    public bool canMove;
    public bool isMoving;

    [SerializeField] private MapGen gen;

    private float BuildrangeDivider;

    // Update is called once per frame

    public void Start()
    {
        target.transform.position = new Vector3(gen.map[0].gridSizeX, gen.mapEditor.buildRange / BuildrangeDivider, -gen.map[0].gridSizeY);
    }
    void Update()
    {

        Transform camTransform = Camera.main.transform;
        Vector3 camPosition = new Vector3(camTransform.position.x, transform.position.y,camTransform.position.z);
        Vector3 direction = (transform.position - camPosition).normalized;

        if(canMove)
        {
            CameraRotation();
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


    //Rotates te camera so you can move around the map
    public void CameraRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
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

        if(Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }

        distanceFromCamera += Input.mouseScrollDelta.y;
    }
}
