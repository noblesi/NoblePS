using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public EquipmentType EquipmentType {  get; set; }
    public int StrengthBonus { get; set; }
    public int DexterityBonus { get; set; }
    public int IntelligenceBonus { get; set; }

    public Equipment(int id, string name, Sprite icon, GameObject itemPrefab, string description, int quantity,
                     EquipmentType equipmentType, int strengthBonus, int dexterityBonus, int intelligenceBonus)
        : base(id, name, icon, itemPrefab, ItemType.Equipment, description, quantity)
    {
        EquipmentType = equipmentType;   // 세부 분류 설정
        StrengthBonus = strengthBonus;
        DexterityBonus = dexterityBonus;
        IntelligenceBonus = intelligenceBonus;
    }
}
