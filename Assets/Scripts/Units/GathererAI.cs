using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererAI : MonoBehaviour
{
    private enum State {
        Idle,
        MovingToResourceNode,
        GatheringResource,
        MovingToStorage
    }
    private IUnit unit;
    private State state;
    private ResourceNode resourceNode;
    private int InventoryAmount;
    private Transform storageTransform;

    private void Awake() 
    {
        // unit = gameObject.GetComponent<IUnit>();
        // state = State.Idle;
        /*
        unit.MoveTo(goldNodeTransform.position, 10f, () => {
            unit.PlayAnimationWork(goldNodeTransform.position, () => {
                unit.MoveTo(storageTransform.position, 5f, null);
            });
        });
        */  
    }

    void Update()
    {
        /*
        switch(state)
        {
            case State.Idle:
                resourceNode = GameRTSController.GetResourceNode_Static();
                if(resourceNode != null)
                {
                    state = State.MovingToResourceNode;
                }
            break;
            case State.MovingToResourceNode:
                if(unit.IsIdle())
                {
                    unit.MoveTo(resourceNode.GetPosition(), 10f, () =>{
                        state = State.GatheringResource;
                    });
                }
            break;
            case State.GatheringResource:
                if(unit.IsIdle())
                {
                    if(InventoryAmount >= 10) {
                        //Move to storage
                        storageTransform = GameRTSController.GetStorage_Static();
                        resourceNode = GameRTSController.GetResourceNodeNearPosition_Static(resourceNode.GetPosition());
                        state = State.MovingToStorage;
                    }else
                    {
                        //Gather
                        unit.PlayAnimationWork(resourceNode.GetPosition(), () => {
                            resourceNode.GrabResource();
                            InventoryAmount++;
                        });
                    }
                }
            break;
            case State.MovingToStorage:
                if(unit.IsIdle())
                {
                    unit.MoveTo(storageTransform.position, 10f, () =>{
                        InventoryAmount = 0;
                        state = State.GatheringResource;
                    });
                }
            break;
        }
        */
    }

    public void SetResourceNode(ResourceNode resourceNode)
    {
        this.resourceNode = resourceNode;
    }
}
