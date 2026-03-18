using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player Stats Data")]
public class PlayerStatsData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth = 50f;
    public float moveSpeed = 5f;
    public float attackDamage = 5f;
    public float attackSpeed = 2f; 
}
