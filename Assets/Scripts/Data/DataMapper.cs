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
    public int Inteligence {  get; set; }
    public int PlayerEXP { get; set; }
    public List<int> PlayerInventory = new List<int>();
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
