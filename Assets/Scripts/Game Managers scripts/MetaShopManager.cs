using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MetaShopManager : MonoBehaviour 
{
    [Header("UI References")]
    public TextMeshProUGUI coinsText;

    [Header("Damage Upgrade (Cost : 10 Coins)")]
    public int damageCost = 10;
    public TextMeshProUGUI damageLevelText;


    [Header("Health Upgrade (Cost : 10 Coins)")]
    public int healthCost = 10;
    public TextMeshProUGUI healthLevelText;

    public void Start()
    {
        UpdateShopUI();
    }

    public void BuyDamageUpgrade()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (currentCoins >= damageCost)
        {
            PlayerPrefs.SetInt("Coins", currentCoins - damageCost);

            int currentLevel = PlayerPrefs.GetInt("Meta_DamageLevel", 0);
            PlayerPrefs.SetInt("Meta_DamageLevel", currentLevel + 1);

            PlayerPrefs.Save();
            UpdateShopUI();
        }
        else 
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void BuyHealthUpgrade()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (currentCoins >= healthCost)
        {
            PlayerPrefs.SetInt("Coins", currentCoins - healthCost);

            int currentLevel = PlayerPrefs.GetInt("Meta_HealthLevel", 0);
            PlayerPrefs.SetInt("Meta_HealthLevel", currentLevel + 1);

            PlayerPrefs.Save();
            UpdateShopUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void UpdateShopUI() 
    {
        if (coinsText != null) 
        {
            coinsText.text = $"Coins: {PlayerPrefs.GetInt("Coins", 0)}";

        }
        
        if (damageLevelText != null) 
        {
            damageLevelText.text = $"Damage Lvl: {PlayerPrefs.GetInt("Meta_DamageLevel", 0)}";
        }

        if (healthLevelText != null)
        {
            healthLevelText.text = $"Health Lvl: {PlayerPrefs.GetInt("Meta_HealthLevel", 0)}";
        }
    }

}
