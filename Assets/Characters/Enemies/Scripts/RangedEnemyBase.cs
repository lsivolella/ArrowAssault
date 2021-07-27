using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBase : EnemyBase
{
    [SerializeField] RangedEnemyConfigurationSO rangedConfigSO;
    [SerializeField] PoolConfigurationSO projectilePoolConfigSO;

    private Camera gameCam;
    private Timer timer;
    private ObjectPooler projectilesPool;
    private Transform projectileTrigger;
    private Plane[] planes;

    private bool insideCamera = false;

    public bool CanAttack { get; private set; } = true;

    protected override void OnAwake()
    {
        base.OnAwake();

        projectilesPool = new ObjectPooler(this.gameObject, projectilePoolConfigSO);
        projectileTrigger = transform.Find("projectile_trigger");
    }

    protected override void OnStart()
    {
        base.OnStart();

        gameCam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(gameCam);
    }

    protected override void OnUpdate()
    {
        CheckPosition();

        base.OnUpdate();  

        ShotTarget();
    }

    private void CheckPosition()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        insideCamera = GeometryUtility.TestPlanesAABB(planes, BoxCollider.bounds);
    }

    protected override void GetMovementDirection()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        float distanceToTarget = Vector2.Distance(Target.transform.position, transform.position);

        if (distanceToTarget > rangedConfigSO.RetreatDistance &&
            distanceToTarget < rangedConfigSO.MaxDistance)
            return;

        if (distanceToTarget > rangedConfigSO.MaxDistance)
            MoveDestination = Target.transform.position - transform.position;
        else
            MoveDestination = transform.position - Target.transform.position;
    }

    protected override void ProcessMovement()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        PlayWalkAnimation();

        Vector2 destination = Vector2.MoveTowards(transform.position, MoveDestination, 
            ConfigSO.MoveSpeed * Time.deltaTime);
        float clampedXPosition = Mathf.Clamp(destination.x, -8.5f, 8.5f);
        float clampedYPosition = Mathf.Clamp(destination.y, -4.5f, 4.5f);
        transform.position = new Vector2(clampedXPosition, clampedYPosition);
    }

    protected virtual void ShotTarget()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        if (!insideCamera) return;

        if (!CanAttack) return;

        CanAttack = false;

        var tag = projectilesPool.GetDictionaryTag(0);
        var projectile = projectilesPool.DequeueObject(tag).GetComponent<ProjectileController>();
        projectile.Setup(projectileTrigger.position, Quaternion.identity, Target);
        projectile.onImpactOrExpiration += EnqueueProjectile;

        timer = new Timer(gameObject.name + "attack_cooldown",
            rangedConfigSO.AttackCooldown, () =>
            {
                CanAttack = true;
            }, runOnce: true);
    }

    protected void EnqueueProjectile(GameObject projectile)
    {
        projectilesPool.EnqueueObject(projectilePoolConfigSO.Pools[0].Tag, projectile);
        projectile.GetComponent<ProjectileController>().onImpactOrExpiration -= EnqueueProjectile;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangedConfigSO.MaxDistance);
        Gizmos.DrawWireSphere(transform.position, rangedConfigSO.RetreatDistance);
    }
}
