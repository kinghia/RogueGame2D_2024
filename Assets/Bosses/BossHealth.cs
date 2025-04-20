using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] Animator anim;
    public int health = 500;
    //public GameObject deathEffect;
    public bool isInvulnerable;

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        health -= amount;

        int randomAnim = Random.Range(1, 3);
        anim.SetTrigger("Hurt" + randomAnim);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //todo
    }
}
