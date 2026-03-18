using System.Collections.Generic;  
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{
    [Header("Settings")]
    public List<UpgradeData> allUpgrades; 

    [Header("References")]
    public GameObject upgradePanel;   
    public UpgradeCard[] upgradeCards;   

    public PlayerStats playerStats;  
    public WaveManager waveManager;       

    private void Start() 
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }


    public void PresentUpgrades()
    {
        if (allUpgrades.Count == 0)
        {
            Debug.LogWarning("No upgrades in the list!");
            if (waveManager != null) waveManager.StartNextWave();
            return;
        }

        upgradePanel.SetActive(true);
        Time.timeScale = 0f;

        List<UpgradeData> availableUpgrades = new List<UpgradeData>(allUpgrades);

        foreach (UpgradeCard card in upgradeCards)
        {
            if (availableUpgrades.Count == 0)
            {
                card.gameObject.SetActive(false);
                continue;
            }

            card.gameObject.SetActive(true);

            int randomIndex = Random.Range(0, availableUpgrades.Count);
            UpgradeData randomData = availableUpgrades[randomIndex];

            card.Setup(randomData, this);

            availableUpgrades.RemoveAt(randomIndex);
        }
    }

    public void SelectUpgrade(UpgradeData chosenUpgrade)
    {
        if (playerStats != null)
        {
            playerStats.ApplyUpgrade(chosenUpgrade);
        }

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;

        if (waveManager != null)
        {
            waveManager.StartNextWave();
        }
    }
}