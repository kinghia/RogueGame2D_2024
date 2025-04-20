using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public Transform attackPoint;
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public float attackRange = 1f;
    public LayerMask attackMask;

    public void Attack()
    {
        Collider2D colInfo = Physics2D.OverlapCircle(attackPoint.position, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);        
    }
}
