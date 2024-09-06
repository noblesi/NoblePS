using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc : Item
{
    public Misc(int id, string name, string iconPath, string prefabPath, string description, int quantity)
        : base(id, name, iconPath, prefabPath, ItemType.Misc, description, quantity) { }
}
