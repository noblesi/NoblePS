using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataManager
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");

    public static bool SaveDataExists()
    {
        return File.Exists(saveFilePath);
    }

    public static void ClearSaveData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
    }

    public static void SavePlayerData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, json);
    }

    public static PlayerData LoadPlayerData()
    {
        if(SaveDataExists())
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            return playerData;
        }
        else
        {
            return null;
        }
    }
}
