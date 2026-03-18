using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Data")]
    public PlayerStatsData baseStats;

    [Header("Current Stats (Runtime)")]
    public float maxHealth;
    public float moveSpeed;
    public float attackDamage;
    public float attackSpeed;

    [Header("Cursed States")]
    [HideInInspector] public bool canDash = true;

    private Health healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
        ResetStats();
    }

    public void ResetStats()
    {
        if (baseStats == null) return;

        int metaDamageLevel = PlayerPrefs.GetInt("Meta_DamageLevel", 0);
        int metaHealthLevel = PlayerPrefs.GetInt("Meta_HealthLevel", 0);

        attackDamage = baseStats.attackDamage + (metaDamageLevel * 2f);
        maxHealth = baseStats.maxHealth + (metaHealthLevel * 10f);

        moveSpeed = baseStats.moveSpeed;
        attackSpeed = baseStats.attackSpeed;
        canDash = true;

        if (healthComponent != null)
        {
            healthComponent.maxHealth = (int)maxHealth;
            healthComponent.Heal((int)maxHealth);
        }
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null) return;

    
        if (upgrade.modifiers != null)
        {
            foreach (StatModifier modifier in upgrade.modifiers)
            {
                ApplyModifier(modifier.statType, modifier.value);
            }
        }

        Debug.Log($"Applied Upgrade: {upgrade.upgradeName}");
    }

    private void ApplyModifier(StatType type, float value)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                ApplyMaxHealthChange(value);
                break;
            case StatType.MoveSpeed:
                moveSpeed += value;
                break;
            case StatType.AttackDamage:
                attackDamage += value;
                break;
            case StatType.AttackSpeed:
                attackSpeed = Mathf.Max(0.1f, attackSpeed - value);
                break;
        }
    }

    private void ApplyMaxHealthChange(float amount)
    {
        maxHealth += amount;
        if (healthComponent != null)
        {
            healthComponent.maxHealth = (int)maxHealth;
        }
    }


    public void ApplyDamageBoost(float amount)
    {
        attackDamage += amount;
        Debug.Log($"Bargain: Gained {amount} Damage!");
    }

    public void ApplyMinorCurse(StatType type, float penaltyAmount)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                maxHealth = Mathf.Max(10, maxHealth - penaltyAmount);
                if (healthComponent != null) healthComponent.maxHealth = (int)maxHealth;
                break;
            case StatType.MoveSpeed:
                moveSpeed = Mathf.Max(1f, moveSpeed - penaltyAmount);
                break;
            case StatType.AttackSpeed:
                attackSpeed += penaltyAmount;
                break;
        }
    }

    public void ApplyMajorDebuff(int debuffType)
    {
        switch (debuffType)
        {
            case 0: 
                attackSpeed *= 2f;
                break;
            case 1:
                canDash = false;
                break;
            case 2: 
                maxHealth /= 2f;
                if (healthComponent != null) healthComponent.maxHealth = (int)maxHealth;
                break;
            case 3: 
                moveSpeed /= 2f;
                break;
        }
        Debug.Log("MAJOR CURSE APPLIED!");
    }

    public void RemoveMajorDebuff(int debuffType)
    {
        switch (debuffType)
        {
            case 0: attackSpeed /= 2f; break;
            case 1: canDash = true; break;
            case 2:
                maxHealth *= 2f;
                if (healthComponent != null) healthComponent.maxHealth = (int)maxHealth;
                break;
            case 3: moveSpeed *= 2f; break;
        }
        Debug.Log("Major Curse Lifted!");
    }
}