using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string PlayerName { get; set; }
    public int PlayerLevel { get; set; }
    public int PlayerHP { get; set; }
    public int PlayerMP { get; set; }
    public int Strength {  get; set; }
    public int Dexterity {  get; set; }
    public int Intelligence {  get; set; }
    public int PlayerEXP { get; set; }
    public int StatPoints { get; set; }

    public List<int> PlayerInventory = new List<int>();

    private const int BaseHP = 100;
    private const int BaseMP = 50;
    private const int HPPerStrength = 10;
    private const int HPPerDexterity = 5;
    private const int MPPerIntelligence = 10;
    private const int DamagePerStrength = 2;
    private const int DamagePerDexterity = 1;

    public PlayerData(string playerName)
    {
        PlayerName = playerName;
        PlayerLevel = 1;
        Strength = 5;
        Dexterity = 5;
        Intelligence = 5;
        StatPoints = 0;
        PlayerEXP = 0;
        UpdateStats();
    }

    public void LevelUp()
    {
        PlayerLevel++;
        StatPoints += 5; // 레벨업 시 5 스탯 포인트 부여
        PlayerEXP = 0;
        UpdateStats();
    }

    public void InvestStat(string stat)
    {
        if (StatPoints > 0)
        {
            switch (stat.ToLower())
            {
                case "strength":
                    Strength++;
                    break;
                case "dexterity":
                    Dexterity++;
                    break;
                case "intelligence":
                    Intelligence++;
                    break;
                default:
                    Debug.LogWarning("Invalid stat name");
                    return;
            }
            StatPoints--;
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        // HP와 MP를 스탯에 따라 계산
        PlayerHP = BaseHP + (Strength * HPPerStrength) + (Dexterity * HPPerDexterity);
        PlayerMP = BaseMP + (Intelligence * MPPerIntelligence);
    }

    public int CalculateDamage()
    {
        return (Strength * DamagePerStrength) + (Dexterity * DamagePerDexterity);
    }

    public void AddExperience(int amount)
    {
        PlayerEXP += amount;
        if (PlayerEXP >= PlayerLevel * 100)
        {
            LevelUp();
        }
    }
}

public class EnemyData
{
    public string EnemyName { get; set; }
    public int EnemyLevel { get; set; }
    public int EnemyHP {  get; set; }
    public int EnemyMP { get; set; }
    public int EnemyATK {  get; set; }
    public int EnemyDEF {  get; set; }
    public int EnemyEXP { get; set; }
    public List<int> EnemyDropList = new List<int>();

    public EnemyData(string enemyName, int enemyLevel, int enemyHP, int enemyMP, int enemyATK, int enemyDEF, int enemyEXP)
    {
        EnemyName = enemyName;
        EnemyLevel = enemyLevel;
        EnemyHP = enemyHP;
        EnemyMP = enemyMP;
        EnemyATK = enemyATK;
        EnemyDEF = enemyDEF;
        EnemyEXP = enemyEXP;
    }

    public void TakeDamage(int amount)
    {
        EnemyHP -= amount;
        if (EnemyHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 사망 처리 로직
        Debug.Log("Enemy is Dead");
    }

    public bool IsDead()
    {
        return EnemyHP <= 0;
    }
}
