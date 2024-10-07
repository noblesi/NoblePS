using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    [SerializeField] private Dictionary<int, Item> items = new Dictionary<int, Item>();

    private void Awake()
    {
        LoadItems();
    }

    public void LoadItems()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Json/ItemData");
        if (jsonData != null)
        {
            ItemDataArray loadedItemDataArray = JsonUtility.FromJson<ItemDataArray>(jsonData.text);

            foreach (var itemData in loadedItemDataArray.items)
            {
                Item newItem = null;

                // ItemType을 enum으로 변환
                if (System.Enum.TryParse(itemData.ItemType, out ItemType itemType))
                {
                    switch (itemType)
                    {
                        case ItemType.Equipment:
                            // EquipmentType도 enum으로 변환
                            if (System.Enum.TryParse(itemData.EquipmentType, out EquipmentType equipmentType))
                            {
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
                            }
                            else
                            {
                                Debug.LogError($"Invalid EquipmentType: {itemData.EquipmentType}");
                            }
                            break;

                        case ItemType.Consumable:
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

                        case ItemType.Misc:
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

                    if (newItem != null)
                    {
                        items.Add(newItem.ItemID, newItem);
                    }
                }
                else
                {
                    Debug.LogError($"Invalid ItemType: {itemData.ItemType}");
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
