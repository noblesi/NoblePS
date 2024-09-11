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

    public void EquipItem(Item item)
    {
        if (item == null || !(item is Equipment equipment)) return;

        equipmentModel.Equip(equipment);

        inventoryPresenter.RemoveItem(equipment);
        statusPresenter.ApplyItemBonus(equipment, true);
        equipmentView.UpdateSlot(equipment.EquipmentType);  // Ư�� ��� ���Ը� ������Ʈ
    }

    public void UnequipItem(EquipmentType equipmentType)
    {
        Equipment unequippedItem = equipmentModel.Unequip(equipmentType);
        if (unequippedItem != null)
        {
            inventoryPresenter.AddItem(unequippedItem);
            statusPresenter.ApplyItemBonus(unequippedItem, false);
        }

        equipmentView.UpdateSlot(equipmentType);  // Ư�� ��� ���Ը� ������Ʈ
    }

    public bool IsEquipable(Item item)
    {
        return item is Equipment;  // Item�� Equipment�� ���� true ��ȯ
    }

    public Item GetItemInSlot(EquipmentType equipmentType)
    {
        return equipmentModel.GetEquipmentInSlot(equipmentType);
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
