using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private PlayerData playerData;

    public void LoadAllData()
    {
        playerData = new PlayerData();
        playerData.LoadPlayerData();
        Debug.Log("모든 게임 데이터가 로드되었습니다.");
    }

    public void SaveAllData()
    {
        if (playerData != null)
        {
            playerData.SavePlayerData();
            Debug.Log("모든 게임 데이터가 저장되었습니다.");
        }
    }

    public void ResetAllData()
    {
        DeleteSaveData();

        playerData = new PlayerData();

        playerData.SavePlayerData();

        Debug.Log("모든 게임 데이터가 초기화되었습니다.");
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public bool HasSaveData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        return File.Exists(filePath);
    }

    private void DeleteSaveData()
    {
        string playerDataPath = Path.Combine(Application.persistentDataPath, "playerData.json");
        if (File.Exists(playerDataPath))
        {
            File.Delete(playerDataPath);
        }

        string inventoryDataPath = Path.Combine(Application.persistentDataPath, "inventoryData.json");
        if (File.Exists(inventoryDataPath))
        {
            File.Delete(inventoryDataPath);
        }

        string equipmentDataPath = Path.Combine(Application.persistentDataPath, "equipmentData.json");
        if (File.Exists(equipmentDataPath))
        {
            File.Delete(equipmentDataPath);
        }

        string statusDataPath = Path.Combine(Application.persistentDataPath, "statusData.json");
        if (File.Exists(statusDataPath))
        {
            File.Delete(statusDataPath);
        }

        Debug.Log("모든 저장된 데이터가 삭제되었습니다.");
    }
}
