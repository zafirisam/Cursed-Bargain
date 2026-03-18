using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    public int contactDamage = 10;
    public float damageInterval = 2f;

    [Header("Player Knockback")]
    public float playerKnockbackForce = 8f;
    public float playerKnockbackDuration = 0.15f;

    private float cooldown;

    private void Update()
    {
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        if (cooldown > 0f)
            return;

        // Deal damage
        Health health = collision.collider.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(contactDamage);
        }

        // Direction from enemy to player
        Vector2 dir = (collision.collider.transform.position - transform.position).normalized;

        // Prefer the player's own movement knockback if present
        PlayerMovement playerMove = collision.collider.GetComponent<PlayerMovement>();
        if (playerMove != null)
        {
            playerMove.ApplyKnockback(dir, playerKnockbackForce, playerKnockbackDuration);
        }
        else
        {
            // Fallback: generic Knockback2D on the player if you add it
            Knockback2D playerKb = collision.collider.GetComponent<Knockback2D>();
            if (playerKb != null)
            {
                playerKb.ApplyKnockback(dir, playerKnockbackForce, playerKnockbackDuration);
            }
        }

        cooldown = damageInterval;
    }
}
