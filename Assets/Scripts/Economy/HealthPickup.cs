using UnityEngine;

///<summary>
///Represents a collectible health item in the game world.
///Restores a set amount of health to the player upon collision.
///</summary>
public class HealthPickup : MonoBehaviour 
{
    [Tooltip("The amount of health restored to the player when collected.")]
    public int healAmount = 5;

    ///<summary>
    ///Triggered when another 2D collider enters this object's trigger space.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the colliding object is the Player.
        if (other.CompareTag("Player"))
        {
            //Attempt to find the Health component on the player.
            Health playerHealth = other.GetComponent<Health>();
            
            //If the player has a Health component heal them and destroy the pickup.
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
