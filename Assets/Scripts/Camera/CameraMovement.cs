using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 35f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;
    //[SerializeField] private float edgeScrollingSpeed = 10f;

    private Camera mainCamera;
    private Vector3 lastMousePosition;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Move the camera using the arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Move the camera using the mouse position relative with the screen
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (mouseX < screenWidth * 0.01f)
        {
            horizontalInput = -1f;
        }
        else if (mouseX > screenWidth * 0.99f)
        {
            horizontalInput = 1f;
        }

        if (mouseY < screenHeight * 0.01f)
        {
            verticalInput = -1f;
        }
        else if (mouseY > screenHeight * 0.99f)
        {
            verticalInput = 1f;
        }

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * movementSpeed * Time.deltaTime);



        // Zoom the camera using the scroll wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomInput * zoomSpeed, minZoom, maxZoom);
        
        if(Input.GetKey(KeyCode.LeftShift)){
            movementSpeed = 45f;
            zoomSpeed = 20.0f;
        }else{
            movementSpeed = 35.0f;
            zoomSpeed = 10.0f;
        }

        // Rotate the camera when Ctrl + Left Click is pressed and the mouse is moving
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1) && Input.mousePosition != lastMousePosition){
            getCameraRotation();
        }else{
            
        }
        lastMousePosition = Input.mousePosition;
    }

    public Quaternion getCameraRotation(){
        Vector3 mousePosition = Input.mousePosition;
        float rotationSpeed = 0.1f;

        float deltaX = mousePosition.x - lastMousePosition.x;
        transform.RotateAround(transform.position, Vector3.forward, -deltaX * rotationSpeed);

        float deltaY = mousePosition.y - lastMousePosition.y;
        transform.RotateAround(transform.position, Vector3.forward, deltaY * rotationSpeed);

        
        return transform.rotation;
    }
}
