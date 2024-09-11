using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterLoader : MonoBehaviour
{
    [SerializeField] private Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();

    private void Awake()
    {
        LoadMonsters();
    }

    public void LoadMonsters()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Json/MonsterData");
        if(jsonData != null)
        {
            MonsterDataArray monsterDataArray = JsonUtility.FromJson<MonsterDataArray>(jsonData.text);

            foreach(var monsterData in monsterDataArray.monsters)
            {
                // 드랍 리스트 데이터 출력 (디버깅용)
                Debug.Log($"Loading Monster: {monsterData.MonsterName}, Drop List Count: {monsterData.DropList.Count}");
                foreach (var dropItem in monsterData.DropList)
                {
                    Debug.Log($"ItemID: {dropItem.ItemID}, DropRate: {dropItem.DropRate}");
                }

                Monster monster = new Monster(
                    monsterData.MonsterID,
                    monsterData.MonsterName,
                    monsterData.Level,
                    monsterData.HP,
                    monsterData.AttackPower,
                    monsterData.Defence,
                    monsterData.DropList
                );

                monsters.Add(monster.MonsterID, monster);
            }
        }
        else
        {
            Debug.LogError("Monster data file not found");
        }
    }

    public Monster GetMonsterByID(int id)
    {
        if (monsters.ContainsKey(id))
        {
            return monsters[id];
        }

        Debug.LogError($"Monster with ID {id} not found");
        return null;
    }
}

[System.Serializable]
public class MonsterDataArray
{
    public MonsterData[] monsters;
}

[System.Serializable]
public class MonsterData
{
    public int MonsterID;
    public string MonsterName;
    public int Level;
    public int HP;
    public int AttackPower;
    public int Defence;
    public List<DropItemData> DropList;
}

[System.Serializable]
public class DropItemData
{
    public int ItemID;
    public int DropRate;
}
