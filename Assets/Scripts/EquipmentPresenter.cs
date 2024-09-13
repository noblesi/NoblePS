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
        equipmentView.DisplayEquipment(equipmentModel.GetEquippedAllItems());
    }

    public void EquipItem(Item item)
    {
        if (item == null || !(item is Equipment equipment)) return;

        Equipment previousItem = equipmentModel.Equip(equipment);

        if (previousItem != null)
        {
            int nextSlot = inventoryPresenter.GetNextEmptySlot();
            if (nextSlot != -1)
            {
                inventoryPresenter.AddItem(previousItem, nextSlot);  // 해제된 아이템을 빈 슬롯에 추가
                statusPresenter.ApplyItemBonus(previousItem, false);
            }
            else
            {
                Debug.LogWarning("No empty slots available for unequipped item.");
            }
        }

        inventoryPresenter.RemoveItem(inventoryPresenter.GetItemSlot(equipment));  // 착용한 아이템을 인벤토리에서 제거
        statusPresenter.ApplyItemBonus(equipment, true);
        equipmentView.UpdateSlot(equipment.EquipmentType, equipment);
    }


    public void UnequipItem(EquipmentType equipmentType)
    {
        Equipment unequippedItem = equipmentModel.Unequip(equipmentType);
        if (unequippedItem != null)
        {
            inventoryPresenter.AddItem(unequippedItem, inventoryPresenter.GetNextEmptySlot());
            statusPresenter.ApplyItemBonus(unequippedItem, false);
            equipmentView.UpdateSlot(equipmentType, null);
        }
    }

    public bool IsEquipable(Item item)
    {
        return item is Equipment;  // Item이 Equipment일 때만 true 반환
    }

    public void ShowItemDescription(Equipment equipment)
    {
        string statBonus = equipment.GetStatBonusText();
        string fullDescription = $"<b>{equipment.ItemName}</b>\n" +  // 아이템 이름
                                 $"{statBonus}";                     // 스탯 상승량

        equipmentView.ShowItemDescription(fullDescription);
    }

    public void HideItemDescription()
    {
        equipmentView.HideItemDescription();
    }

    public Item GetItemInSlot(EquipmentType equipmentType)
    {
        return equipmentModel.GetEquipmentInSlot(equipmentType);
    }
}
