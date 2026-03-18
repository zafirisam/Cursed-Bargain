using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangedEnemyChase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float stopDistance = 5f;
    public float distanceBuffer = 0.5f;   // avoids jitter around the edge

    [Header("Combat")]
    public GameObject projectilePrefab;
    public float fireRate = 2f;
    private float fireTimer;

    [Header("Prediction")]
    [Range(0, 1)] public float predictionAccuracy = 1.0f;

    public Transform target;              // Player
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-find player if not set in Inspector
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }
    
    private void Update()
    {
        if (fireTimer > 0) 
        {
            fireTimer -= Time.deltaTime;
        }           
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = (Vector2)target.position - rb.position;
        float distance = toPlayer.magnitude;

        // If far away -> move toward player
        if (distance > stopDistance + distanceBuffer)
        {
            Vector2 dir = toPlayer.normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        // If close enough -> stop (ranged attack zone)
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (fireTimer <= 0)
            {
                Shoot();
                fireTimer = fireRate;
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || target == null) return;

        Vector2 targetPosition = target.position;

        if (playerRb != null && predictionAccuracy > 0)
        {            
            float bulletSpeed = projectilePrefab.GetComponent<EnemyProjectile>().speed;

            float distance = Vector2.Distance(transform.position, target.position);
            float travelTime = distance / bulletSpeed;

            Vector2 prediction = playerRb.linearVelocity * travelTime * predictionAccuracy;
            targetPosition += prediction;
        }

        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        EnemyProjectile projScript = bullet.GetComponent<EnemyProjectile>();

        if (projScript != null)
        {
            Vector2 shootDir = (targetPosition - (Vector2)transform.position).normalized;
            projScript.Setup(shootDir);
        }
    }
}
