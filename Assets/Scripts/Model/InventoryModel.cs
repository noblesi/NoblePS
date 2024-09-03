using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryModel
{
    private List<Item> items = new List<Item>();
    private const string SaveFileName = "inventoryData.json";

    public List<Item> GetItems()
    {
        return new List<Item>(items);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        SaveInventoryData();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        SaveInventoryData();
    }

    public void SaveInventoryData()
    {
        string json = JsonUtility.ToJson(new InventoryData(items));
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), json);    
    }

    public void LoadInventoryData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            items = data.ToItemList();
        }
    }
}

[System.Serializable]
public class InventoryData
{
    public List<ItemData> items;

    public InventoryData(List<Item> items)
    {
        this.items = new List<ItemData>();
        foreach(var item in items)
        {
            this.items.Add(new ItemData(item));
        }
    }

    public List<Item> ToItemList()
    {
        List<Item> itemList = new List<Item>();
        foreach(var itemData in items)
        {
            itemList.Add(itemData.ToItem());
        }
        return itemList;
    }
}

[System.Serializable]
public class ItemData
{
    public int ItemID;
    public string ItemName;
    public ItemType Type;
    public int Quantity;
    public int StrengthBonus;
    public int DexterityBonus;
    public int IntelligenceBonus;

    public ItemData(Item item)
    {
        ItemID = item.ItemID;
        ItemName = item.ItemName;
        Type = item.Type;
        Quantity = item.Quantity;
        StrengthBonus = item.StrengthBonus;
        DexterityBonus = item.DexterityBonus;
        IntelligenceBonus = item.IntelligenceBonus;
    }

    public Item ToItem()
    {
        return new Item(ItemID, ItemName, null, Type, "", Quantity, StrengthBonus, DexterityBonus, IntelligenceBonus);
    }
}
