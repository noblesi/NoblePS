using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryView
{
    void ShowItems(List<Item> items);
    void OnItemAdded(Item item);
    void OnItemRemoved(Item item);
}
