using UnityEngine;

/// <summary>
/// Handles boss-specific logic, such as notifying the Bargain Manager upon death.
/// </summary>
public class BossEnemy : MonoBehaviour
{
    private CursedBargainManager bargainManager;
    private Health myHealth;

    private void Start()
    {
        //Locates the bargain manager in the scene
        bargainManager = Object.FindFirstObjectByType<CursedBargainManager>();
        //Subscribes to the Health components death event 
        myHealth = GetComponent<Health>();
        if (myHealth != null)
        {
            myHealth.OnDeath += HandleBossDeath;
        }
    }

    /// <summary>
    /// Triggered when the boss's health reaches zero.
    /// </summary>
    private void HandleBossDeath()
    {
        if (bargainManager != null)
        {
            //Unlocks new mechanics and notifies the manager of victory
            bargainManager.UnlockBargain();
            bargainManager.OnBossDefeated();
        }
    }

    //Unsubscribe to prevent memory leaks or errors when the object is destroyed
    private void OnDestroy()
    {
        if (myHealth != null) myHealth.OnDeath -= HandleBossDeath;
    }
}