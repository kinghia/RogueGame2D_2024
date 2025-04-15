using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] int maxHealth = 100;
    int currentHealth = 0;
    public int CurrentHitPoints {get { return currentHealth; }}

    Enemy enemy;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();   
    }

    public void TakeDamage (int amount)
    {
        currentHealth -= Mathf.Abs(amount);
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0 )
        {
            anim.SetBool("IsDead", true);
            Invoke("Die", 1f);
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
