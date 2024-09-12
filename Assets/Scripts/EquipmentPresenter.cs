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

        // 기존 장비가 있을 때만 교체 작업 수행
        if (previousItem != null && previousItem.ItemID != 0)
        {
            Debug.Log($"Replacing old item: {previousItem.ItemName}, ID: {previousItem.ItemID}");

            inventoryPresenter.AddItem(previousItem);
            statusPresenter.ApplyItemBonus(previousItem, false);
        }

        // 새 장비는 인벤토리에서 제거
        inventoryPresenter.RemoveItem(newEquipment);
        statusPresenter.ApplyItemBonus(newEquipment, true);

        // 해당 장비 슬롯 업데이트
        equipmentView.UpdateSlot(newEquipment.EquipmentType);
    }


    public void UnequipItem(EquipmentType equipmentType)
    {
        // 해당 슬롯에 장비가 있는지 확인
        Equipment unequippedItem = equipmentModel.Unequip(equipmentType);

        if (unequippedItem != null && unequippedItem.ItemID != 0)
        {
            Debug.Log($"Unequipping item: {unequippedItem.ItemName}, ID: {unequippedItem.ItemID}");

            inventoryPresenter.AddItem(unequippedItem);
            statusPresenter.ApplyItemBonus(unequippedItem, false);
        }
        else
        {
            // 잘못된 아이템이나 빈 슬롯을 처리
            Debug.LogWarning($"No valid item to unequip in {equipmentType} slot.");
            return;
        }

        equipmentView.UpdateSlot(equipmentType);  // 특정 장비 슬롯만 업데이트
    }


    public bool IsEquipable(Item item)
    {
        return item is Equipment;  // Item이 Equipment일 때만 true 반환
    }

    public Item GetItemInSlot(EquipmentType equipmentType)
    {
        return equipmentModel.GetEquipmentByType(equipmentType);
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
}
