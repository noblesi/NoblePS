using UnityEngine;

public class EquipmentPresenter
{
    private EquipmentView equipmentView;
    private EquipmentModel equipmentModel;
    private InventoryPresenter inventoryPresenter;
    private StatusPresenter statusPresenter;

    public EquipmentPresenter(EquipmentView view, EquipmentModel model, InventoryPresenter inventory, StatusPresenter status)
    {
        equipmentView = view;
        equipmentModel = model;
        inventoryPresenter = inventory;
        statusPresenter = status;
    }

    public void Initialize()
    {
        equipmentView.DisplayEquipment(equipmentModel);
    }

    public void EquipItem(Equipment newEquipment)
    {
        Equipment previousItem = equipmentModel.Equip(newEquipment);

        // ���� ��� ���� ���� ��ü �۾� ����
        if (previousItem != null && previousItem.ItemID != 0)
        {
            Debug.Log($"Replacing old item: {previousItem.ItemName}, ID: {previousItem.ItemID}");

            inventoryPresenter.AddItem(previousItem);
            statusPresenter.ApplyItemBonus(previousItem, false);
        }

        // �� ���� �κ��丮���� ����
        inventoryPresenter.RemoveItem(newEquipment);
        statusPresenter.ApplyItemBonus(newEquipment, true);

        // �ش� ��� ���� ������Ʈ
        equipmentView.UpdateSlot(newEquipment.EquipmentType);
    }


    public void UnequipItem(EquipmentType equipmentType)
    {
        // �ش� ���Կ� ��� �ִ��� Ȯ��
        Equipment unequippedItem = equipmentModel.Unequip(equipmentType);

        if (unequippedItem != null && unequippedItem.ItemID != 0)
        {
            Debug.Log($"Unequipping item: {unequippedItem.ItemName}, ID: {unequippedItem.ItemID}");

            inventoryPresenter.AddItem(unequippedItem);
            statusPresenter.ApplyItemBonus(unequippedItem, false);
        }
        else
        {
            // �߸��� �������̳� �� ������ ó��
            Debug.LogWarning($"No valid item to unequip in {equipmentType} slot.");
            return;
        }

        equipmentView.UpdateSlot(equipmentType);  // Ư�� ��� ���Ը� ������Ʈ
    }


    public bool IsEquipable(Item item)
    {
        return item is Equipment;  // Item�� Equipment�� ���� true ��ȯ
    }

    public Item GetItemInSlot(EquipmentType equipmentType)
    {
        return equipmentModel.GetEquipmentByType(equipmentType);
    }

    public void ShowItemDescription(Equipment equipment)
    {
        string statBonus = equipment.GetStatBonusText();
        string fullDescription = $"<b>{equipment.ItemName}</b>\n" +  // ������ �̸�
                                 $"{statBonus}";                     // ���� ��·�

        equipmentView.ShowItemDescription(fullDescription);
    }

    public void HideItemDescription()
    {
        equipmentView.HideItemDescription();
    }
}
