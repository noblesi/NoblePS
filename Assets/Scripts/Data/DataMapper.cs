using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string PlayerName { get; set; }
    public int PlayerLevel { get; set; }
    public int PlayerHP { get; set; }
    public int PlayerMP { get; set; }
    public int Strength {  get; set; }
    public int Dexterity {  get; set; }
    public int Intelligence {  get; set; }
    public int PlayerEXP { get; set; }
    public int StatPoints { get; set; }
}

public class EnemyData
{
    public string EnemyName { get; set; }
    public int EnemyLevel { get; set; }
    public int EnemyHP {  get; set; }
    public int EnemyMP { get; set; }
    public int EnemyATK {  get; set; }
    public int EnemyDEF {  get; set; }
    public int EnemyEXP { get; set; }
    public List<int> EnemyDropList = new List<int>();
}

public class Item
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public Sprite Icon { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
}

public enum ItemType
{
    Weapon, Armor, Consumable, Misc
}
