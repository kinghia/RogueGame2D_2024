using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefabs;
    [SerializeField] Transform rangePlayer;
    [SerializeField] Transform enemyParent;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxRange = 15f;

    public float timeSpawn = 1f;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxRange);
            Vector3 spawnPosition = rangePlayer.position + new Vector3(randomDirection.x, randomDirection.y, 0 );
            yield return new WaitForSeconds(timeSpawn);
            Instantiate(enemyPrefabs, spawnPosition, Quaternion.identity, enemyParent);
            Debug.Log("spawn enemy");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (rangePlayer == null) return;

        Gizmos.DrawWireSphere(rangePlayer.position, minDistance);        
    }

}
