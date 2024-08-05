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
        int finalDamage = Mathf.Max(0, damage - defense); // 방어력을 고려한 데미지 계산
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

    protected abstract void Die(); // 죽음 처리 로직을 자식 클래스에서 구현하도록
}
