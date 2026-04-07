using UnityEngine;

/// <summary>
/// Represents a collectible coin in the game world.
/// Grants currency to the player upon collision.
/// </summary>
public class Coin : MonoBehaviour 
{
    public int coinValue = 1;

    /// <summary>
    /// Triggered when another 2D collider enters this object's trigger space.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCoins(coinValue);
            }

            Destroy(gameObject);
        }
    }

}
