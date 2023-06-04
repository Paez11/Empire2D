using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    bool cuttingDown = false;
    bool canCutDown = true;
    [SerializeField] float timeToCutDown = 2;
    Inventory inventory;
    GameRTSController gameRTSController;
    MovePositionDirect movePositionDirect;

    GatheringManager gatheringManager;
    public GameObject TargetTree;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        movePositionDirect = GetComponent<MovePositionDirect>();
        gameRTSController = GameObject.Find("GameRTSController").GetComponent<GameRTSController>();
        gatheringManager = GameObject.Find("GatheringManager").GetComponent<GatheringManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cuttingDown)
            CutDown();
    }
    void CutDown()
    {
        //if he doesn´t any resources in his inventory
        if(inventory.itemsInventory < inventory.maxItemsInventory)
        {
            movePositionDirect.movePosition = TargetTree.transform.position;
            if(Vector3.Distance(TargetTree.transform.position, transform.position) < 2.1)
            {
                if(canCutDown)
                {
                    animator.SetBool("Feling", true);
                    StartCoroutine(Tree());
                }
                //cut the object
            }
            //continue cuttingdown
        }else
        {
            //if inventory full drop to the centerTown
            animator.SetBool("Feling", false);
            movePositionDirect.movePosition = GameObject.Find("TownCenter").transform.position;
            if(Vector3.Distance(GameObject.Find("TownCenter").transform.position, transform.position) < 2.1)
            {
                //Deposit Items
                if(inventory.itemType == itemType.Wood){
                    gatheringManager.Wood += inventory.itemsInventory;
                    inventory.itemsInventory = 0;
                }
            }
        }
        //find a tree
    }

    IEnumerator Tree()
    {
        canCutDown = false;
        yield return new WaitForSeconds(timeToCutDown);
        inventory.itemsInventory++;
        inventory.itemType = itemType.Wood;
        canCutDown = true;
    }


    public void StartCuttingDown(GameObject targetTree)
    {
        TargetTree = targetTree;
        cuttingDown = true;
    }
}
