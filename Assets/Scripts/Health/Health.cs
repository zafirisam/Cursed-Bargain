using System.Collections;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    [Header("Health")]
    public int maxHealth = 100; // This will be overwritten by PlayerStats
    private int currentHealth;
    public int CurrentHealth => currentHealth; // this is so that the UI can get the current health of the player

    [Header("Damage Feedback")]
    public SpriteRenderer spriteRenderer;   // assign in Inspector (or auto-find)
    public Color hurtColor = Color.red;     // color when hurt
    public float flashDuration = 0.1f;      // how long the flash lasts

    private Color originalColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        // We handle SpriteRenderer setup here
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        // Moved this to Start() so it runs AFTER PlayerStats sets the correct MaxHealth
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log($"{gameObject.name} took {amount} damage. HP = {currentHealth}");

        // Start visual feedback
        if (spriteRenderer != null)
        {
            // if a previous flash is running, stop it so they don't fight
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(DamageFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} healed for {amount}. Current HP: {currentHealth}");
    }

    private IEnumerator DamageFlash()
    {
        // change to hurt color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hurtColor;
        }

        // wait
        yield return new WaitForSeconds(flashDuration);

        // back to normal
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        flashRoutine = null;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        OnDeath?.Invoke();

        Destroy(gameObject);   // later: replace with death anim / loot, etc.
    }
}