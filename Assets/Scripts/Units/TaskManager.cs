using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class TaskManager : MonoBehaviour
{
    //public static event EventHandler OnResourceClicked;
    bool IsGathering = false;
    bool CanGather = true;
    [SerializeField] float TimeToGather = 2;
    Inventory inventory;
    GameRTSController gameRTSController;
    UnitRTS unit;
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

    private Coroutine soundCoroutine; 
    [SerializeField] AudioClip[] pickupSound;
    [SerializeField] AudioSource selectedResourceSound;
    [SerializeField] AudioClip[] gatherResourceSound;
    [SerializeField] AudioSource resourceSound;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        unit = GetComponent<UnitRTS>();
        gameRTSController = GameObject.Find("GameRTSController").GetComponent<GameRTSController>();
        gatheringManager = GameObject.Find("GatheringManager").GetComponent<GatheringManager>();
        animator = GetComponent<Animator>();

        woodResourceText.text = wood.ToString();
        foodResourceText.text = food.ToString();
        stoneResourceText.text = stone.ToString();
        goldResourceText.text = gold.ToString();
        //UpdateInventoryText();
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
                if(soundCoroutine != null)
                    StopCoroutine(soundCoroutine);
            }
            Gather();
            //UpdateInventoryText();
        }         
    }
    void Gather()
    {
        // If there are no resources in the inventory
        if (inventory.ItemTypes.Count < inventory.maxItemsInventory)
        {
            unit.MoveTo(TargetResource.transform.position);
            if (Vector3.Distance(transform.position, TargetResource.transform.position) < 1)
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
                                soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[1], 1.2f));
                                break;
                            case ItemType.Food:
                                animator.SetBool("Farming", true);
                                soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[0], 1.2f));
                                break;
                            case ItemType.Stone:
                                animator.SetBool("Mining", true);
                                soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[2], 1.2f));
                                break;
                            case ItemType.Gold:
                                animator.SetBool("Mining", true);
                                soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[3], 1.2f));
                                break;
                        }
                    }
                    
                    StartCoroutine(Resource(currentResourceType));
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
            StopCoroutine(soundCoroutine);
            unit.MoveTo(GameObject.Find("TownCenter").transform.position);
            if (Vector3.Distance(GameObject.Find("TownCenter").transform.position, transform.position) < 6)
            {
                foreach (ItemType item in inventory.ItemTypes)
                {
                    switch (item)
                    {
                        case ItemType.Wood:
                            gatheringManager.Wood++;
                            wood = 0;
                            woodResourceText.text = wood.ToString();
                            break;
                        case ItemType.Food:
                            gatheringManager.Food++;
                            food = 0;
                            foodResourceText.text = food.ToString();
                            break;
                        case ItemType.Stone:
                            gatheringManager.Stone++;
                            stone = 0;
                            stoneResourceText.text = stone.ToString();
                            break;
                        case ItemType.Gold:
                            gatheringManager.Gold++;
                            gold = 0;
                            goldResourceText.text = gold.ToString();
                            break;
                        default:
                            gatheringManager.None = 0;
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
                            soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[1], 1.2f));
                            break;
                        case ItemType.Food:
                            animator.SetBool("Farming", true);
                            soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[0], 1.2f));
                            break;
                        case ItemType.Stone:
                            animator.SetBool("Mining", true);
                            soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[2], 1.2f));
                            break;
                        case ItemType.Gold:
                            animator.SetBool("Mining", true);
                            soundCoroutine = StartCoroutine(PlayDelayedLoopedSound(gatherResourceSound[3], 1.2f));
                            break;
                    }
                }
            }
        }
    }

    IEnumerator Resource(ItemType currentResourceType)
    {
        CanGather = false;
        yield return new WaitForSeconds(TimeToGather);
        if(TargetResource != null)
        {
            if(Vector3.Distance(TargetResource.transform.position, transform.position) < 2.1)
            {
                inventory.ItemTypes.Add(TargetResource.GetComponent<ResourceType>().resource);
                switch(currentResourceType)
                {
                    case ItemType.Wood:
                        wood++;
                        woodResourceText.text = wood.ToString();
                        break;
                    case ItemType.Food:
                        food++;
                        foodResourceText.text = food.ToString();
                        break;
                    case ItemType.Stone:
                        stone++;
                        stoneResourceText.text = stone.ToString();
                        break;
                    case ItemType.Gold:
                        gold++;
                        goldResourceText.text = gold.ToString();
                        break;
                }
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
        int randomIndex = UnityEngine.Random.Range(0, 2);
        selectedResourceSound.clip = pickupSound[randomIndex];
        selectedResourceSound.Play();
    }

    public void StopGathering()
    {
        currentResourceType = ItemType.None; 
        TargetResource = null;
        IsGathering = false;
        CanGather = false;
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
