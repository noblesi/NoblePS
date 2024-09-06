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
        EquipmentType = equipmentType;   // ���� �з� ����
        StrengthBonus = strengthBonus;
        DexterityBonus = dexterityBonus;
        IntelligenceBonus = intelligenceBonus;
    }
}
