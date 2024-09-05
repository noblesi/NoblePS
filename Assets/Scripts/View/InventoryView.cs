using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;
    public Tooltip tooltip;

    private List<InventorySlot> slots = new List<InventorySlot>();

    private InventoryPresenter inventoryPresenter;

    public void Initialize(InventoryPresenter presenter)
    {
        inventoryPresenter = presenter;
        inventoryPresenter.Initialize();
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
        GameObject slotGO = Instantiate(inventorySlotPrefab, itemsParent);
        InventorySlot slot = slotGO.GetComponent<InventorySlot>();
        slot.Initialize(inventoryPresenter, tooltip);  // ���Կ� �������Ϳ� ���� ����
        slot.AddItem(item);  // ���Կ� ������ �߰�
        slots.Add(slot);  // ���� ����Ʈ�� �߰�
    }

    private void RemoveItemFromUI(Item item)
    {
        InventorySlot slotToRemove = slots.Find(slot => slot.item == item);
        if (slotToRemove != null)
        {
            slots.Remove(slotToRemove);
            Destroy(slotToRemove.gameObject);  // ������ ����
        }
    }

    private void ClearSlots()
    {
        foreach(var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }

    public void ShowTooltip(string description)
    {
        tooltip.ShowTooltip(description);
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
    }
}
