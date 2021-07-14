using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Pool Configuration")]
public class ProjectilePoolConfigurationSO : ScriptableObject
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int poolSize;

    public GameObject ProjectilePrefab { get { return projectilePrefab; } }
    public int PoolSize { get { return poolSize; } }
}
