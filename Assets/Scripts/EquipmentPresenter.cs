public class EquipmentPresenter
{
    private IEquipmentView equipmentView;
    private EquipmentModel equipmentModel;
    private InventoryPresenter inventoryPresenter;
    private StatusPresenter statusPresenter;

    public EquipmentPresenter(IEquipmentView view, EquipmentModel model, InventoryPresenter inventory, StatusPresenter status)
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
        Item previousItem = equipmentModel.Equip(item);
        if (previousItem != null)
        {
            inventoryPresenter.AddItem(previousItem);
            statusPresenter.ApplyItemBonus(previousItem, false);
        }

        inventoryPresenter.RemoveItem(item);
        statusPresenter.ApplyItemBonus(item, true);
        equipmentView.DisplayEquipment(equipmentModel);
    }

    public void UnequipItem(ItemType itemType)
    {
        Item unequippedItem = equipmentModel.Unequip(itemType);
        if (unequippedItem != null)
        {
            inventoryPresenter.AddItem(unequippedItem);
            statusPresenter.ApplyItemBonus(unequippedItem, false);  // 장비 해제 시 능력치 제거
        }

        equipmentView.DisplayEquipment(equipmentModel);
    }
}
