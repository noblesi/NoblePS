using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int MonsterID { get; set; }
    public string MonsterName { get; set; }
    public int Level {  get; set; } 
    public int HP {  get; set; }
    public int MaxHP { get; set; }
    public int AttackPower { get; set; }
    public int Defence {  get; set; }
    public List<DropItemData> DropList { get; set; }

    public Monster(int monsterID, string monsterName, int level, int maxHP, int attackPower, int defence, List<DropItemData> dropList)
    {
        MonsterID = monsterID;
        MonsterName = monsterName;
        Level = level;
        MaxHP = maxHP;
        HP = maxHP;
        AttackPower = attackPower;
        Defence = defence;
        DropList = dropList;
    }

    public void TakeDamage(int damage)
    {
        HP = Mathf.Max(HP - damage, 0);
        if(HP <= 0)
        {
            Debug.Log($"{MonsterName} is dead");
        }
    }

    public List<Item> GetDroppedItems(ItemLoader itemLoader)
    {
        List<Item> droppedItems = new List<Item>();

        foreach(var dropItem in DropList)
        {
            int chance = Random.Range(0, 100);
            if(chance < dropItem.DropRate)
            {
                Item item = itemLoader.GetItemByID(dropItem.ItemID);
                droppedItems.Add(item);
            }
        }

        return droppedItems;
    }
}
