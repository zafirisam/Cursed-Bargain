using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Settings")]
    public int currentCoins = 0;

    private const string COIN_SAVE_KEY = "MetaCurrency";

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

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        SaveCoins(); 
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {currentCoins}";
        }
    }

    // ---- SAVING & LOADING ----

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COIN_SAVE_KEY, currentCoins);
        PlayerPrefs.Save(); 
        Debug.Log("Game Saved!");
    }

    private void LoadCoins()
    {
        
        currentCoins = PlayerPrefs.GetInt(COIN_SAVE_KEY, 0);
    }

    private void ResetData()
    {
        currentCoins = 0;
        PlayerPrefs.DeleteKey(COIN_SAVE_KEY);
        UpdateUI();
        Debug.Log("Save Data Reset!");
    }
}