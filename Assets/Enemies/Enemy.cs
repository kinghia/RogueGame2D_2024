using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] int healthDamaged = 1;
    //[SerializeField] int healthTaked = 1;

    EnemyHealth enemyHealth;

    void Start()
    {
        //enemyHealth = FindFirstObjectByType<EnemyHealth>();
    }

    /*public void ApllyDamge()
    {
        if (enemyHealth == null) return;
        //enemyHealth.TakeDamage(healthDamaged);
    }*/
}
