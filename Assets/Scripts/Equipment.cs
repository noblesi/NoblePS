using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public EquipmentType EquipmentType {  get; set; }
    public int StrengthBonus { get; set; }
    public int DexterityBonus { get; set; }
    public int IntelligenceBonus { get; set; }

    public Equipment(int id, string name, string iconPath, string prefabPath, string description, int quantity,
                     EquipmentType equipmentType, int strengthBonus, int dexterityBonus, int intelligenceBonus)
        : base(id, name, iconPath, prefabPath, ItemType.Equipment, description, quantity)
    {
        EquipmentType = equipmentType;   // 세부 분류 설정
        StrengthBonus = strengthBonus;
        DexterityBonus = dexterityBonus;
        IntelligenceBonus = intelligenceBonus;
    }

    public string GetStatBonusText()
    {
        string statBonus = "";
        if (StrengthBonus > 0) statBonus += $"Strength: +{StrengthBonus}\n";
        if (DexterityBonus > 0) statBonus += $"Dexterity: +{DexterityBonus}\n";
        if (IntelligenceBonus > 0) statBonus += $"Intelligence: +{IntelligenceBonus}\n";

        return statBonus;
    }
}
