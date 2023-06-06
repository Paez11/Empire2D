using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
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
    public ItemType ItemType;

    public List<ItemType> ItemTypes;
    
}
