using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerData playerData;
    public string PlayerName { get; private set; }

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        PlayerName = playerData.PlayerName;
        UpdatePlayerStats();
        playerData.Position.ApplyTo(transform);
    }

    public void SavePlayerData()
    {
        UpdatePlayerStats();
        playerData.Position.UpdateFrom(transform);
        playerData.SavePlayerData();
    }

    public void UpdatePlayerStats()
    {
        StatusModel status = playerData.Status;

        status.MaxHP = PlayerStatsFormula.CalculateMaxHP(status.MaxHP, status.Level, status.Strength, status.Dexterity);
        status.MaxMP = PlayerStatsFormula.CalculateMaxMP(status.MaxMP, status.Level, status.Intelligence);

        if(status.CurrentHP > status.MaxHP) status.CurrentHP = status.MaxHP;
        if(status.CurrentMP > status.MaxMP) status.CurrentMP = status.MaxMP;

        status.AttackPower = (int)PlayerStatsFormula.CalculateAttackPower(status.Strength, status.Dexterity);
    }

    public int CalculateDamageToEnemy(Enemy enemy)
    {
        int finalDamage = (int)DamageCalculator.CalculateDamage(playerData.Status.AttackPower, enemy.MonsterData.Defence);
        return finalDamage;
    }

    public void TakeDamage(int incomingDamage)
    {
        StatusModel playerStatus = playerData.Status;

        int actualDamage = (int)DamageCalculator.CalculateDamage(incomingDamage, playerStatus.Defence);
        playerStatus.CurrentHP -= actualDamage;

        if(playerStatus.CurrentHP <= 0)
        {
            playerStatus.CurrentHP = 0;
            Die();
        }

        playerData.SavePlayerData();
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }

    public void GainExperience(int exp)
    {
        playerData.Status.GainExp(exp);
        UpdatePlayerStats();
    }

    public void AllocateStatPoint(string statType)
    {
        playerData.Status.AllocateStatPoint(statType);
        UpdatePlayerStats();
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
