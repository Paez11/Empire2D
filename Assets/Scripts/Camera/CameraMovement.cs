using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 35f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float hardMovementSpeed = 4f;
    //[SerializeField] private float edgeScrollingSpeed = 10f;
    [SerializeField] private Vector2 minCameraPosition;
    [SerializeField] private Vector2 maxCameraPosition;

    private Camera mainCamera;
    private Vector3 lastMousePosition;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Change respect the scale of the map
        minCameraPosition = new Vector2(-39f, -23f); 
        maxCameraPosition = new Vector2(39f, 23f);  
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

        //Edge of the screen
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        //camera move at edge of the screen
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

        //Move of the camera
        if (Input.GetMouseButton(1))
        {
            horizontalInput = -hardMovementSpeed*Input.GetAxis("Mouse X");
            verticalInput = -hardMovementSpeed*Input.GetAxis("Mouse Y");
        }

        //position of the camera, controlling the max and min position it can go

        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * movementSpeed * Time.deltaTime);
        Vector3 desiredTranslation = new Vector3(horizontalInput, verticalInput, 0) * movementSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + desiredTranslation;
        newPosition.x = Mathf.Clamp(newPosition.x, minCameraPosition.x, maxCameraPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraPosition.y, maxCameraPosition.y);
        transform.position = newPosition;



        // Zoom the camera using the scroll wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomInput * zoomSpeed, minZoom, maxZoom);
        // Calculate new camera position limits based on zoom level
        float zoomFactor = mainCamera.orthographicSize / maxZoom;  // Adjust this factor to control the rate of change
        Vector2 zoomedMinPosition = minCameraPosition * zoomFactor;
        Vector2 zoomedMaxPosition = maxCameraPosition * zoomFactor;
        
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
