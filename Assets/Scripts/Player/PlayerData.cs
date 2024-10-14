using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public StatusModel Status { get; set; }
    public InventoryModel Inventory { get; set; }
    public EquipmentModel Equipment { get; set; }

    private const string SaveFileName = "playerData.json";

    public PlayerData()
    {
        Status = new StatusModel(1, 100, 50, 20, 20, 20);
        Inventory = new InventoryModel();
        Equipment = new EquipmentModel();
        LoadPlayerData();
    }

    public void SavePlayerData()
    {
        PlayerDataSerializable data = new PlayerDataSerializable(Status, Inventory, Equipment);
        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, SaveFileName), json);
    }

    public void LoadPlayerData()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, SaveFileName);
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerDataSerializable data = JsonUtility.FromJson<PlayerDataSerializable>(json);

            if (data != null)
            {
                data.ApplyTo(this);
            }
            else
            {
                Debug.LogWarning("Player data is null. Using default values.");
            }
        }
        else
        {
            Debug.LogWarning("Player data file not found. Using default values.");
        }
    }

    public int GetAttackPower()
    {
        return Status.AttackPower;
    }

    public int GetDefence()
    {
        return Status.Defence;
    }
}

[System.Serializable]
public class PlayerDataSerializable
{
    public StatusData Status;
    public InventoryData Inventory;
    public EquipmentData Equipment;

    public PlayerDataSerializable(StatusModel status, InventoryModel inventory, EquipmentModel equipment)
    {
        Status = new StatusData(status);
        Inventory = new InventoryData(inventory.GetAllItems());
        Equipment = new EquipmentData(equipment.GetEquippedAllItems());
    }

    public void ApplyTo(PlayerData playerData)
    {
        Status.ApplyTo(playerData.Status);
        playerData.Inventory.LoadInventoryData();
        playerData.Equipment.LoadEquipmentData();
    }
}
