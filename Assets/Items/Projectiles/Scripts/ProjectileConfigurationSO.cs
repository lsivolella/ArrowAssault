using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Configuration")]
public class ProjectileConfigurationSO : ScriptableObject
{
    [SerializeField] float projectileSpeed = 2f;
    [SerializeField] float projectileDamage = 1f;
    [SerializeField] float lifeExpectancy = 2f;
    [SerializeField] bool isSeeker = false;

    public float ProjectileSpeed { get { return projectileSpeed; } }
    public float ProjectileDamage { get { return projectileDamage; } }
    public float LifeExpectancy { get { return lifeExpectancy; } }
    public bool IsSeeker { get { return isSeeker; } }
}
