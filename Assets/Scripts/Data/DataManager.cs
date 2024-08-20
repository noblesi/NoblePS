using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : Singleton<DataManager>
{
    private string filePath;
    public Dictionary<string, PlayerData> LoadedPlayerData {  get; private set; }
    public Dictionary<string, EnemyData> LoadedEnemyData { get; private set; }

    private readonly string _dataRootPath = Application.streamingAssetsPath;

    protected void Awake()
    {
        filePath = Application.persistentDataPath + "/Data.json";
    }

    private void ReadAllDataOnAwake()
    {
        LoadedPlayerData = LoadDataTable(nameof(PlayerData), ParsePlayerData, pd => pd.PlayerName);
        LoadedEnemyData = LoadDataTable(nameof(EnemyData), ParseEnemyData, ed => ed.EnemyName);
    }

    private void CopyFileToPersistentPath(string fileName)
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        if (Application.platform == RuntimePlatform.Android)
        {
            if (!File.Exists(destinationPath))
            {
                UnityWebRequest www = UnityWebRequest.Get(sourcePath);
                www.SendWebRequest();

                while (!www.isDone) { }

                if (www.result == UnityWebRequest.Result.Success)
                {
                    File.WriteAllBytes(destinationPath, www.downloadHandler.data);
                }
                else
                {
                    Debug.LogError("Failed to copy file from StreamingAssets: " + www.error);
                }
            }
        }
        else
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
            }
            else
            {
                Debug.LogError("File not found in StreamingAssets: " + sourcePath);
            }
        }
    }

    private Dictionary<TKey, TValue> LoadDataTable<TKey, TValue>(string fileName, Func<XElement, TValue> parseElement, Func<TValue, TKey> getKey)
    {
        var dataTable = new Dictionary<TKey, TValue>();

        CopyFileToPersistentPath($"{fileName}.xml");

        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.xml");

        if (File.Exists(filePath))
        {
            XDocument doc = XDocument.Load(filePath);
            var dataElements = doc.Descendants("data");

            foreach(var data in dataElements)
            {
                TValue value = parseElement(data);
                TKey key = getKey(value);
                dataTable.Add(key, value);
            }
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }

        return dataTable;
    }

    private PlayerData ParsePlayerData(XElement data)
    {
        var tempPlayerData =  new PlayerData
        {
            PlayerName = data.Attribute(nameof(PlayerData.PlayerName)).Value,
            PlayerLevel = int.Parse(data.Attribute(nameof(PlayerData.PlayerLevel)).Value),
            PlayerHP = int.Parse(data.Attribute(nameof(PlayerData.PlayerHP)).Value),
            PlayerMP = int.Parse(data.Attribute(nameof(PlayerData.PlayerMP)).Value),
            Strength = int.Parse(data.Attribute(nameof(PlayerData.Strength)).Value),
            Dexterity = int.Parse(data.Attribute(nameof(PlayerData.Dexterity)).Value),
            Inteligence = int.Parse(data.Attribute(nameof(PlayerData.Inteligence)).Value),
            PlayerEXP = int.Parse(data.Attribute(nameof(PlayerData.PlayerEXP)).Value)
        };

        SetDataList(out tempPlayerData.PlayerInventory, data, "PlayerInventory");

        return tempPlayerData;
    }

    private EnemyData ParseEnemyData(XElement data)
    {
        var tempEnemyData = new EnemyData
        {
            EnemyName = data.Attribute(nameof(EnemyData.EnemyName)).Value,
            EnemyHP = int.Parse(data.Attribute(nameof(EnemyData.EnemyHP)).Value),
            EnemyMP = int.Parse(data.Attribute(nameof(EnemyData.EnemyMP)).Value),
            EnemyATK = int.Parse(data.Attribute(nameof(EnemyData.EnemyATK)).Value),
            EnemyDEF = int.Parse(data.Attribute(nameof(EnemyData.EnemyDEF)).Value),
            EnemyEXP = int.Parse(data.Attribute(nameof(EnemyData.EnemyHP)).Value)
        };

        SetDataList(out tempEnemyData.EnemyDropList, data, "EnemyDropList");

        return tempEnemyData;
    }

    private void SetDataList<T>(out List<T> usingList, XElement data, string listName, Func<string, T> parseElement = null)
    {
        string ListStr = data.Attribute(listName)?.Value;
        if(!string.IsNullOrEmpty(ListStr))
        {
            ListStr = ListStr.Replace("{", "").Replace("}", "");

            var elements = ListStr.Split(',');

            var list = new List<T>();

            foreach(var element in elements)
            {
                T value = parseElement != null ? parseElement(element) : (T)Convert.ChangeType(element, typeof(T));
                list.Add(value);
            }
            usingList = list;
        }
        else
        {
            usingList = null;
        }
    }

    public PlayerData GetPlayerData(string playerName)
    {
        string name = RemoveTextAfterParenthsis(playerName);

        if (LoadedPlayerData.Count == 0 || !LoadedPlayerData.ContainsKey(name)) return null;

        return LoadedPlayerData[name];
    }

    public EnemyData GetEnemyData(string enemyName)
    {
        string name = RemoveTextAfterParenthsis(enemyName);

        if (LoadedEnemyData.Count == 0 || !LoadedEnemyData.ContainsKey(name)) return null;

        return LoadedEnemyData[name];
    }

    public string RemoveTextAfterParenthsis(string input)
    {
        int index = input.IndexOf('(');

        if (index == -1) return input;

        return input.Substring(0, index).Trim();
    }
}
