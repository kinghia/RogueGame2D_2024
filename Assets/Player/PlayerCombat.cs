using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dame Range")]
    [SerializeField] Transform attackPoint;
    public Transform AttackPoint {get { return attackPoint;}}
    [SerializeField] float attackRange = .5f;
    [SerializeField] int attackDamage = 40;

    [Header("Combo Attacking")]
    [SerializeField] int combo = 1;
    [SerializeField] int comboNumber = 3;
    [SerializeField] bool attacking; public bool Attacking { get { return attacking;}}   
    [SerializeField] float comboTiming = .5f;
    private float lastAttackTime = 0f; 
    private float comboResetTime = 1f; 
    
    [SerializeField] [Range(2f, 6f)] float  attackRate = 2f;
    float nextAttackTime = 0f;

    public LayerMask enemyLayers;

    Animator anim;
    PlayerMovement playerMovement;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
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

            //playerMovement.isRun = false;   

            attacking = true;

            anim.SetTrigger("Attacking" + combo);
            AttackNormal();

            lastAttackTime = Time.time;
            nextAttackTime = Time.time + 1f / attackRate;

            combo++;

            if (combo > 3)
            {
                StartCoroutine(ComboDash(0.3f, .1f));
            }   

            if (combo > comboNumber)
            {
                combo = 1;
            }
        }

        if (attacking && Time.time - lastAttackTime > .5f)
        {
            attacking = false;
            //playerMovement.isRun = true;
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

    IEnumerator ComboDash(float dashDistance, float dashTime)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (playerMovement.isFacingRight ? Vector3.right : Vector3.left) * dashDistance;

        while (elapsed < dashTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / dashTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
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
