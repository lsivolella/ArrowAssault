using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Configuration")]
public class ProjectileConfigurationSO : ScriptableObject
{
    [SerializeField] float projectileSpeed = 2f;
    [SerializeField] float lifeExpectancy = 2f;

    public float ProjectileSpeed { get { return projectileSpeed; } }
    public float LifeExpectancy { get { return lifeExpectancy; } }
}
