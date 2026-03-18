using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback2D : MonoBehaviour
{
    public float knockbackDuration = 0.15f;   // how long the knockback lasts

    private Rigidbody2D rb;
    private bool isKnockedBack;
    private float knockbackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isKnockedBack) return;

        knockbackTimer -= Time.deltaTime;
        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;
        }
    }

    /// <summary>
    /// Apply knockback in a direction with given force, using the default duration.
    /// </summary>
    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        direction.Normalize();
        rb.linearVelocity = direction * force;
    }

    /// <summary>
    /// Apply knockback with a custom duration (used by PlayerMeleeAttack).
    /// </summary>
    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        knockbackDuration = duration;
        ApplyKnockback(direction, force);
    }

    public bool IsKnockedBack => isKnockedBack;
}
