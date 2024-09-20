using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int MonsterID { get; set; }
    public string MonsterName { get; set; }
    public int Level {  get; set; } 
    public int HP {  get; set; }
    public int AttackPower { get; set; }
    public int Defence {  get; set; }
    public List<DropItemData> DropList { get; set; }

    public float MoveSpeed { get; set; } // �̵� �ӵ�
    public float ChaseDistance { get; set; } // ���� �Ÿ�
    public float AttackDistance { get; set; } // ���� �Ÿ�
    public float ReChaseDistance { get; set; } // ������ �Ÿ�
    public float AttackDelay { get; set; } // ���� ������
    public int EXPReward { get; set; } // óġ �� �÷��̾�� ������ ����ġ

    public Monster(int monsterID, string monsterName, int level, int hP, int attackPower, int defence, List<DropItemData> dropList,
                   float moveSpeed, float chaseDistance, float attackDistance, float reChaseDistance, float attackDelay, int expReward)
    {
        MonsterID = monsterID;
        MonsterName = monsterName;
        Level = level;
        HP = hP;
        AttackPower = attackPower;
        Defence = defence;
        DropList = dropList;

        MoveSpeed = moveSpeed;
        ChaseDistance = chaseDistance;
        AttackDistance = attackDistance;
        ReChaseDistance = reChaseDistance;
        AttackDelay = attackDelay;
        EXPReward = expReward;
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
