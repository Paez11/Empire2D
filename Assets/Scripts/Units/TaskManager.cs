using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    bool IsGathering = false;
    bool CanGather = true;
    [SerializeField] float TimeToGather = 2;
    Inventory inventory;
    GameRTSController gameRTSController;
    MovePositionDirect movePositionDirect;

    GatheringManager gatheringManager;
    public GameObject TargetResource;

    private Animator animator;
    public ItemType currentResourceType = ItemType.None;
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
        if(IsGathering)
        {
            // Reset animator parameters
            if (currentResourceType.Equals(ItemType.None))
            {
                animator.SetBool("Feling", false);
                animator.SetBool("Farming", false);
                animator.SetBool("Mining", false);
            }
            Gather();
        }
            
    }
    void Gather()
    {
        // If there are no resources in the inventory
        if (inventory.ItemTypes.Count < inventory.maxItemsInventory)
        {
            movePositionDirect.movePosition = TargetResource.transform.position;
            if (Vector3.Distance(TargetResource.transform.position, transform.position) < 2.1)
            {
                if (CanGather)
                {
                    // Check if the target resource has changed
                    if (TargetResource.GetComponent<ResourceType>().resource != currentResourceType)
                    {
                        currentResourceType = TargetResource.GetComponent<ResourceType>().resource;
                        switch (currentResourceType)
                        {
                            case ItemType.Wood:
                                animator.SetBool("Feling", true);
                                break;
                            case ItemType.Food:
                                animator.SetBool("Farming", true);
                                break;
                            case ItemType.Stone:
                                animator.SetBool("Mining", true);
                                break;
                            case ItemType.Gold:
                                animator.SetBool("Mining", true);
                                break;
                        }
                    }
                    
                    StartCoroutine(Resource());
                }
            }
            //continue IsGathering
        }
        else
        {
            //if inventory full drop to the centerTown
            animator.SetBool("Feling", false);
            animator.SetBool("Farming", false);
            animator.SetBool("Mining", false);
            movePositionDirect.movePosition = GameObject.Find("TownCenter").transform.position;
            if (Vector3.Distance(GameObject.Find("TownCenter").transform.position, transform.position) < 6)
            {
                foreach (ItemType item in inventory.ItemTypes)
                {
                    switch (item)
                    {
                        case ItemType.Wood:
                            gatheringManager.Wood++;
                            break;
                        case ItemType.Food:
                            gatheringManager.Food++;
                            break;
                        case ItemType.Stone:
                            gatheringManager.Stone++;
                            break;
                        case ItemType.Gold:
                            gatheringManager.Gold++;
                            break;
                        default:
                            gatheringManager.None++;
                            break;
                    }   
                }
                inventory.ItemTypes.Clear();
                if (TargetResource.GetComponent<ResourceType>().resource == currentResourceType)
                {
                    switch (currentResourceType)
                    {
                        case ItemType.Wood:
                            animator.SetBool("Feling", true);
                            break;
                        case ItemType.Food:
                            animator.SetBool("Farming", true);
                            break;
                        case ItemType.Stone:
                            animator.SetBool("Mining", true);
                            break;
                        case ItemType.Gold:
                            animator.SetBool("Mining", true);
                            break;
                    }
                }
            }
        }
    }

    IEnumerator Resource()
    {
        CanGather = false;
        yield return new WaitForSeconds(TimeToGather);
        if(Vector3.Distance(TargetResource.transform.position, transform.position) < 2.1)
        {
            inventory.ItemTypes.Add(TargetResource.GetComponent<ResourceType>().resource);
        }
        
        CanGather = true;
    }


    public void StartGathering(GameObject targetResource)
    {
        TargetResource = targetResource;
        IsGathering = true;
    }

    public void StopGathering()
    {
        TargetResource = null;
        IsGathering = false;
    }
}
