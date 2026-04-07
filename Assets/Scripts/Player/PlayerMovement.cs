using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// This makes sure the GameObject always has a Rigidbody2D,
// because this script needs one to move the player.
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private PlayerStats stats;   //how fast the player is moving

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;        // reference to the Rigidbody2D

    //input + movement state
    private Vector2 inputDir;      //current input direction
    private Vector2 lastMoveDir;   // last non zero movement direction

    //dash state
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldownRemaining = 0f;

    //knockback state
    private bool isKnockback = false;
    private float knockbackTimeRemaining = 0f;
    private Vector2 knockbackDir;
    private float knockbackStrength;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        // this runs when the object is created 
        // here we grab the Rigidbody2D component from the same GameObject
    }

    void Update()
    {
        // ---- READ INPUT ----
        float x = 0f;
        float y = 0f;

        //check if movement keys are pressed and update direction
        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;
        if (Input.GetKey(KeyCode.S)) y = -1f;
        if (Input.GetKey(KeyCode.W)) y = 1f;

        inputDir = new Vector2(x, y).normalized;

        //store last non-zero movement direction
        if (inputDir != Vector2.zero)
        {
            lastMoveDir = inputDir;
        }

        //---- HANDLE DASH COOLDOWN ----
        if (dashCooldownRemaining > 0f)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }

        // ---- START DASH ----
        if (!isDashing &&
            !isKnockback &&                        //cant start dash while being knocked back
            dashCooldownRemaining <= 0f &&
            Input.GetKeyDown(KeyCode.Space) &&
            lastMoveDir != Vector2.zero &&
            stats.canDash)
        {
            StartDash();
        }

        // ---- DASH TIMER ----
        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f)
            {
                EndDash();
            }
        }

        // ---- KNOCKBACK TIMER ----
        if (isKnockback)
        {
            knockbackTimeRemaining -= Time.deltaTime;
            if (knockbackTimeRemaining <= 0f)
            {
                isKnockback = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Order of priority: Knockback > Dash > Normal movement

        if (isKnockback)
        {
            rb.linearVelocity = knockbackDir * knockbackStrength;
        }
        else if (isDashing)
        {
            rb.linearVelocity = lastMoveDir * dashSpeed;
        }
        else
        {
            rb.linearVelocity = inputDir * stats.moveSpeed;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }

    private void EndDash()
    {
        isDashing = false;
    }

    // Expose last move direction for aiming / attacks
    public Vector2 LastMoveDir => lastMoveDir;

    // --------- KNOCKBACK API ---------
    public void ApplyKnockback(Vector2 knockDirection, float strength, float duration)
    {
        if (knockDirection == Vector2.zero)
        {
            knockDirection = -lastMoveDir; // fallback
        }

        knockbackDir = knockDirection.normalized;
        knockbackStrength = strength;
        knockbackTimeRemaining = duration;
        isKnockback = true;

        //Cancel dash if we get hit during dash)
        isDashing = false;
    }
}
