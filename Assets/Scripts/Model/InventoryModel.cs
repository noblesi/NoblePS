using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryModel
{
    private Dictionary<int, Item> items = new Dictionary<int, Item>();
    private const string SaveFileName = "inventoryData.json";

    public Dictionary<int, Item> GetAllItems()
    {
        return new Dictionary<int, Item>(items);
    }

    public Item GetItemInSlot(int slotIndex)
    {
        items.TryGetValue(slotIndex, out Item item);
        return item;
    }

    public void AddItemToSlot(int slotIndex, Item item)
    {
        if (!items.ContainsKey(slotIndex))
        {
            items[slotIndex] = item;
            SaveInventoryData();
        }
    }

    public void RemoveItemFromSlot(int slotIndex)
    {
        if(items.ContainsKey(slotIndex))
        {
            items.Remove(slotIndex);
            SaveInventoryData();
        }
    }

    public void SaveInventoryData()
    {
        string json = JsonUtility.ToJson(new InventoryData(items));
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), json);    
    }

    public void LoadInventoryData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            items = data.ToDictionary();
        }
    }

}

[System.Serializable]
public class InventoryData
{
    public List<InventoryItemData> items;

    public InventoryData(Dictionary<int, Item> inventoryItems)
    {
        items = new List<InventoryItemData>();
        foreach(var item in inventoryItems.Values)
        {
            items.Add(new InventoryItemData(item));
        }
    }

    public Dictionary<int, Item> ToDictionary()
    {
        Dictionary<int, Item> inventoryItems = new Dictionary<int, Item>();
        foreach(var itemData in items)
        {
            Item item = itemData.ToItem();
            if(item != null)
            {
                inventoryItems[inventoryItems.Count] = item;
            }
        }
        return inventoryItems;
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

    public string IconPath;
    public string Description;

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
        IconPath = item.IconPath;
        Description = item.Description;
        
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
                return new Equipment(ItemID, ItemName, IconPath, null, Description, Quantity, EquipmentType, StrengthBonus, DexterityBonus, IntelligenceBonus);
            case ItemType.Consumable:
                return new Consumable(ItemID, ItemName, IconPath, null, Description, Quantity, HealthRestore, ManaRestore);
            case ItemType.Misc:
                return new Misc(ItemID, ItemName, IconPath, null, Description, Quantity);
            default:
                Debug.LogError($"Invalid ItemType : {Type}");
                return new Misc(ItemID, "Unknown Item", null, null, Description, Quantity);
        }
    }
}
