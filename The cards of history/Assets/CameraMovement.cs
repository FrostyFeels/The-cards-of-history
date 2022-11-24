using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;

    [SerializeField] private float distanceFromCamera;

    private Vector3 previousPosition;

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float speed;

    public bool canMove;
    public bool isMoving;

    [SerializeField] private MapStats map;

    [SerializeField] private CharacterPathLogic pathlogic;

    public Transform rotateobject;

    // Update is called once per frame

    public void Start()
    {
        target.transform.position = new Vector3(map.map[0].gridSizeX, 0, -map.map[0].gridSizeY);
    }
    void Update()
    {

        Transform camTransform = Camera.main.transform;
        Vector3 camPosition = new Vector3(camTransform.position.x, transform.position.y, camTransform.position.z);
        Vector3 direction = (transform.position - camPosition).normalized;


        if(canMove || !pathlogic.enabled)
        {
            CameraRotation();
        }
        else
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
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

    public bool CalculateRotation()
    {
        Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

        if(rotateobject == null) 
            rotateobject = new GameObject().transform;

        Transform fakeCam = rotateobject;
        fakeCam.rotation = cam.transform.rotation;
        fakeCam.position = cam.transform.position;
        fakeCam.position = target.transform.position;

       
        fakeCam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
        fakeCam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
        fakeCam.transform.Translate(new Vector3(0, 0, distanceFromCamera));



        if (fakeCam.eulerAngles.x > 15 && fakeCam.eulerAngles.x < 75)
        {
            return true;
        }
        else
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            return false;
        }
    } 


    public void CameraRotation()
    {

        distanceFromCamera += Input.mouseScrollDelta.y;

        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            isMoving = true;
        }


       /* if (!CalculateRotation())
            return;*/

        if (Input.GetMouseButton(0))
        {

            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = target.transform.position;


           
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            cam.transform.Translate(new Vector3(0, 0, distanceFromCamera));



            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }



   
    }
}
