using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int attackDamage = 20;
    public float moveSpeed = 3.5f;
    public float attackRange = 2f;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            // ��� ó�� ����
            Debug.Log("Enemy is Dead");
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
