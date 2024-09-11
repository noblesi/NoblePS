using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment, Consumable, Misc
}

public class Item
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public string IconPath { get; set; } 
    public string PrefabPath { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }

    public virtual Item ItemCopy()
    {
        return new  Item(ItemID, ItemName, IconPath, PrefabPath, Type, Description, Quantity);
        
    }

    public Item(int id, string name, string iconPath, string prefabPath, ItemType type, string description, int quantity)
    {
        ItemID = id;
        ItemName = name;
        IconPath = iconPath;
        PrefabPath = prefabPath;
        Type = type;
        Description = description;
        Quantity = quantity;
    }

    public Sprite GetIcon()
    {
        if (!string.IsNullOrEmpty(IconPath))
        {
            Sprite icon = Resources.Load<Sprite>(IconPath);
            if (icon != null)
            {
                return icon;
            }
            else
            {
                Debug.LogError($"Icon not found at path: {IconPath}");
            }
        }
        // 기본 아이콘 반환 (기본 이미지 설정이 필요함)
        return null;
    }

    public GameObject GetPrefab()
    {
        if (!string.IsNullOrEmpty(PrefabPath))
        {
            return Resources.Load<GameObject>(PrefabPath);
        }

        Debug.LogError("PrefabPath is null or empty");
        return null;
    }
}
