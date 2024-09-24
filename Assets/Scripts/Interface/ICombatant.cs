using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatant
{
    int HP { get; set; }
    int AttackPower {  get; }
    int Defence { get; }

    void TakeDamage(int damage);
    void Attack(ICombatant target);
}
