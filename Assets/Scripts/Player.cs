using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Level { get; private set; } = 1;
    public int Experience { get; private set; } = 0;
    public int Health { get; private set; } = 100;
    public int Stamina { get; private set; } = 100;

    private const int MaxHealth = 100;
    private const int MaxStamina = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
            // 플레이어 사망 처리
        }
    }

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void UseStamina(int amount)
    {
        Stamina -= amount;
        if (Stamina < 0)
        {
            Stamina = 0;
        }
    }

    public void GainExperience(int amount)
    {
        Experience += amount;
        if (Experience >= Level * 100) // 레벨업 조건 예시
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience = 0;
        Health = MaxHealth;
        Stamina = MaxStamina;
        // 레벨업에 따른 추가 처리가 필요할 수 있음
    }
}
