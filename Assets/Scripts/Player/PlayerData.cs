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
        Status = new StatusModel(1, 100, 50, 10, 10, 10);
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
            // 파일이 없을 때는 기본값으로 시작
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
        Status = status ?? new StatusModel(1, 100, 50, 10, 10, 10); // 기본 값
        Inventory = inventory ?? new InventoryModel(); // 빈 인벤토리로 초기화
        Equipment = equipment ?? new EquipmentModel(); // 빈 장비 슬롯으로 초기화
    }

    public void ApplyTo(PlayerData playerData)
    {
        playerData.Status = Status ?? new StatusModel(1, 100, 50, 10, 10, 10); // 기본 값 설정
        playerData.Inventory = Inventory ?? new InventoryModel(); // 기본 인벤토리 설정
        playerData.Equipment = Equipment ?? new EquipmentModel(); // 기본 장비 설정
    }
}
