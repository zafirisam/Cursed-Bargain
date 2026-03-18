using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public struct StatModifier
{
    public StatType statType;
    public float value; 
}

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Progression/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("UI Info")]
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Stats")]
    
    public List<StatModifier> modifiers;
}