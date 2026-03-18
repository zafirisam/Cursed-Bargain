using UnityEngine;

[RequireComponent(typeof(Health))]
public class LootBag : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject healthPrefab;

    [Header("Drop Rates")]
    [Range(0, 1)] public float coinDropChance = 0.25f;
    [Range(0, 1)] public float healthDropChance = 0.15f;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        //Subscribe to the death event
        if (health != null) health.OnDeath += DropLoot;
    }

    private void OnDisable()
    {
        //Unsubscribe to prevent errors
        if (health != null) health.OnDeath -= DropLoot;
    }

    private void DropLoot()
    {
        float roll = Random.value;
        if (coinPrefab != null && roll <= coinDropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        float healthRoll = Random.value;
        if (healthPrefab != null && healthRoll <= healthDropChance)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }
    }
}