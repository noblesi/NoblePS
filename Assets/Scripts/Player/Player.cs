using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerData playerData;
    public string PlayerName { get; private set; }

    public event Action<int> OnDamageTaken;
    public event Action<int> OnExperienceGained;
    public event Action OnPlayerDied;

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        PlayerName = playerData.PlayerName;
        playerData.Position.ApplyTo(transform);
        UpdatePlayerStats();
    }

    public void SavePlayerData()
    {
        playerData.Position.UpdateFrom(transform);
        playerData.SavePlayerData();
    }

    public void UpdatePlayerStats()
    {
        StatusModel status = playerData.Status;

        status.MaxHP = PlayerStatsFormula.CalculateMaxHP(status.MaxHP, status.Level, status.Strength, status.Dexterity);
        status.MaxMP = PlayerStatsFormula.CalculateMaxMP(status.MaxMP, status.Level, status.Intelligence);
        status.AttackPower = (int)PlayerStatsFormula.CalculateAttackPower(status.Strength, status.Dexterity);
      
        if(status.CurrentHP > status.MaxHP) status.CurrentHP = status.MaxHP;
        if(status.CurrentMP > status.MaxMP) status.CurrentMP = status.MaxMP;
    }

    public void GainExperience(int exp)
    {
        playerData.Status.GainExp(exp);
        UpdatePlayerStats();
        OnExperienceGained?.Invoke(exp);
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = (int)DamageCalculator.CalculateDamage(damage, playerData.Status.Defence);
        playerData.Status.CurrentHP -= actualDamage;

        OnDamageTaken?.Invoke(actualDamage);

        if(playerData.Status.CurrentHP <= 0)
        {
            playerData.Status.CurrentHP = 0;
            Die();
        }

        SavePlayerData();
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        OnPlayerDied?.Invoke();
    }

    public int CalculateDamageToEnemy(Enemy enemy)
    {
        int finalDamage = (int)DamageCalculator.CalculateDamage(playerData.Status.AttackPower, enemy.MonsterData.Defence);
        return finalDamage;
    }

    public void PickupItem(Item item, InventoryPresenter inventoryPresenter)
    {
        int nextSlot = inventoryPresenter.GetNextEmptySlot();
        if (nextSlot != -1)
        {
            inventoryPresenter.AddItem(item, nextSlot);
            Debug.Log($"아이템 {item.ItemName}을 획득했습니다.");
        }
        else
        {
            Debug.Log("인벤토리에 빈 슬롯이 없습니다.");
        }
    }
}
