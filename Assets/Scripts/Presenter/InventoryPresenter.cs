using System.Collections.Generic;

public class InventoryPresenter
{
    private IInventoryView inventoryView;
    private InventoryModel inventoryModel;

    public InventoryPresenter(IInventoryView view, InventoryModel model)
    {
        inventoryView = view;
        inventoryModel = model;
    }

    public void Initialize()
    {
        inventoryView.ShowItems(inventoryModel.GetItems());
    }

    public void AddItem(Item item)
    {
        inventoryModel.AddItem(item);
        inventoryView.OnItemAdded(item);
    }

    public void RemoveItem(Item item)
    {
        inventoryModel.RemoveItem(item);
        inventoryView.OnItemRemoved(item);
    }

    public void SwapItems(Item item1, Item item2)
    {
        int index1 = inventoryModel.GetItems().IndexOf(item1);
        int index2 = inventoryModel.GetItems().IndexOf(item2);

        if(index1 >= 0 && index2 >= 0)
        {
            inventoryModel.GetItems()[index1] = item2;
            inventoryModel.GetItems()[index2] = item1;

            inventoryView.ShowItems(inventoryModel.GetItems());
        }
    }
}
