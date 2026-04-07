using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangedEnemyChase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float stopDistance = 5f;
    public float distanceBuffer = 0.5f;   

    [Header("Combat")]
    public GameObject projectilePrefab;
    public float fireRate = 2f;
    private float fireTimer;

    [Header("Prediction")]
    [Range(0, 1)] public float predictionAccuracy = 1.0f;

    public Transform target;              
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
                playerRb = playerObj.GetComponent<Rigidbody2D>();
            }
        }
    }
    
    private void Update()
    {
        //Handles fire rate cooldown
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

        
        if (distance > stopDistance + distanceBuffer)
        {
            Vector2 dir = toPlayer.normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        
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
        //Aim Prediction, calculate where the player will be by the time the bullet arrives
        if (playerRb != null && predictionAccuracy > 0)
        {            
            float bulletSpeed = projectilePrefab.GetComponent<EnemyProjectile>().speed;

            float distance = Vector2.Distance(transform.position, target.position);
            float travelTime = distance / bulletSpeed;
            //Offset target position based on player velocity and travel time 
            Vector2 prediction = playerRb.linearVelocity * travelTime * predictionAccuracy;
            targetPosition += prediction;
        }
        //Spawn and setup the projectile
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        EnemyProjectile projScript = bullet.GetComponent<EnemyProjectile>();

        if (projScript != null)
        {
            Vector2 shootDir = (targetPosition - (Vector2)transform.position).normalized;
            projScript.Setup(shootDir);
        }
    }
}
