using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 5;
    public float lifeTime = 5f;

    private Vector2 moveDirection;
    private bool isInitialized = false;


    /// <summary>
    /// Initializes direction, rotation, and self-destruction timer.
    /// </summary>
    public void Setup(Vector2 dir)
    {
        moveDirection = dir.normalized;

        //Rotate the projectile to point toward its travel direction
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        isInitialized = true;
        Destroy(gameObject, lifeTime);//Cleanup after lifetime expires
    }

    private void Update()
    {
        if (!isInitialized) return;

        //Move the projectile forwards
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check for the player collision
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        //Destroy if it hits an obstacle
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}