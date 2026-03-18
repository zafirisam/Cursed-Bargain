using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    private PlayerStats stats;
    public float attackRange = 1.5f;
    public float attackAngle = 180f;

    [Header("Knockback Settings")]
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.15f;

    [Header("Visual Effects")]
    public GameObject attackVisualPrefab;
    public float visualDuration = 0.2f;
    public float visualOffset = 1.0f;
    public float visualEffectSize = 1f;

    [Header("Layers")]
    public LayerMask enemyLayer;

    private PlayerMovement movement;
    private float attackTimer;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();

        if (stats != null)
            attackTimer = stats.attackSpeed;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            PerformAttack();
            if (stats != null)
                attackTimer = stats.attackSpeed;
            else
                attackTimer = 1f;
        }
    }

    private void PerformAttack()
    {
        Vector2 facingDir = movement != null ? movement.LastMoveDir : Vector2.right;

        if (facingDir == Vector2.zero) facingDir = Vector2.right;

        // --- SPAWN VISUAL ---
        if (attackVisualPrefab != null)
        {
            Vector3 spawnPos = transform.position + (Vector3)facingDir * visualOffset;
            float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject effectInstance = Instantiate(attackVisualPrefab, spawnPos, rotation);

            // --- NEW: Apply the size change ---
            effectInstance.transform.localScale = new Vector3(visualEffectSize, visualEffectSize, visualEffectSize);

            Destroy(effectInstance, visualDuration);
        }

        // --- LOGIC ---
        Vector2 origin = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Vector2 toTarget = (Vector2)hit.transform.position - origin;
            float angleToTarget = Vector2.Angle(facingDir, toTarget);

            if (angleToTarget <= attackAngle * 0.5f)
            {
                Health health = hit.GetComponent<Health>();
                if (health != null && stats != null)
                {
                    health.TakeDamage((int)stats.attackDamage);
                }

                Knockback2D enemyKb = hit.GetComponent<Knockback2D>();
                if (enemyKb != null)
                {
                    Vector2 dir = (hit.transform.position - transform.position).normalized;
                    enemyKb.ApplyKnockback(dir, knockbackForce, knockbackDuration);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}