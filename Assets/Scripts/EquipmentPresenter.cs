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

        Equipment previousItem = equipmentModel.Equip(equipment);
        if(previousItem != null)
        {
            inventoryPresenter.AddItem(previousItem);
            statusPresenter.ApplyItemBonus(previousItem, false);
        }

        inventoryPresenter.RemoveItem(equipment);
        statusPresenter.ApplyItemBonus(equipment, true);
        equipmentView.DisplayEquipment(equipmentModel);
    }

    public void UnequipItem(EquipmentType equipmentType)
    {
        Equipment unequippedItem = equipmentModel.Unequip(equipmentType);
        if(unequippedItem != null)
        {
            inventoryPresenter.AddItem(unequippedItem);
            statusPresenter.ApplyItemBonus(unequippedItem, false);
        }

        equipmentView.DisplayEquipment(equipmentModel);
    }

    public bool IsEquipable(Item item)
    {
        return item is Equipment;
    }

    public Item GetItemInSlot(EquipmentType equipmentType)
    {
        return equipmentModel.GetItemByType(equipmentType);
    }

    public void ShowItemDescription(string description)
    {
        equipmentView.ShowItemDescription(description);
    }

    public void HideItemDescription()
    {
        equipmentView.HideItemDescription();
    }
}
