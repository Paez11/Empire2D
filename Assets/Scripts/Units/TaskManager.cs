using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] TMP_Text woodResourceText;
    private int wood;
    [SerializeField] TMP_Text foodResourceText;
    private int food;
    [SerializeField] TMP_Text stoneResourceText;
    private int stone;
    [SerializeField] TMP_Text goldResourceText;
    private int gold;

    [SerializeField] AudioClip[] pickupSound;
    [SerializeField] AudioSource selectedResourceSound;
    [SerializeField] AudioClip[] gatherResourceSound;
    [SerializeField] AudioSource resourceSound;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        movePositionDirect = GetComponent<MovePositionDirect>();
        gameRTSController = GameObject.Find("GameRTSController").GetComponent<GameRTSController>();
        gatheringManager = GameObject.Find("GatheringManager").GetComponent<GatheringManager>();
        animator = GetComponent<Animator>();

        woodResourceText = transform.Find("WoodResourceText").GetComponent<TMP_Text>();
        foodResourceText = transform.Find("FoodResourceText").GetComponent<TMP_Text>();
        stoneResourceText = transform.Find("StoneResourceText").GetComponent<TMP_Text>();
        goldResourceText = transform.Find("GoldResourceText").GetComponent<TMP_Text>();
        UpdateInventoryText();
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
            UpdateInventoryText();
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
                                StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[1], 1.2f));
                                break;
                            case ItemType.Food:
                                animator.SetBool("Farming", true);
                                StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[0], 1.2f));
                                break;
                            case ItemType.Stone:
                                animator.SetBool("Mining", true);
                                StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[2], 1.2f));
                                break;
                            case ItemType.Gold:
                                animator.SetBool("Mining", true);
                                StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[3], 1.2f));
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
            resourceSound.Stop();
            movePositionDirect.movePosition = GameObject.Find("TownCenter").transform.position;
            if (Vector3.Distance(GameObject.Find("TownCenter").transform.position, transform.position) < 6)
            {
                foreach (ItemType item in inventory.ItemTypes)
                {
                    switch (item)
                    {
                        case ItemType.Wood:
                            gatheringManager.Wood++;
                            wood++;
                            break;
                        case ItemType.Food:
                            gatheringManager.Food++;
                            food++;
                            break;
                        case ItemType.Stone:
                            gatheringManager.Stone++;
                            stone++;
                            break;
                        case ItemType.Gold:
                            gatheringManager.Gold++;
                            gold++;
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
        if(TargetResource != null)
        {
            if(Vector3.Distance(TargetResource.transform.position, transform.position) < 2.1)
            {
                inventory.ItemTypes.Add(TargetResource.GetComponent<ResourceType>().resource);
            }
            
            CanGather = true;
        }
        else
        {
            currentResourceType = ItemType.None;
        }
    }


    public void StartGathering(GameObject targetResource)
    {
        TargetResource = targetResource;
        IsGathering = true;
        int randomIndex = Random.Range(0, 2);
        selectedResourceSound.clip = pickupSound[randomIndex];
        selectedResourceSound.Play();
    }

    public void StopGathering()
    {
        TargetResource = null;
        IsGathering = false;
    }

    private void UpdateInventoryText()
    {
        if(inventory.ItemTypes.Count > 0)
        {
            switch(inventory.ItemType)
            {
                case ItemType.Wood:
                    woodResourceText.text = "" + wood;
                    break;
                case ItemType.Food:
                    foodResourceText.text = "" + food;
                    break;
                case ItemType.Stone:
                    stoneResourceText.text = "" + stone;
                    break;
                case ItemType.Gold:
                    goldResourceText.text = "" + gold;
                    break;
                default:
                    
                    break;
            }
        }
        else
        {
            woodResourceText.text = "0";
            foodResourceText.text = "0";
            stoneResourceText.text = "0";
            goldResourceText.text = "0";
        }
    }

    private IEnumerator PlayDelayedLoopedSound(AudioClip soundClip, float delay)
    {
    AudioSource resourceSound = GetComponent<AudioSource>();

    while (true)
    {
        resourceSound.clip = soundClip;
        resourceSound.Play();
        yield return new WaitForSeconds(delay);
    }
    }
}
