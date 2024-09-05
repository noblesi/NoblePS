using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment, Consumable, Misc
}

public class Item
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public Sprite Icon { get; set; } // Á÷·ÄÈ­ X
    public GameObject ItemPrefab { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }

    public Item(int id, string name, Sprite icon, GameObject itemPrefab, ItemType type, string description, int quantity)
    {
        ItemID = id;
        ItemName = name;
        Icon = icon;
        ItemPrefab = itemPrefab;
        Type = type;
        Description = description;
        Quantity = quantity;
    }
}
