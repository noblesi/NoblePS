using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsFormula
{
    private const int baseHPIncreasePerLevel = 10;
    private const int baseMPIncreasePreLevel = 5;

    private const int HPBonusPerStrength = 5;
    private const int HPBonusPerDexterity = 2;

    private const int MPBonusPerIntelligence = 3;

    private const float attackBonusPerStrength = 1.5f;
    private const float attackBonusPerDexterity = 1.2f;

    public static int CalculateMaxHP(int baseMaxHP, int level, int strength, int dexterity)
    {
        return baseMaxHP
                + (level * baseHPIncreasePerLevel)
                + (strength * HPBonusPerStrength)
                + (dexterity * HPBonusPerDexterity);
    }

    public static int CalculateMaxMP(int baseMaxMP, int level, int intelligence)
    {
        return baseMaxMP
                +(level * baseMPIncreasePreLevel)
                +(intelligence * MPBonusPerIntelligence);
    }

    public static float CalculateAttackPower(int strength, int dexterity)
    {
        return (strength * attackBonusPerStrength) + (dexterity * attackBonusPerDexterity);
    }
}
