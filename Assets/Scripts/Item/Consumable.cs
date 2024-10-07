using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public int HealthRestore { get; set; }  // ȸ���Ǵ� HP
    public int ManaRestore { get; set; }    // ȸ���Ǵ� MP

    public Consumable(int id, string name, string iconPath, string prefabPath, string description, int quantity, int healthRestore, int manaRestore)
        : base(id, name, iconPath, prefabPath, ItemType.Consumable, description, quantity)
    {
        HealthRestore = healthRestore;
        ManaRestore = manaRestore;
    }
}
