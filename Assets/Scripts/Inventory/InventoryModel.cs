using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private List<Item> items = new List<Item>();

    public List<Item> GetItems()
    {
        return new List<Item>(items);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
}
