using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Manages the core "Cursed Bargain" risk-reward mechanic. 
/// Handles player sacrifices, applies stat modifications (buffs and curses), 
/// and updates the UI feedback.
/// </summary>
public class CursedBargainManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public Button bargainButton;
    public TextMeshProUGUI feedbackText;

    [Header("Settings")]
    public float damageGain = 5f;
    public int majorDebuffFrequency = 5;
    
    //Tracks how many times the player has used the bargain during the current cycle.
    private int usageCount = 0;
    
    //Stores active major curses so they can be removed later.
    private List<int> activeMajorDebuffs = new List<int>();

    //Reference to the active feedback coroutine so we can stop it if a new message appears.
    private Coroutine feedbackCoroutine;

    private void Start()
    {
        //Hide the bargain button by default until the mechanic is unlocked.
        if (bargainButton != null)
            bargainButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the Cursed Bargain mechanic, making the UI button visible and interactable.
    /// </summary>
    public void UnlockBargain()
    {
        if (bargainButton != null)
        {
            bargainButton.gameObject.SetActive(true);
            //Clear any existing listeners to prevent multiple triggers, then assign the TakeBargain method.
            bargainButton.onClick.RemoveAllListeners();
            bargainButton.onClick.AddListener(TakeBargain);
            ShowFeedback("The Cursed Bargain is open!!!");
        }
    }

    /// <summary>
    /// Triggered when the player clicks the Bargain button. 
    /// Grants an immediate damage boost and applies either a minor sacrifice or a major curse.
    /// </summary>
    public void TakeBargain()
    {
        if (playerStats == null) return;

        usageCount++;
        //Always grant the offensive reward first.
        playerStats.ApplyDamageBoost(damageGain);
        //Check if the player has hit the threshold for a major punishment.
        if (usageCount % majorDebuffFrequency == 0)
        {
            ApplyMajorCurse();
        }
        else
        {
            ApplyRandomMinorSacrifice();
        }
    }

    /// <summary>
    /// Randomly selects and applies a minor stat penalty to the player.
    /// </summary>
    private void ApplyRandomMinorSacrifice()
    {
        int roll = Random.Range(0, 3);
        string msg = "";
        //Apply the penalty based on the random roll.
        switch (roll)
        {
            case 0:
                playerStats.ApplyMinorCurse(StatType.MaxHealth, 10f);
                msg = "Sacrificed 10 Max HP";
                break;
            case 1:
                playerStats.ApplyMinorCurse(StatType.MoveSpeed, 0.5f);
                msg = "Sacrificed Move Speed";
                break;
            case 2:
                playerStats.ApplyMinorCurse(StatType.AttackSpeed, 0.2f);
                msg = "Sacrificed Attack Speed";
                break;
        }
        ShowFeedback($"{msg} (+{damageGain} DMG)");
    }
    /// <summary>
    /// Applies a severe gameplay penalty and tracks it so it can be removed upon boss defeat.
    /// </summary>
    private void ApplyMajorCurse()
    {
        int curseType = Random.Range(0, 4);
        playerStats.ApplyMajorDebuff(curseType);
        activeMajorDebuffs.Add(curseType);

        ShowFeedback("MAJOR CURSE: " + GetCurseName(curseType));
    }

    /// <summary>
    /// Cleanses all active major curses and resets the usage counter. 
    /// Called when the player successfully defeats a boss.
    /// </summary>
    public void OnBossDefeated()
    {
        foreach (int curse in activeMajorDebuffs)
        {
            playerStats.RemoveMajorDebuff(curse);
        }
        activeMajorDebuffs.Clear();
        usageCount = 0;

        ShowFeedback("Curses Lifted!");
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            UnlockBargain();
            Debug.Log("Showcase Mode: Cursed Bargain Unlocked");
        }
    }
    /// <summary>
    /// Displays a temporary message on the screen to provide immediate player feedback.
    /// </summary>
    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;

        if (feedbackCoroutine != null) StopCoroutine(feedbackCoroutine);

        feedbackCoroutine = StartCoroutine(ClearFeedbackAfterDelay(2f));
    }

    /// <summary>
    /// Waits for a specified duration in real-time before clearing the feedback text.
    /// Uses Realtime so it works even if the game is paused (Time.timeScale = 0).
    /// </summary>
    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        feedbackText.text = "";
        feedbackCoroutine = null;
    }

    /// <summary>
    /// Helper method to convert the curse integer ID into a readable string for the UI.
    /// </summary>
    private string GetCurseName(int type)
    {
        switch (type)
        {
            case 0: return "Slow Attacks!";
            case 1: return "DASH DISABLED!";
            case 2: return "Frailty (Half HP)!";
            case 3: return "Slowness!";
            default: return "Doom";
        }
    }
}