using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private PlayerData playerData;

    private void Awake()
    {
        LoadAllData();
    }

    public void LoadAllData()
    {
        playerData = new PlayerData();
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
        playerData = new PlayerData();
        SaveAllData();
        Debug.Log("모든 게임 데이터가 초기화되었습니다.");
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public bool HasSaveData()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, "playerData.json");
        return System.IO.File.Exists(filePath);
    }
}
