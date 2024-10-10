using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatant
{
    int HP { get; set; }
    int AttackPower {  get; }
    int Defence { get; }

    int CalculateDamage(int attackerPower, int defenderDefence);

    void TakeDamage(int damage);
}
