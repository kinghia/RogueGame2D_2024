using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] GameObject particleVFXPrefab;
    [SerializeField] GameObject particleVFXPrefab2;
    [SerializeField] GameObject slashVFXPrefab;
    [SerializeField] GameObject lungeVFXPrefab;
    [SerializeField] GameObject wideVFXPrefab;
    [SerializeField] Transform vfxSpawnPoint;

    [Header("Dame Range")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = .5f;
    [SerializeField] int attackDamage = 40;
    
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    
    float nextAttackTime = 0f;

    SpriteRenderer sr;
    Animator anim;
    PlayerMovement playerMovement;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AttackPixil();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                AttackLunge();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AttackWide();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    //void OnMouseDown()
    //{
    //    AttackPixil();  
    //}

    void AttackPixil()
    {
        anim.SetTrigger("Player_Pixil");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            StartCoroutine(DelayedHit(enemy, .5f));
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

    void AttackLunge()
    {
        anim.SetTrigger("Player_Lunge");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // todo
        }
    }

    void AttackWide()
    {
        anim.SetTrigger("Player_Wide");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // todo
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);        
    }

    public void SpawnPixilVFX()
    {
        SpawnAndFlipVFX(particleVFXPrefab, 0.8f);
        SpawnAndFlipVFX(particleVFXPrefab2, 0.8f);
        SpawnAndFlipVFX(slashVFXPrefab, 0.5f);
    }

    public void SpawnLungeVFX()
    {
        SpawnAndFlipVFX(lungeVFXPrefab, .3f);
    }

    public void SpawnWideVFX()
    {
        SpawnAndFlipVFX(wideVFXPrefab, .3f);
    }

    private void SpawnAndFlipVFX(GameObject prefab, float lifetime)
    {
        if (prefab == null || vfxSpawnPoint == null) return;

        GameObject vfx = Instantiate(prefab, vfxSpawnPoint.position, Quaternion.identity, transform);

        // Flip theo spriteRenderer.flipX
        Vector3 scale = vfx.transform.localScale;
        scale.x = sr.flipX ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        vfx.transform.localScale = scale;

        Destroy(vfx, lifetime);
    }
}
