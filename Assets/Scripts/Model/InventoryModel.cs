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
    public List<InventoryItemData> items;

    public InventoryData(List<Item> items)
    {
        this.items = new List<InventoryItemData>();
        foreach(var item in items)
        {
            this.items.Add(new InventoryItemData(item));
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
public class InventoryItemData
{
    public int ItemID;
    public string ItemName;
    public ItemType Type;
    public EquipmentType EquipmentType;
    public int Quantity;

    public int StrengthBonus;
    public int DexterityBonus;
    public int IntelligenceBonus;

    public int HealthRestore;
    public int ManaRestore;

    public InventoryItemData(Item item)
    {
        if(item == null)
        {
            Debug.LogError("Item is null in InventoryItemData constructor.");
            return;
        }

        ItemID = item.ItemID;
        ItemName = item.ItemName;
        Type = item.Type;
        Quantity = item.Quantity;
        
        if(item is Equipment equipment)
        {
            EquipmentType = equipment.EquipmentType;
            StrengthBonus = equipment.StrengthBonus;
            DexterityBonus = equipment.DexterityBonus;
            IntelligenceBonus = equipment.IntelligenceBonus;
        }
        else if(item is Consumable consumable)
        {
            HealthRestore = consumable.HealthRestore;
            ManaRestore = consumable.ManaRestore;
        }
    }

    public Item ToItem()
    {
        switch (Type)
        {
            case ItemType.Equipment:
                return new Equipment(ItemID, ItemName, null, null, "", Quantity, EquipmentType, StrengthBonus, DexterityBonus, IntelligenceBonus);
            case ItemType.Consumable:
                return new Consumable(ItemID, ItemName, null, null, "", Quantity, HealthRestore, ManaRestore);
            case ItemType.Misc:
                return new Misc(ItemID, ItemName, null, null, "", Quantity);
            default:
                return null;
        }
    }
}
