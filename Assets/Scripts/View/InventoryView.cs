using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class InventoryView : MonoBehaviour, IInventoryView
{
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private List<InventorySlot> slots;
    private Dictionary<int, InventorySlot> slotDictionary = new Dictionary<int, InventorySlot>();
    private InventoryPresenter inventoryPresenter;

    public void Initialize(InventoryPresenter presenter)
    {
        inventoryPresenter = presenter;
        inventoryPresenter.Initialize();

        for(int i = 0; i < slots.Count; i++)
        {
            slotDictionary[i] = slots[i];
            slotDictionary[i].Initialize(inventoryPresenter, tooltip, i);
        }
    }

    public void ShowItems(Dictionary<int, Item> items)
    {
        ClearSlots();
        foreach(var itemPair in items)
        {
            OnItemAdded(itemPair.Key, itemPair.Value);
        }
    }

    public void OnItemAdded(int slotIndex, Item item)
    {
        if (slotDictionary.TryGetValue(slotIndex, out InventorySlot slot))
        {
            slot.AddItem(item);
            slot.UpdateSlot(item);
        }
    }

    public void OnItemRemoved(int slotIndex)
    {
        if(slotDictionary.TryGetValue(slotIndex, out InventorySlot slot))
        {
            slot.ClearSlot();
        }
    }

    private void ClearSlots()
    {
        foreach(var slot in slotDictionary.Values)
        {
            slot.ClearSlot();
        }
    }

    public List<InventorySlot> GetSlots()
    {
        return slots;  // 슬롯 리스트 반환
    }
}
