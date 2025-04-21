using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] Animator anim;
    public int health = 500;
    //public GameObject deathEffect;
    public bool isInvulnerable;

    Boss boss;

    public void TakeDamage(int amount, Transform attacker)
    {
        if (isInvulnerable) return;

        health -= amount;

        int randomAnim = Random.Range(1, 3);
        anim.SetTrigger("Hurt" + randomAnim);

        // Tính knockback chỉ theo trục X
        Vector3 knockbackDir = (transform.position - attacker.position);
        knockbackDir.y = 0;
        knockbackDir.z = 0;
        knockbackDir = knockbackDir.normalized;

        StartCoroutine(DamageDash(knockbackDir, 0.2f, 0.1f));

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageDash(Vector3 direction, float dashDistance, float dashTime)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * dashDistance;

        while (elapsed < dashTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / dashTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    void Die()
    {
        //todo
    }
}
