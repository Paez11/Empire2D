using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class GameRTSController : MonoBehaviour
{
    [SerializeField] private Transform[] goldNodeTransformArray;
    [SerializeField] private Transform storageTransform;

    [SerializeField] private Transform selectionAreaTransform;
    [SerializeField] LayerMask resourceLayer;
    private Vector3 startPosition;
    private List<UnitRTS> selectedUnitRTSList;

    private CameraMovement cameraMovement;

    public GameObject SelectedGameObject = null;

    private static GameRTSController instance;
    private List<ResourceNode> resourceNodeList;
    [SerializeField] private GathererAI gathererAI;

    private void Awake() {

        selectedUnitRTSList = new List<UnitRTS>();
        selectionAreaTransform.gameObject.SetActive(false);
        cameraMovement = FindObjectOfType<CameraMovement>();

        //TaskManager.OnResourceClicked += Resource_OnResourceClicked;
        ResourceNode.OnResourceNodeClicked += Resource_OnResourceClicked;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenu.isPaused){

            if (Input.GetMouseButtonDown(0))
            {
                //Left Mouse Button Pressed
                selectionAreaTransform.gameObject.SetActive(true);
                startPosition = UtilsClass.GetMouseWorldPosition();
            }

            if(Input.GetMouseButton(0))
            {
                //Left Mouse Button Held Down
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // Se realiza la selección del área en el mapa
                    GetSelectedArea();
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                //Left Mouse Button Released
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // Seleccionar las unidades en el área seleccionada
                    UnitsSelect();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                // Right Mouse button pressed
                UnitsActionPerformance();
            }
        }
    }

    /// <summary>
            //List for value position for objects to create a circle
            //moveToPosition --> click coordenates 
            //2º distance between objects
            //3º position around the circle
    ///</summary>
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray){
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount){
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i*(360f/positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1,0),angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle){
        return Quaternion.Euler(0,0, angle) * vec;
    }

    private void GetSelectedArea()
    {
        Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 lowerLeft = new Vector3(
            Mathf.Min(startPosition.x, currentMousePosition.x),
            Mathf.Min(startPosition.y, currentMousePosition.y)
        );
        Vector3 upperRight = new Vector3(
            Mathf.Max(startPosition.x, currentMousePosition.x),
            Mathf.Max(startPosition.y, currentMousePosition.y)
        );
        selectionAreaTransform.position = lowerLeft;
        selectionAreaTransform.localScale = upperRight -lowerLeft;
    }

    private void UnitsSelect()
    {
        selectionAreaTransform.gameObject.SetActive(false);

        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

        //Deselect All Units
        foreach (UnitRTS unitRTS in selectedUnitRTSList)
        {
            unitRTS.SetSelectedVisible(false);
        }
        
        selectedUnitRTSList.Clear();

        //Select All Units within an selection Area
        foreach (Collider2D collider2D in collider2DArray)
        {
            UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
            if (unitRTS != null){
                unitRTS.SetSelectedVisible(true);
                selectedUnitRTSList.Add(unitRTS);
            }
        }

        if(selectedUnitRTSList.Count > 0)
        {
            cameraMovement.isCharacterSelected = true;
        }else
        {
            cameraMovement.isCharacterSelected = false;
        }
    }
    private void UnitsActionPerformance()
    {
        Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();
        List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] {1f, 5f, 10f}, new int[] {5, 10, 20});

        int targetPositionListIndex = 0;
        foreach (UnitRTS unitRTS in selectedUnitRTSList)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.one, 0.1f, resourceLayer);
            Debug.Log(hit.normal);
            Debug.DrawRay(mousePosition, Vector2.zero, Color.red);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Resource"))
                {
                    SelectedGameObject = hit.collider.gameObject;

                    if (SelectedGameObject != null)
                    {
                        unitRTS.GetComponent<TaskManager>().currentResourceType = ItemType.None;
                        unitRTS.GetComponent<TaskManager>().StartGathering(hit.collider.gameObject);
                    }
                }
                else
                {
                    unitRTS.GetComponent<TaskManager>().currentResourceType = ItemType.None;
                    unitRTS.GetComponent<TaskManager>().StopGathering();
                    unitRTS.MoveTo(targetPositionList[targetPositionListIndex]);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
            }
            else
            {
                unitRTS.MoveTo(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    private void Resource_OnResourceClicked(object sender, EventArgs e)
    {
        ResourceNode resourceNode = sender as ResourceNode;
        gathererAI.SetResourceNode(resourceNode);
        //TaskManager taskManager = sender as TaskManager;
    }

    /*

    public ResourceNode GetResourceNode()
    {
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);
        for(int i=0; i < tmpResourceNodeList.Count; i++){
            if(!tmpResourceNodeList[i].HasResource()){
                //No more resource
                tmpResourceNodeList.RemoveAt(i);
                i--;
            }
        }
        if(tmpResourceNodeList.Count > 0)
        {
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];
        }
        else 
        {
            return null;
        }

    }
    public ResourceNode GetResourceNodeNearPosition(Vector3 position)
    {
        float maxDistance = 20f;
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);
        for(int i=0; i < tmpResourceNodeList.Count; i++){
            if(!tmpResourceNodeList[i].HasResource() || Vector3.Distance(position, tmpResourceNodeList[i].GetPosition()) > maxDistance){
                //No more resource
                tmpResourceNodeList.RemoveAt(i);
                i--;
            }
        }
        if(tmpResourceNodeList.Count > 0)
        {
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];
        }
        else 
        {
            return null;
        }

    }
    private Transform GetStorage()
    {
        return storageTransform;
    }
    public static Transform GetStorage_Static()
    {
        return instance.GetStorage();
    }
    public static ResourceNode GetResourceNode_Static()
    {
        return instance.GetResourceNode();
    }
    public static ResourceNode GetResourceNodeNearPosition_Static(Vector3 position)
    {
        return instance.GetResourceNodeNearPosition(position);
    }
    */
}
