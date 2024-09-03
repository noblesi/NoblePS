using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquipmentModel
{
    public Item Weapon { get; set; }
    public Item Armor { get; set; }
    public Item Helmet { get; set; }
    public Item Boots { get; set; }

    private const string SaveFileName = "equipmentData.json";

    public EquipmentModel()
    {
        LoadEquipmentData();
    }

    public Item Equip(Item item)
    {
        Item previousItem = null;

        switch (item.Type)
        {
            case ItemType.Weapon:
                previousItem = Weapon;
                Weapon = item;
                break;
            case ItemType.Armor:
                previousItem = Armor;
                Armor = item;
                break;
            case ItemType.Helmet:
                previousItem = Helmet;
                Helmet = item;
                break;
            case ItemType.Boots:
                previousItem = Boots;
                Boots = item;
                break;
        }

        SaveEquipmentData();
        return previousItem;  // 기존 장착된 아이템 반환
    }

    public Item Unequip(ItemType itemType)
    {
        Item unequippedItem = null;

        switch (itemType)
        {
            case ItemType.Weapon:
                unequippedItem = Weapon;
                Weapon = null;
                break;
            case ItemType.Armor:
                unequippedItem = Armor;
                Armor = null;
                break;
            case ItemType.Helmet:
                unequippedItem = Helmet;
                Helmet = null;
                break;
            case ItemType.Boots:
                unequippedItem = Boots;
                Boots = null;
                break;
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

    }
}

[System.Serializable]
public class EquipmentData
{
    public ItemData Weapon;
    public ItemData Armor;
    public ItemData Helmet;
    public ItemData Boots;

    public EquipmentData(Item weapon, Item armor, Item helmet, Item boots)
    {
        Weapon = weapon != null ? new ItemData(weapon) : null;
        Armor = armor != null ? new ItemData(armor) : null;
        Helmet = helmet != null ? new ItemData(helmet) : null;
        Boots = boots != null ? new ItemData(boots) : null;
    }
}
