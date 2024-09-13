using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum EquipmentType
{
    Weapon, Armor, Helmet, Boots
}

public class EquipmentModel
{
    private const string SaveFileName = "equipmentData.json";
    private Dictionary<EquipmentType, Equipment> equippedItems = new Dictionary<EquipmentType, Equipment>();

    public Equipment GetEquipmentInSlot(EquipmentType equipmentType)
    {
        equippedItems.TryGetValue(equipmentType, out Equipment equipped);
        return equipped;
    }

    public Dictionary<EquipmentType, Equipment> GetEquippedAllItems()
    {
        return new Dictionary<EquipmentType, Equipment>(equippedItems);
    }

    public int GetItemSlot(Equipment equipment)
    {
        foreach (var pair in equippedItems)
        {
            if (pair.Value == equipment)
            {
                return (int)pair.Key;  // 슬롯 인덱스를 반환
            }
        }
        return -1;  // 없으면 -1 반환
    }

    public Equipment Equip(Equipment item)
    {
        Equipment previousItem = null;

        if(equippedItems.ContainsKey(item.EquipmentType))
        {
            previousItem = equippedItems[item.EquipmentType];
        }

        equippedItems[item.EquipmentType] = item;
        SaveEquipmentData();

        return previousItem;
    }

    public Equipment Unequip(EquipmentType equipmentType)
    {
        if(equippedItems.TryGetValue(equipmentType, out Equipment unequippedItem))
        {
            equippedItems.Remove(equipmentType);
            SaveEquipmentData();
            return unequippedItem;
        }

        return null;
    }

    public void SaveEquipmentData()
    {
        EquipmentData data = new EquipmentData(equippedItems);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), json);
    }

    public void LoadEquipmentData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            EquipmentData data = JsonUtility.FromJson<EquipmentData>(json);

            equippedItems = data.ToDictionary();  // Dictionary로 로드
        }
    }
}

[System.Serializable]
public class EquipmentData
{
    public List<InventoryItemData> items;

    public EquipmentData(Dictionary<EquipmentType, Equipment> equippedItems)
    {
        items = new List<InventoryItemData>();
        foreach(var item in equippedItems.Values)
        {
            items.Add(new InventoryItemData(item));
        }
    }

    public Dictionary<EquipmentType, Equipment> ToDictionary()
    {
        Dictionary<EquipmentType, Equipment> equippedItems = new Dictionary<EquipmentType, Equipment>();
        foreach(var itemData in items)
        {
            Equipment equipment = itemData.ToItem() as Equipment;
            if(equipment != null)
            {
                equippedItems[equipment.EquipmentType] = equipment;
            }
        }
        return equippedItems;
    }
}
