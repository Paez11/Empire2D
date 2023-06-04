using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    None,
    Wood,
    Food,
    Stone,
    Gold
}
public class Inventory : MonoBehaviour
{
    public int itemsInventory = 0;
    public int maxItemsInventory = 10;
    public itemType itemType;

    List<itemType> itemTypes;
    
}
