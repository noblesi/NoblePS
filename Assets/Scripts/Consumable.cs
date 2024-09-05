using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public int HealthRestore { get; set; }  // ȸ���Ǵ� HP
    public int ManaRestore { get; set; }    // ȸ���Ǵ� MP

    public Consumable(int id, string name, Sprite icon, GameObject itemPrefab, string description, int quantity, int healthRestore, int manaRestore)
        : base(id, name, icon, itemPrefab, ItemType.Consumable, description, quantity)
    {
        HealthRestore = healthRestore;
        ManaRestore = manaRestore;
    }
}
