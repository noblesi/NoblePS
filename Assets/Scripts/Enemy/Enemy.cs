using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Monster MonsterData {  get; private set; }
    public int CurrentHP {  get; private set; }

    public void Initialize(Monster monster)
    {
        MonsterData = monster;
        CurrentHP = monster.HP;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = (int)DamageCalculator.CalculateDamage(damage, MonsterData.Defence);
        CurrentHP -= actualDamage;
        if(CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{MonsterData.MonsterName} has died.");
    }

    public List<Item> DropItems(ItemLoader itemLoader)
    {
        return MonsterData.GetDroppedItems(itemLoader);
    }

    public bool IsAlive()
    {
        return CurrentHP > 0;
    }
}
