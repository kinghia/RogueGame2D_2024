using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] Transform targetPlayer;
    [SerializeField] float moveSpeed = 2f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            targetPlayer = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player ko dc tim` thay");
        }
    }

    void Update()
    {
        if (targetPlayer != null)
        {
            // Di chuyển enemy về phía player
            Vector2 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }
}
