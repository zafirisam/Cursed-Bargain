using UnityEngine;
using TMPro;

/// <summary>
/// A Singleton manager that tracks, saves, and loads the player's currency (coins) across sessions.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Settings")]
    public int currentCoins = 0;

    private const string COIN_SAVE_KEY = "Coins";

    [Header("UI Reference")]
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadCoins(); 
        UpdateUI();
    }

    private void Update()
    {
        
        if (Application.isEditor && Input.GetKeyDown(KeyCode.R))
        {
            ResetData();
        }
    }
    
    /// <summary>
    /// Adds a specified amount of coins to the player's inventory, saves the game, and updates the UI.
    /// </summary>
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        SaveCoins(); 
        UpdateUI();
    }

    /// <summary>
    /// Refreshes the TextMeshPro UI element to display the current coin total.
    /// </summary>
    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {currentCoins}";
        }
    }

    /// <summary>
    /// Saves the current coin amount to the device using PlayerPrefs.
    /// </summary>
    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COIN_SAVE_KEY, currentCoins);
        PlayerPrefs.Save(); 
        Debug.Log("Game Saved!");
    }

    /// <summary>
    /// Loads the saved coin amount from the device. Defaults to 0 if no save data exists.
    /// </summary>
    private void LoadCoins()
    {
        
        currentCoins = PlayerPrefs.GetInt(COIN_SAVE_KEY, 0);
    }

    /// <summary>
    /// Wipes the player's saved coin data. Used for debugging and testing.
    /// </summary>
    private void ResetData()
    {
        currentCoins = 0;
        PlayerPrefs.DeleteKey(COIN_SAVE_KEY);
        UpdateUI();
        Debug.Log("Save Data Reset!");
    }
}