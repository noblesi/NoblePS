using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc : Item
{
    public Misc(int id, string name, Sprite icon, GameObject itemPrefab, string description, int quantity)
        : base(id, name, icon, itemPrefab, ItemType.Misc, description, quantity) { }
}
