using System.Collections.Generic;

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
        inventoryModel.AddItemToSlot(slotIndex, item);
        inventoryView.OnItemAdded(slotIndex, item);
        //playerData.SavePlayerData(); // ������ ����
    }

    public void RemoveItem(int slotIndex)
    {
        inventoryModel.RemoveItemFromSlot(slotIndex);
        inventoryView.OnItemRemoved(slotIndex);
    }

    public void SwapItems(int slotIndex1, int slotIndex2)
    {
        Item item1 = inventoryModel.GetItemInSlot(slotIndex1);
        Item item2 = inventoryModel.GetItemInSlot(slotIndex2);

        inventoryModel.AddItemToSlot(slotIndex1, item2);
        inventoryModel.AddItemToSlot(slotIndex2, item1);

        inventoryView.OnItemAdded(slotIndex1, item2);
        inventoryView.OnItemAdded(slotIndex2, item1);
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
