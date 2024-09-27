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
        Debug.Log("��� ���� �����Ͱ� �ε�Ǿ����ϴ�.");
    }

    public void SaveAllData()
    {
        if (playerData != null)
        {
            playerData.SavePlayerData();
            Debug.Log("��� ���� �����Ͱ� ����Ǿ����ϴ�.");
        }
    }

    public void ResetAllData()
    {
        playerData = new PlayerData();
        SaveAllData();
        Debug.Log("��� ���� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
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
