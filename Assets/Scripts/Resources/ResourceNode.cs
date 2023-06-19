using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System;

public class ResourceNode
{
    public static event EventHandler OnResourceNodeClicked;
    private Transform resourceNodeTransform;
    private int resourceAmount;

    public ResourceNode(Transform resourceNodeTransform)
    {
        this.resourceNodeTransform = resourceNodeTransform;
        resourceAmount = 0;
        resourceNodeTransform.GetComponent<Button_Sprite>().ClickFunc = () => {
            if(OnResourceNodeClicked != null) OnResourceNodeClicked(this, EventArgs.Empty);
        };
    }

    public Vector3 GetPosition()
    {
        return resourceNodeTransform.position;
    }

    public void GrabResource(){
        resourceAmount -= 1;
        // if(resourceAmount <= 0)
        // {
        //     resourceNodeTransform.GetComponent<SpriteRenderer>().sprite = 
        // }
    }

    public bool HasResource()
    {
        return resourceAmount > 0;
    }
}
