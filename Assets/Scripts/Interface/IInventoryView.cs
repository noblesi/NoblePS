using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryView
{
    void ShowItems(Dictionary<int, Item> items);
    void OnItemAdded(int slotIndex, Item item);
    void OnItemRemoved(int slotIndex);
}
