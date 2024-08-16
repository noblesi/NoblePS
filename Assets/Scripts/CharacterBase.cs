using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IDamageable, IAttack
{
    public string characterName;
    public int health;
    public int attackPower;
    public int defense;

    public abstract void Move(Vector3 direction);

    public virtual void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(0, damage - defense); // ������ ����� ������ ���
        health -= finalDamage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Attack(IDamageable target)
    {
        target.TakeDamage(attackPower);
    }

    protected abstract void Die(); // ���� ó�� ������ �ڽ� Ŭ�������� �����ϵ���
}
