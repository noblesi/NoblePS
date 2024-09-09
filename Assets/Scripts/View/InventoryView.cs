using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;
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
            AddItemToUI(item);
        }
    }

    public void OnItemAdded(Item item)
    {
        AddItemToUI(item);
    }

    public void OnItemRemoved(Item item)
    {
        RemoveItemFromUI(item);
    }

    public void UpdateView()
    {
        foreach(var slot in slots)
        {
            slot.UpdateSlot();
        }
    }

    private void AddItemToUI(Item item)
    {
        InventorySlot emptySlot = slots.Find(slot => slot.IsEmpty());

        if(emptySlot != null)
        {
            emptySlot.AddItem(item);
            emptySlot.UpdateSlot();
        }
        else
        {
            Debug.LogError("¿Œ∫•≈‰∏Æ∞° ∞°µÊ √°Ω¿¥œ¥Ÿ.");
        }
    }

    private void RemoveItemFromUI(Item item)
    {
        InventorySlot slotToRemove = slots.Find(slot => slot.item == item);
        if (slotToRemove != null)
        {
            slots.Remove(slotToRemove);
            Destroy(slotToRemove.gameObject);  // ΩΩ∑‘¿ª ¡¶∞≈
        }
    }

    private void ClearSlots()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlot();
        }
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
    }
}
