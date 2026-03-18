using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections; 

public class CursedBargainManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public Button bargainButton;
    public TextMeshProUGUI feedbackText;

    [Header("Settings")]
    public float damageGain = 5f;
    public int majorDebuffFrequency = 5;
    private int usageCount = 0;
    private List<int> activeMajorDebuffs = new List<int>();

    private Coroutine feedbackCoroutine;

    private void Start()
    {
        if (bargainButton != null)
            bargainButton.gameObject.SetActive(false);
    }

    public void UnlockBargain()
    {
        if (bargainButton != null)
        {
            bargainButton.gameObject.SetActive(true);
            bargainButton.onClick.RemoveAllListeners();
            bargainButton.onClick.AddListener(TakeBargain);
            ShowFeedback("The Cursed Bargain is open!!!");
        }
    }

    public void TakeBargain()
    {
        if (playerStats == null) return;

        usageCount++;
        playerStats.ApplyDamageBoost(damageGain);

        if (usageCount % majorDebuffFrequency == 0)
        {
            ApplyMajorCurse();
        }
        else
        {
            ApplyRandomMinorSacrifice();
        }
    }

    private void ApplyRandomMinorSacrifice()
    {
        int roll = Random.Range(0, 3);
        string msg = "";

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

    private void ApplyMajorCurse()
    {
        int curseType = Random.Range(0, 4);
        playerStats.ApplyMajorDebuff(curseType);
        activeMajorDebuffs.Add(curseType);

        ShowFeedback("MAJOR CURSE: " + GetCurseName(curseType));
    }

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

    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;

        if (feedbackCoroutine != null) StopCoroutine(feedbackCoroutine);

        feedbackCoroutine = StartCoroutine(ClearFeedbackAfterDelay(2f));
    }

    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        feedbackText.text = "";
        feedbackCoroutine = null;
    }

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