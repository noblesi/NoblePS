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
    public Equipment Weapon { get; set; }
    public Equipment Armor { get; set; }
    public Equipment Helmet { get; set; }
    public Equipment Boots { get; set; }

    private const string SaveFileName = "equipmentData.json";

    public EquipmentModel()
    {
        LoadEquipmentData();
    }

    public bool IsSlotEmpty(EquipmentType equipmentType)
    {
        Equipment equippedItem = GetEquipmentByType(equipmentType);
        return equippedItem == null;
    }

    public Equipment GetEquipmentByType(EquipmentType equipmentType)
    {
        return equipmentType switch
        {
            EquipmentType.Weapon => Weapon,
            EquipmentType.Armor => Armor,
            EquipmentType.Helmet => Helmet,
            EquipmentType.Boots => Boots,
            _ => null,
        };
    }

    public Equipment Equip(Equipment item)
    {
        Equipment previousItem = null;

        switch (item.EquipmentType)
        {
            case EquipmentType.Weapon:
                previousItem = Weapon;
                Weapon = item;
                break;
            case EquipmentType.Armor:
                previousItem = Armor;
                Armor = item;
                break;
            case EquipmentType.Helmet:
                previousItem = Helmet;
                Helmet = item;
                break;
            case EquipmentType.Boots:
                previousItem = Boots;
                Boots = item;
                break;
        }

        SaveEquipmentData();
        return previousItem;  // 기존 장착된 아이템 반환
    }

    public Equipment Unequip(EquipmentType equipmentType)
    {
        Equipment unequippedItem = null;

        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                unequippedItem = Weapon;
                Weapon = null;
                break;
            case EquipmentType.Armor:
                unequippedItem = Armor;
                Armor = null;
                break;
            case EquipmentType.Helmet:
                unequippedItem = Helmet;
                Helmet = null;
                break;
            case EquipmentType.Boots:
                unequippedItem = Boots;
                Boots = null;
                break;
        }

        // 장비가 없을 경우 null을 반환
        if (unequippedItem == null || unequippedItem.ItemID == 0)
        {
            return null;
        }

        SaveEquipmentData();
        return unequippedItem;  // 해제된 아이템 반환
    }


    public void SaveEquipmentData()
    {
        EquipmentData data = new EquipmentData(Weapon, Armor, Helmet, Boots);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), json);
    }

    public void LoadEquipmentData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            EquipmentData data = JsonUtility.FromJson<EquipmentData>(json);

            Weapon = data.Weapon != null ? (Equipment)data.Weapon.ToItem() : null;
            Armor = data.Armor != null ? (Equipment)data.Armor.ToItem() : null;
            Helmet = data.Helmet != null ? (Equipment)data.Helmet.ToItem() : null;
            Boots = data.Boots != null ? (Equipment)data.Boots.ToItem() : null;
        }
    }
}

[System.Serializable]
public class EquipmentData
{
    public InventoryItemData Weapon;
    public InventoryItemData Armor;
    public InventoryItemData Helmet;
    public InventoryItemData Boots;

    public EquipmentData(Item weapon, Item armor, Item helmet, Item boots)
    {
        Weapon = weapon != null ? new InventoryItemData(weapon) : null;
        Armor = armor != null ? new InventoryItemData(armor) : null;
        Helmet = helmet != null ? new InventoryItemData(helmet) : null;
        Boots = boots != null ? new InventoryItemData(boots) : null;
    }
}
