using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Knockback2D))]
public class EnemyMeleeChase : MonoBehaviour
{
    public float moveSpeed = 3f;

    public Transform target;
    private Rigidbody2D rb;
    private Knockback2D knockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback2D>();

        //Automatically find the player by tag if not assigned 
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        //Prevent AI movement if the enemyu is currently being knocked back
        if (knockback != null && knockback.IsKnockedBack)
            return;

        //Stop moving if the target is lost
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        //Move the Rigidbody directly towards the target
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }
}
