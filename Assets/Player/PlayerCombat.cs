using UnityEngine;
using System.Collections;
using UnityEditor.MPE;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dame Range")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = .5f;
    [SerializeField] int attackDamage = 40;

    [Header("Combo Attacking")]
    [SerializeField] int combo = 1;
    [SerializeField] int comboNumber = 3;
    [SerializeField] bool attacking;
    [SerializeField] float comboTiming = .5f;
    [SerializeField] float comboTempo;
    private float lastAttackTime = 0f; 
    private float comboResetTime = 1f; 
    
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    Animator anim;
    public LayerMask enemyLayers;

    void Start()
    {
        anim = GetComponent<Animator>();

        comboTempo = comboTiming;
    }

    void Update()
    {
        ComboAttack();
    }

    void ComboAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            // Reset combo nếu bấm quá trễ
            if (Time.time - lastAttackTime > comboResetTime)
            {
                combo = 1;
            }

            attacking = true;

            // Gọi animation và logic đòn đánh
            anim.SetTrigger("Attacking" + combo);
            AttackNormal();

            lastAttackTime = Time.time;
            nextAttackTime = Time.time + 1f / attackRate;

            combo++;

            // Reset combo nếu đã đến max
            if (combo > comboNumber)
            {
                combo = 1;
            }
        }

        // Tự động tắt trạng thái attacking nếu không bấm nữa sau 1 khoảng
        if (attacking && Time.time - lastAttackTime > comboResetTime)
        {
            attacking = false;
            combo = 1;
        }
    }

    void AttackNormal()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            Debug.Log("hit enemy");
            //StartCoroutine(DelayedHit(enemy, .5f));
        }
    }

    IEnumerator DelayedHit(Collider2D enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy != null)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            Debug.Log("hit enemy");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);        
    }
}
