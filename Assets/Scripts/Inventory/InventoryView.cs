using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;

    private List<GameObject> slots = new List<GameObject>();

    private InventoryPresenter presenter;

    private void Start()
    {
        InventoryModel model = new InventoryModel();
        presenter = new InventoryPresenter(this, model);
        presenter.Initialize();
    }

    public void ShowItems(List<Item> items)
    {
        foreach(var slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

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

    private void AddItemToUI(Item item)
    {
        GameObject slot = Instantiate(inventorySlotPrefab, itemsParent);
        InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
        inventorySlot.Initialize(item, presenter, FindObjectOfType<Tooltip>());
        slots.Add(slot);
    }

    private void RemoveItemFromUI(Item item)
    {
        var slotToRemove = slots.Find(slot => slot.GetComponentInChildren<Text>().text == item.ItemName);
        if(slotToRemove != null)
        {
            slots.Remove(slotToRemove);
            Destroy(slotToRemove);
        }
    }
}
