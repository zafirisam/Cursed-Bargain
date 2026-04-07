using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    public int contactDamage = 10;
    public float damageInterval = 2f; //Cooldown between hits

    [Header("Player Knockback")]
    public float playerKnockbackForce = 8f;
    public float playerKnockbackDuration = 0.15f;

    private float cooldown;

    private void Update()
    {
        //Reduce the damage cooldown over time
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Only interact with the Player
        if (!collision.collider.CompareTag("Player"))
            return;
        //Exit if the cooldown hasnt expired yet
        if (cooldown > 0f)
            return;

        //Apply damage to the players Health component 
        Health health = collision.collider.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(contactDamage);
        }

        //Calculate the direction from this enemy to the player
        Vector2 dir = (collision.collider.transform.position - transform.position).normalized;

        //Handle knockback logic
        PlayerMovement playerMove = collision.collider.GetComponent<PlayerMovement>();
        if (playerMove != null)
        {
            //Use players specific movement knockback if available
            playerMove.ApplyKnockback(dir, playerKnockbackForce, playerKnockbackDuration);
        }
        else
        {
            //Use a generic knockback component as a fallback
            Knockback2D playerKb = collision.collider.GetComponent<Knockback2D>();
            if (playerKb != null)
            {
                playerKb.ApplyKnockback(dir, playerKnockbackForce, playerKnockbackDuration);
            }
        }
        //Reset the damage cooldown
        cooldown = damageInterval;
    }
}
