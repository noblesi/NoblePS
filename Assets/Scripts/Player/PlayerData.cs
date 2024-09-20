using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string PlayerName {  get; set; }
    public StatusModel Status { get; set; }
    public InventoryModel Inventory { get; set; }
    public EquipmentModel Equipment { get; set; }
    public PlayerPosition Position { get; set; }

    private const string SaveFileName = "playerData.json";
    private const string DefaultPlayerName = "ฟ๋ป็";

    public PlayerData()
    {
        PlayerName = DefaultPlayerName;
        Status = new StatusModel(1, 100, 50, 10, 10, 10, 10, 10);
        Inventory = new InventoryModel();
        Equipment = new EquipmentModel();
        Position = new PlayerPosition();
    }

    public void SavePlayerData()
    {
        try
        {
            PlayerDataSerializable data = new PlayerDataSerializable(this);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save player data: {ex.Message}");
        }
    }

    public void LoadPlayerData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
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
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load player data: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Player data file not found. Using default values.");
        }
    }
}

[System.Serializable]
public class PlayerDataSerializable
{
    public string PlayerName;
    public StatusModel Status;
    public InventoryModel Inventory;
    public EquipmentModel Equipment;
    public SerializableVector3 Position;
    public SerializableQuaternion Rotation;

    public PlayerDataSerializable(PlayerData playerData)
    {
        PlayerName = playerData.PlayerName;
        Status = playerData.Status;
        Inventory = playerData.Inventory;
        Equipment = playerData.Equipment;
        Position = new SerializableVector3(playerData.Position.Position.ToVector3());
        Rotation = new SerializableQuaternion(playerData.Position.Rotation.ToQuaternion());
    }

    public void ApplyTo(PlayerData playerData)
    {
        playerData.PlayerName = PlayerName;
        playerData.Status = Status;
        playerData.Inventory = Inventory;
        playerData.Equipment = Equipment;
        playerData.Position.Position = Position;
        playerData.Position.Rotation = Rotation;
    }
}

[System.Serializable]
public class PlayerPosition
{
    public SerializableVector3 Position { get; set; }
    public SerializableQuaternion Rotation { get; set; }

    public PlayerPosition()
    {
        Position = new SerializableVector3(Vector3.zero);
        Rotation = new SerializableQuaternion(Quaternion.identity);
    }

    public void ApplyTo(Transform transform)
    {
        transform.position = Position.ToVector3();
        transform.rotation = Rotation.ToQuaternion();
    }

    public void UpdateFrom(Transform transform)
    {
        Position = new SerializableVector3(transform.position);
        Rotation = new SerializableQuaternion(transform.rotation);
    }
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}


