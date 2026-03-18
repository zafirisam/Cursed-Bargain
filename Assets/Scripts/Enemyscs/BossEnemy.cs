using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private CursedBargainManager bargainManager;
    private Health myHealth;

    private void Start()
    {
        bargainManager = Object.FindFirstObjectByType<CursedBargainManager>();

        myHealth = GetComponent<Health>();
        if (myHealth != null)
        {
            myHealth.OnDeath += HandleBossDeath;
        }
    }

    private void HandleBossDeath()
    {
        if (bargainManager != null)
        {
            bargainManager.UnlockBargain();

            bargainManager.OnBossDefeated();
        }
    }

    private void OnDestroy()
    {
        if (myHealth != null) myHealth.OnDeath -= HandleBossDeath;
    }
}