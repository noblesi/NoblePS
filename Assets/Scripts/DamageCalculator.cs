using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    private const float criticalMultiplier = 2.0f;
    private const float defenceEffectiveness = 0.5f;

    public static float CalculateDamage(int attackPower, int defence, bool isCritical = false)
    {
        float damageReduction = defence * defenceEffectiveness;
        float baseDamage = attackPower - damageReduction;

        if (isCritical)
        {
            baseDamage *= criticalMultiplier;
        }

        return Mathf.Max(baseDamage, 0);
    }

    public static bool IsCriticalHit(float criticalChance)
    {
        return Random.value < criticalChance;
    }
}
