using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatusModel
{
    public int Level {  get; set; }
    public int CurrentHP {  get; set; }
    public int CurrentMP {  get; set; }
    public int MaxHP { get; set; }
    public int MaxMP { get; set; }
    public int AttackPower { get; set; }
    public int Defence { get; set; }
    public int BaseStrength { get; private set; }
    public int BaseDexterity { get; private set; }
    public int BaseIntelligence {  get; private set; }

    private int currentExp;
    private int expToNextLevel;
    private int statPoints;

    private int strengthBonus;
    private int dexterityBonus;
    private int intelligenceBonus;

    public int Strength => BaseStrength + strengthBonus;
    public int Dexterity => BaseDexterity + dexterityBonus;
    public int Intelligence => BaseIntelligence + intelligenceBonus;

    public int GetStatPoints() => statPoints;
    public int GetCurrentExp() => currentExp;
    public int GetExpToNextLevel() => expToNextLevel;

    public int GetBaseStrength() => BaseStrength;
    public int GetBaseDexterity() => BaseDexterity;
    public int GetBaseIntelligence() => BaseIntelligence;

    public int GetStrengthBonus() => strengthBonus;
    public int GetDexterityBonus() => dexterityBonus;
    public int GetIntelligenceBonus() => intelligenceBonus;

    private Dictionary<EquipmentType, Equipment> equippedItems = new Dictionary<EquipmentType, Equipment>();

    private const string SaveFileName = "statusData.json";

    public StatusModel(int level, int maxHP, int maxMP, int attackPower, int defence, int strength, int dexterity, int intelligence)
    {
        Level = level;
        MaxHP = maxHP;
        MaxMP = maxMP;
        CurrentHP = maxHP;
        CurrentMP = maxMP;
        AttackPower = attackPower;
        Defence = defence;
        BaseStrength = strength;
        BaseDexterity = dexterity;
        BaseIntelligence = intelligence;

        currentExp = 0;
        expToNextLevel = CalculateExpToNextLevel();
        statPoints = 0;
    }

    public StatusModel()
    {
        LoadStatusData();
    }

    private int CalculateExpToNextLevel()
    {
        return Level * 100;
    }

    public void GainExp(int exp)
    {
        currentExp += exp;
        if(currentExp >= expToNextLevel)
        {
            LevelUp();
        }
        SaveStatusData();
    }

    private void LevelUp()
    {
        Level++;
        currentExp = 0;
        expToNextLevel = CalculateExpToNextLevel();

        MaxHP += 10;
        MaxMP += 5;
        BaseStrength += 2;
        BaseDexterity += 2;
        BaseIntelligence += 2;
        statPoints += 5; // 레벨업 시 스탯 포인트 추가

        CurrentHP = MaxHP;
        CurrentMP = MaxMP;

        SaveStatusData();
    }

    public void GainStatPoints(int points)
    {
        statPoints += points;
    }

    public void AllocateStatPoint(string statType)
    {
        if (statPoints > 0)
        {
            switch (statType)
            {
                case "Strength":
                    BaseStrength++;
                    break;
                case "Dexterity":
                    BaseDexterity++;
                    break;
                case "Intelligence":
                    BaseIntelligence++;
                    break;
            }
            statPoints--;
            SaveStatusData();
        }
    }

    public void ApplyItemBonus(Equipment item, bool equip)
    {
        int multiplier = equip ? 1 : -1;

        if(equip && equippedItems.ContainsKey(item.EquipmentType))
        {
            Debug.LogWarning("this item is already equipped.");
            return;
        }

        if(!equip && !equippedItems.ContainsKey(item.EquipmentType))
        {
            Debug.LogWarning("This item is not currently equipped.");
            return;
        }

        strengthBonus += item.StrengthBonus * multiplier;
        dexterityBonus += item.DexterityBonus * multiplier;
        intelligenceBonus += item.IntelligenceBonus * multiplier;

        if (equip)
        {
            equippedItems[item.EquipmentType] = item;
        }
        else
        {
            equippedItems.Remove(item.EquipmentType);
        }

        SaveStatusData();
    }

    public void SaveStatusData()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), json);
    }

    public void LoadStatusData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            // 파일이 없을 경우 기본 초기화
            Level = 1;
            MaxHP = 100;
            MaxMP = 50;
            CurrentHP = MaxHP;
            CurrentMP = MaxMP;
            BaseStrength = 10;
            BaseDexterity = 10;
            BaseIntelligence = 10;
            currentExp = 0;
            expToNextLevel = CalculateExpToNextLevel();
            statPoints = 0;
        }
    }


}
