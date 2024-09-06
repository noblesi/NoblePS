using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    private string itemDataFilePath;

    public Dictionary<int, Item> items = new Dictionary<int, Item>();

    private void Start()
    {
        itemDataFilePath = Path.Combine(Application.dataPath, "Json/ItemData.json");
        LoadItems();
    }

    public void LoadItems()
    {
        if(File.Exists(itemDataFilePath))
        {
            string jsonData = File.ReadAllText(itemDataFilePath);
            ItemDataArray loadedItemDataArray = JsonUtility.FromJson<ItemDataArray>(jsonData);

            foreach(var itemData in loadedItemDataArray.items)
            {
                Item newItem = null;

                switch (itemData.ItemType)
                {
                    case "Equipment":
                        EquipmentType equipmentType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), itemData.EquipmentType);
                        newItem = new Equipment(
                            itemData.ItemID,
                            itemData.ItemName,
                            itemData.IconPath,
                            itemData.PrefabPath,
                            itemData.Description,
                            itemData.Quantity,
                            equipmentType,
                            itemData.StrengthBonus,
                            itemData.DexterityBonus,
                            itemData.IntelligenceBonus
                        );
                        break;

                    case "Consumable":
                        newItem = new Consumable(
                            itemData.ItemID,
                            itemData.ItemName,
                            itemData.IconPath,
                            itemData.PrefabPath,
                            itemData.Description,
                            itemData.Quantity,
                            itemData.HealthRestore,
                            itemData.ManaRestore
                        );
                        break;

                    case "Misc":
                        newItem = new Misc(
                            itemData.ItemID,
                            itemData.ItemName,
                            itemData.IconPath,
                            itemData.PrefabPath,
                            itemData.Description,
                            itemData.Quantity
                        );
                        break;
                }

                if(newItem != null)
                {
                    items.Add(newItem.ItemID, newItem);
                }

                
            }
        }
        else
        {
            Debug.LogError("Item data file not found");
        }
    }

    public Item GetItemByID(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }

        Debug.LogError($"Item with ID {id} not found");
        return null;
    }
}

[System.Serializable]
public class ItemDataArray
{
    public LoadedItemData[] items;
}

[System.Serializable]
public class LoadedItemData
{
    public int ItemID;
    public string ItemName;
    public string ItemType;
    public string EquipmentType;
    public string Description;
    public string IconPath;
    public string PrefabPath;
    public int Quantity;

    public int StrengthBonus;
    public int DexterityBonus;
    public int IntelligenceBonus;

    public int HealthRestore;
    public int ManaRestore;
}
