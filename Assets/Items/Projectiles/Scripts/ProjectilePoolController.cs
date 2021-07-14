using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolController : MonoBehaviour
{
    [SerializeField] ProjectilePoolConfigurationSO poolConfigSO;

    private readonly Queue<GameObject> projectilesPool = new Queue<GameObject>();
    private GameObject projectileParent;

    private void Awake()
    {
        projectileParent = new GameObject(gameObject.name + "'s Projectiles Pool");
        PopulateProjectilesPool();
    }

    private void PopulateProjectilesPool()
    {
        for (int i = 0; i < poolConfigSO.PoolSize; i++)
        {
            GameObject newProjectile = Instantiate(poolConfigSO.ProjectilePrefab);
            newProjectile.transform.SetParent(projectileParent.transform);
            newProjectile.SetActive(false);
            projectilesPool.Enqueue(newProjectile);
        }
    }

    public GameObject DequeueProjectile()
    {
        var projectile = projectilesPool.Dequeue();
        projectile.GetComponent<ProjectileController>().onImpactOrExpiration += EnqueueProjectile;
        projectile.SetActive(true);
        return projectile;
    }

    private void EnqueueProjectile(GameObject projectile)
    {
        projectilesPool.Enqueue(projectile);
        projectile.GetComponent<ProjectileController>().onImpactOrExpiration -= EnqueueProjectile;
        projectile.SetActive(false);
    }
}
