using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dame Range")]
    [SerializeField] Transform attackPoint; public Transform AttackPoint {get { return attackPoint;}}
    [SerializeField] float attackRange = .5f;
    [SerializeField] int damageNormal = 40;
    [SerializeField] int damageSkill = 60;

    [Header("Combo Attacking")]
    [SerializeField] int combo = 1;
    [SerializeField] int comboNumber = 3;
    [SerializeField] bool attacking; public bool Attacking { get { return attacking;}}   

    private float lastAttackTime = 0f; 
    private float comboResetTime = 1f; 
    
    [SerializeField] [Range(2f, 6f)] float  attackRate = 2f;
    float nextAttackTime = 0f;

    public LayerMask enemyAndBossLayers;

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

            if (combo > comboNumber)
            {
                combo = 1;
            }

            // Tính knockback chỉ theo trục X
            Vector3 knockbackDir = transform.position;
            knockbackDir.y = 0;
            knockbackDir.z = 0;
            knockbackDir = knockbackDir.normalized;
            StartCoroutine(ComboDash(knockbackDir, 0.2f, .1f));
        }

        if (Input.GetKeyDown(KeyCode.J) && Time.time >= nextAttackTime)
        {
            attacking = true;

            SkillOne();
            lastAttackTime = Time.time;
            nextAttackTime = Time.time + 1f / attackRate;
        }

        if (attacking && Time.time - lastAttackTime > .5f)
        {
            attacking = false;
            combo = 1;
        }
    }

    void AttackNormal()
    {
        DamageAttackNormal();
    }

    void SkillOne()
    {
        anim.SetTrigger("Skill1");

        StartCoroutine(DoMultiHitSkill(3, 0.2f));
    }

    void DamageAttackNormal()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyAndBossLayers);

        foreach (Collider2D target in hitTargets)
        {
            if (target.CompareTag("Boss"))
            {
                target.GetComponent<BossHealth>().TakeDamage(damageNormal, transform);
            }
            else if (target.CompareTag("Enemy"))
            {
                target.GetComponent<EnemyHealth>().TakeDamage(damageNormal);
            }
        }
    }

    void DamageAttackSkill()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyAndBossLayers);

        foreach (Collider2D target in hitTargets)
        {
            if (target.CompareTag("Boss"))
            {
                target.GetComponent<BossHealth>().TakeDamage(damageSkill, transform);
            }
            else if (target.CompareTag("Enemy"))
            {
                target.GetComponent<EnemyHealth>().TakeDamage(damageSkill);
            }
        }
    }

    IEnumerator DoMultiHitSkill(int hitCount, float delayBetweenHits)
    {
        for (int i = 0; i < hitCount; i++)
        {
            DamageAttackSkill();

            yield return new WaitForSeconds(delayBetweenHits);
        }
    }

    IEnumerator ComboDash(Vector3 direction, float dashDistance, float dashTime)
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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);        
    }
}
