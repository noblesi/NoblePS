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
    public Dictionary<int, Item> LoadedItemData { get; private set; }

    private readonly string _dataRootPath = Application.streamingAssetsPath;

    protected void Awake()
    {
        filePath = Application.persistentDataPath + "/Data.json";
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {
        LoadedPlayerData = LoadDataTable(nameof(PlayerData), ParsePlayerData, pd => pd.PlayerName);
        LoadedEnemyData = LoadDataTable(nameof(EnemyData), ParseEnemyData, ed => ed.EnemyName);
        LoadedItemData = LoadDataTable(nameof(Item), ParseItemData, item => item.ItemID);
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

            foreach (var data in dataElements)
            {
                TValue value = parseElement(data);
                TKey key = getKey(value);
                
                if (!dataTable.ContainsKey(key))
                {
                    dataTable.Add(key, value);
                }
                else
                {
                    Debug.LogError($"Duplicate key found: {key} in {fileName}");
                }
            }
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }

        return dataTable;
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

    public Item GetItemData(int itemID)
    {
        if(LoadedItemData.Count == 0 || !LoadedItemData.ContainsKey(itemID)) return null;

        return LoadedItemData[itemID];
    }

    public string RemoveTextAfterParenthsis(string input)
    {
        int index = input.IndexOf('(');

        if (index == -1) return input;

        return input.Substring(0, index).Trim();
    }

    private PlayerData ParsePlayerData(XElement data)
    {
        return new PlayerData
        {
            PlayerName = data.Attribute(nameof(PlayerData.PlayerName))?.Value,
            PlayerLevel = int.Parse(data.Attribute(nameof(PlayerData.PlayerLevel))?.Value ?? "0"),
            PlayerHP = int.Parse(data.Attribute(nameof(PlayerData.PlayerHP))?.Value ?? "0"),
            PlayerMP = int.Parse(data.Attribute(nameof(PlayerData.PlayerMP))?.Value ?? "0"),
            Strength = int.Parse(data.Attribute(nameof(PlayerData.Strength))?.Value ?? "0"),
            Dexterity = int.Parse(data.Attribute(nameof(PlayerData.Dexterity))?.Value ?? "0"),
            Intelligence = int.Parse(data.Attribute(nameof(PlayerData.Intelligence))?.Value ?? "0"),
            PlayerEXP = int.Parse(data.Attribute(nameof(PlayerData.PlayerEXP))?.Value ?? "0"),
            StatPoints = int.Parse(data.Attribute(nameof(PlayerData.StatPoints))?.Value ?? "0")
        };
    }

    private EnemyData ParseEnemyData(XElement data)
    {
        var enemyData = new EnemyData
        {
            EnemyName = data.Attribute(nameof(EnemyData.EnemyName))?.Value,
            EnemyLevel = int.Parse(data.Attribute(nameof(EnemyData.EnemyLevel))?.Value ?? "0"),
            EnemyHP = int.Parse(data.Attribute(nameof(EnemyData.EnemyHP))?.Value ?? "0"),
            EnemyMP = int.Parse(data.Attribute(nameof(EnemyData.EnemyMP))?.Value ?? "0"),
            EnemyATK = int.Parse(data.Attribute(nameof(EnemyData.EnemyATK))?.Value ?? "0"),
            EnemyDEF = int.Parse(data.Attribute(nameof(EnemyData.EnemyDEF))?.Value ?? "0"),
            EnemyEXP = int.Parse(data.Attribute(nameof(EnemyData.EnemyEXP))?.Value ?? "0")
        };

        SetDataList(out enemyData.EnemyDropList, data, nameof(EnemyData.EnemyDropList), int.Parse);

        return enemyData;
    }

    private Item ParseItemData(XElement data)
    {
        return new Item
        {
            ItemID = int.Parse(data.Attribute(nameof(Item.ItemID))?.Value ?? "0"),
            ItemName = data.Attribute(nameof(Item.ItemName))?.Value,
            // 아이콘은 리소스 로딩 방식에 따라 추가 구현 필요
            Type = (ItemType)Enum.Parse(typeof(ItemType), data.Attribute(nameof(Item.Type))?.Value ?? "Misc"),
            Description = data.Attribute(nameof(Item.Description))?.Value
        };
    }
}
