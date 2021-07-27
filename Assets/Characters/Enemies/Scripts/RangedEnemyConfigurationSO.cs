using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Ranged Enemies Configuration")]
public class RangedEnemyConfigurationSO : ScriptableObject
{
    [Header("Movement Variables")]
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float retreatDistance = 6f;
    [Header("Attack Variables")]
    [SerializeField] float attackCooldown = 1f;

    public float MaxDistance { get { return maxDistance; } }
    public float RetreatDistance { get { return retreatDistance; } }
    public float AttackCooldown { get { return attackCooldown; } }
}
