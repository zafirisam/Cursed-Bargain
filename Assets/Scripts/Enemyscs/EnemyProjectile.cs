using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 5;
    public float lifeTime = 5f;

    private Vector2 moveDirection;
    private bool isInitialized = false;

    // This is called by the RangedEnemyChase script
    public void Setup(Vector2 dir)
    {
        moveDirection = dir.normalized;

        // Rotate the sprite to face the player (visual only)
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        isInitialized = true;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!isInitialized) return;

        // Move in the specific direction in World Space
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}