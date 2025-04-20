using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] int maxHealth = 100;
    int currentHealth = 0;

    //public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage (int amount)
    {
        currentHealth -= Mathf.Abs(amount);
        anim.SetTrigger("Hurt");

        //healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0 )
        {
            Die();
        }
    }
    void Die()
    {
        //todo
    }
}
