using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter
{
    private IInventoryView inventoryView;
    private InventoryModel inventoryModel;
    private List<InventorySlot> slots;

    private EquipmentPresenter equipmentPresenter;
    private PlayerData playerData;

    public InventoryPresenter(IInventoryView view, InventoryModel model, List<InventorySlot> inventorySlots)
    {
        inventoryView = view;
        inventoryModel = model;
        slots = inventorySlots;
    }

    public void SetEquipmentPresenter(EquipmentPresenter equipment)
    {
        equipmentPresenter = equipment;  // EquipmentPresenter ����
    }

    public void Initialize()
    {
        inventoryModel.LoadInventoryData();
        inventoryView.ShowItems(inventoryModel.GetAllItems());
    }

    public int GetNextEmptySlot()
    {
        // ��ü ���� ���� �������� �� ���� Ž��
        for (int i = 0; i < slots.Count; i++)
        {
            if (!inventoryModel.GetAllItems().ContainsKey(i))  // �ش� �ε����� �������� ���� ��
            {
                return i;  // �� ���� �ε����� ��ȯ
            }
        }
        return -1;  // �� ������ ������ -1 ��ȯ
    }

    public int GetItemSlot(Item item)
    {
        Dictionary<int, Item> items = inventoryModel.GetAllItems();
        foreach (var pair in items)
        {
            if (pair.Value == item)
            {
                return pair.Key;  // �������� ��ġ�� ���� �ε��� ��ȯ
            }
        }
        return -1;  // �������� ������ -1 ��ȯ
    }

    public void AddItem(Item item, int slotIndex)
    {
        if(item is Consumable consumableItem)
        {
            if(inventoryModel.GetItemInSlot(slotIndex) is Consumable existingItem)
            {
                int totalQuantity = existingItem.Quantity + consumableItem.Quantity;
                if(totalQuantity <= 99)
                {
                    existingItem.Quantity = totalQuantity;
                    inventoryView.OnItemAdded(slotIndex, existingItem);
                }
                else
                {
                    int remaining = totalQuantity - 99;
                    existingItem.Quantity = 99;
                    consumableItem.Quantity = remaining;
                    inventoryView.OnItemAdded(slotIndex, existingItem);
                    AddItemToNewSlot(consumableItem);
                }
            }
            else
            {
                inventoryModel.AddItemToSlot(slotIndex, item);
                inventoryView.OnItemAdded(slotIndex, item);
            }
        }
        else if(item is Equipment)
        {
            inventoryModel.AddItemToSlot(slotIndex, item);
            inventoryView.OnItemAdded(slotIndex, item);
        }
    }

    private void AddItemToNewSlot(Item item)
    {
        int nextSlot = GetNextEmptySlot();
        if(nextSlot != -1)
        {
            AddItem(item, nextSlot);
        }
    }

    public void RemoveItem(int slotIndex)
    {
        inventoryModel.RemoveItemFromSlot(slotIndex);
        inventoryView.OnItemRemoved(slotIndex);
    }

    public bool CanEquipItem(Item item)
    {
        return equipmentPresenter.IsEquipable(item);
    }

    public void EquipItem(Equipment equipmentItem)
    {
        equipmentPresenter.EquipItem(equipmentItem);
    }
}
