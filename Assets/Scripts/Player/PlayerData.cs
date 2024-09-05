using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public StatusModel Status {  get; set; }
    public InventoryModel Inventory { get; set; }
    public EquipmentModel Equipment { get; set; }

    private const string SaveFileName = "playerData.json";

    public PlayerData()
    {
        Status = new StatusModel();
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
            data.ApplyTo(this);
        }
        else
        {
            // 기본 초기화는 각 모델의 기본 생성자에서 처리
        }
    }
}

[System.Serializable]
public class PlayerDataSerializable
{
    public StatusModel Status;
    public InventoryModel Inventory;
    public EquipmentModel Equipment;

    public PlayerDataSerializable(StatusModel status, InventoryModel inventory, EquipmentModel equipment)
    {
        Status = status;
        Inventory = inventory;
        Equipment = equipment;
    }

    public void ApplyTo(PlayerData playerData)
    {
        playerData.Status = Status;
        playerData.Inventory = Inventory;
        playerData.Equipment = Equipment;
    }
}
