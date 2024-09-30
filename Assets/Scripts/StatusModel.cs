using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class StatusModel
{
    [SerializeField] public int Level;
    [SerializeField] public int HP;
    [SerializeField] public int MaxHP;
    [SerializeField] public int MP;
    [SerializeField] public int MaxMP;
    [SerializeField] public int BaseStrength;
    [SerializeField] public int BaseDexterity;
    [SerializeField] public int BaseIntelligence;

    [SerializeField] private int currentExp;
    [SerializeField] private int expToNextLevel;
    [SerializeField] private int statPoints;

    [SerializeField] private int strengthBonus;
    [SerializeField] private int dexterityBonus;
    [SerializeField] private int intelligenceBonus;

    public int AttackPower => Mathf.RoundToInt((BaseStrength + strengthBonus) * 0.7f + (BaseDexterity + dexterityBonus) * 0.3f);
    public int Defence => Mathf.RoundToInt((BaseStrength + strengthBonus) * 0.3f + (BaseDexterity + dexterityBonus) * 0.7f);

    public int Strength => BaseStrength + strengthBonus;
    public int Dexterity => BaseDexterity + dexterityBonus;
    public int Intelligence => BaseIntelligence + intelligenceBonus;

    public int GetBaseStrength() => BaseStrength;
    public int GetBaseDexterity() => BaseDexterity;
    public int GetBaseIntelligence() => BaseIntelligence;

    private Dictionary<EquipmentType, Equipment> equippedItems = new Dictionary<EquipmentType, Equipment>();

    private const string SaveFileName = "statusData.json";

    public StatusModel(int level, int maxHP, int maxMP, int strength, int dexterity, int intelligence)
    {
        Level = level;
        MaxHP = maxHP;
        HP = maxMP;
        MaxMP = maxMP;
        MP = maxMP;
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

        HP += 10;
        HP = MaxHP;
        MP += 5;
        MP = MaxMP;

        BaseStrength += 2;
        BaseDexterity += 2;
        BaseIntelligence += 2;
        statPoints += 5; // 레벨업 시 스탯 포인트 추가

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

        if (equip && equippedItems.ContainsKey(item.EquipmentType))
        {
            Debug.LogWarning("this item is already equipped.");
            return;
        }

        if (!equip && !equippedItems.ContainsKey(item.EquipmentType))
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

    public int GetStatPoints() => statPoints;
    public int GetCurrentExp() => currentExp;
    public int GetExpToNextLevel() => expToNextLevel;

    public int GetStrengthBonus() => strengthBonus;
    public int GetDexterityBonus() => dexterityBonus;
    public int GetIntelligenceBonus() => intelligenceBonus;

    public void TakeDamage(int damage)
    {
        HP = Mathf.Max(HP - damage, 0);
        SaveStatusData();
    }

    public void Heal(int amount)
    {
        HP = Mathf.Min(HP + amount, MaxHP);
        SaveStatusData();
    }

    public void UseMana(int amount)
    {
        MP = Mathf.Max(MP - amount, 0);
        SaveStatusData();
    }

    public void RestoreMana(int amount)
    {
        MP = Mathf.Min(MP + amount, MaxMP);
        SaveStatusData();
    }

    public void SaveStatusData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

        StatusData data = new StatusData(this);
        string json = JsonUtility.ToJson(this);

        File.WriteAllText(filePath, json);

        Debug.Log("Status data saved: " + json); 
        Debug.Log("Status data saved to: " + filePath);
    }

    public void LoadStatusData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            StatusData data = JsonUtility.FromJson<StatusData>(json);
            data.ApplyTo(this);

            Debug.Log("Status data loaded: " + json);
            Debug.Log("Status data loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("Status data file not found. Initializing with default values.");
            Debug.LogWarning("Status data file not found at: " + filePath);
            InitializeDefaultValues();
        }
    }

    private void InitializeDefaultValues()
    {
        Level = 1;
        MaxHP = 100;
        HP = MaxHP;
        MaxMP = 50;
        MP = MaxMP;
        BaseStrength = 10;
        BaseDexterity = 10;
        BaseIntelligence = 10;
        currentExp = 0;
        expToNextLevel = CalculateExpToNextLevel();
        statPoints = 0;
    }

    public void SetCurrentExp(int exp) { currentExp = exp; }
    public void SetExpToNextLevel(int expToNext) { expToNextLevel = expToNext; }
    public void SetStatPoints(int points) { statPoints = points; }
    public void SetStrengthBonus(int bonus) { strengthBonus = bonus; }
    public void SetDexterityBonus(int bonus) { dexterityBonus = bonus; }
    public void SetIntelligenceBonus(int bonus) { intelligenceBonus = bonus; }
}

[System.Serializable]
public class StatusData
{
    public int Level;
    public int HP;
    public int MaxHP;
    public int MP;
    public int MaxMP;
    public int BaseStrength;
    public int BaseDexterity;
    public int BaseIntelligence;

    public int CurrentExp;
    public int ExpToNextLevel;
    public int StatPoints;

    public int StrengthBonus;
    public int DexterityBonus;
    public int IntelligenceBonus;

    public StatusData(StatusModel statusModel)
    {
        Level = statusModel.Level;
        HP = statusModel.HP;
        MaxHP = statusModel.MaxHP;
        MP = statusModel.MP;
        MaxMP = statusModel.MaxMP;
        BaseStrength = statusModel.BaseStrength;
        BaseDexterity = statusModel.BaseDexterity;
        BaseIntelligence = statusModel.BaseIntelligence;

        CurrentExp = statusModel.GetCurrentExp();
        ExpToNextLevel = statusModel.GetExpToNextLevel();
        StatPoints = statusModel.GetStatPoints();

        StrengthBonus = statusModel.GetStrengthBonus();
        DexterityBonus = statusModel.GetDexterityBonus();
        IntelligenceBonus = statusModel.GetIntelligenceBonus();
    }

    public void ApplyTo(StatusModel statusModel)
    {
        statusModel.Level = Level;
        statusModel.HP = HP;
        statusModel.MaxHP = MaxHP;
        statusModel.MP = MP;
        statusModel.MaxMP = MaxMP;
        statusModel.BaseStrength = BaseStrength;
        statusModel.BaseDexterity = BaseDexterity;
        statusModel.BaseIntelligence = BaseIntelligence;

        statusModel.SetCurrentExp(CurrentExp);
        statusModel.SetExpToNextLevel(ExpToNextLevel);
        statusModel.SetStatPoints(StatPoints);

        statusModel.SetStrengthBonus(StrengthBonus);
        statusModel.SetDexterityBonus(DexterityBonus);
        statusModel.SetIntelligenceBonus(IntelligenceBonus);
    }
}
