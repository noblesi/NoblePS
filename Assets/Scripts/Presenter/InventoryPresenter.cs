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

    public void EquipItem(Item item)
    {
        if (equipmentPresenter.IsEquipable(item))
        {
            inventoryModel.RemoveItem(item);
            equipmentPresenter.EquipItem(item);
            inventoryView.UpdateView();
            playerData.SavePlayerData(); // 데이터 저장
        }
    }

    public void AddItem(Item item)
    {
        inventoryModel.AddItem(item);
        inventoryView.OnItemAdded(item);
        playerData.SavePlayerData(); // 데이터 저장
    }

    public void RemoveItem(Item item)
    {
        inventoryModel.RemoveItem(item);
        inventoryView.OnItemRemoved(item);
        playerData.SavePlayerData(); // 데이터 저장
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
            playerData.SavePlayerData(); // 데이터 저장
        }
    }

    public void ShowItemDescription(string description)
    {
        inventoryView.ShowTooltip(description);
    }

    public void HideItemDescription()
    {
        inventoryView.HideTooltip();
    }

    public bool CanEquipItem(Item item)
    {
        return equipmentPresenter.IsEquipable(item);
    }
}
