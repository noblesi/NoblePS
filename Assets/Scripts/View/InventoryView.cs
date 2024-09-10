using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    public Transform itemsParent;
    public Tooltip tooltip;

    [SerializeField]private List<InventorySlot> slots = new List<InventorySlot>();

    private InventoryPresenter inventoryPresenter;

    public void Initialize(InventoryPresenter presenter)
    {
        inventoryPresenter = presenter;
        inventoryPresenter.Initialize();

        foreach(var slot in slots)
        {
            slot.Initialize(inventoryPresenter, tooltip);
        }
    }

    public void ShowItems(List<Item> items)
    {
        ClearSlots();
        foreach(var item in items)
        {
            OnItemAdded(item);
        }
    }

    public void OnItemAdded(Item item)
    {
        InventorySlot emptySlot = slots.Find(slot => slot.IsEmpty());
        if(emptySlot != null)
        {
            emptySlot.AddItem(item);
            emptySlot.UpdateSlot();
        }
    }

    public void OnItemRemoved(Item item)
    {
        InventorySlot slotToRemove = slots.Find(slot => slot.item == item);
        if(slotToRemove != null)
        {
            slotToRemove.ClearSlot();
        }
    }

    

    private void ClearSlots()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlot();
        }
    }
}
