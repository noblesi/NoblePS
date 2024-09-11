using UnityEngine;

public class InventoryPresenter
{
    private IInventoryView inventoryView;
    private InventoryModel inventoryModel;
    private EquipmentPresenter equipmentPresenter;
    private PlayerData playerData;

    public InventoryPresenter(IInventoryView view, InventoryModel model, EquipmentPresenter equipment, PlayerData data)
    {
        inventoryView = view;
        inventoryModel = model;
        equipmentPresenter = equipment;
        playerData = data;
    }

    public void Initialize()
    {
        inventoryView.ShowItems(inventoryModel.GetItems());
    }

    public void SetEquipmentPresenter(EquipmentPresenter equipment)
    {
        equipmentPresenter = equipment; 
    }

    public void EquipItem(Equipment equipmentItem)
    {
        if (equipmentPresenter.IsEquipable(equipmentItem))
        {
            Item copiedItem = equipmentItem.ItemCopy();  // ItemCopy�� Item ��ȯ

            // ĳ���� ���� �����ϰ� Ÿ�� üũ
            if (copiedItem is Equipment copiedEquipment)
            {
                equipmentPresenter.EquipItem(copiedEquipment);  // ��� ����
                inventoryModel.RemoveItem(equipmentItem);
                inventoryView.OnItemRemoved(equipmentItem);
                playerData.SavePlayerData(); // ������ ����
            }
            else
            {
                Debug.LogError($"EquipItem failed: Item {equipmentItem.ItemName} (ID: {equipmentItem.ItemID}) is not an Equipment.");
            }
        }
    }

    public void AddItem(Item item)
    {
        inventoryModel.AddItem(item);
        inventoryView.OnItemAdded(item);
        playerData.SavePlayerData(); // ������ ����
    }

    public void RemoveItem(Item item)
    {
        int itemSlotIndex = inventoryModel.GetItemSlot(item);
        if(itemSlotIndex != -1)
        {
            inventoryModel.RemoveItem(item);
            inventoryView.OnItemRemoved(item);
            playerData.SavePlayerData(); // ������ ����
        }
    }

    public void SwapItems(Item item1, Item item2)
    {
        int index1 = inventoryModel.GetItems().IndexOf(item1);
        int index2 = inventoryModel.GetItems().IndexOf(item2);

        if (index1 >= 0 && index2 >= 0)
        {
            inventoryModel.GetItems()[index1] = item2;
            inventoryModel.GetItems()[index2] = item1;

            inventoryView.ShowItems(inventoryModel.GetItems());
            playerData.SavePlayerData(); // ������ ����
        }
    }

    public bool CanEquipItem(Item item)
    {
        return equipmentPresenter.IsEquipable(item);
    }
}
