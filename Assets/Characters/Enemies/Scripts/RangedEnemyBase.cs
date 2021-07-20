using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyBase : EnemyBase
{
    [SerializeField] RangedEnemyConfigurationSO rangedConfigSO;
    [SerializeField] PoolConfigurationSO projectilePoolConfigSO;

    private ObjectPooler projectilesPool;
    private Camera gameCam;
    private Timer timer;
    private Plane[] planes;
    private bool insideCamera = false;

    public bool CanAttack { get; private set; } = true;

    protected override void OnAwake()
    {
        base.OnAwake();

        projectilesPool = new ObjectPooler(this.gameObject, projectilePoolConfigSO);
    }

    protected override void OnStart()
    {
        base.OnStart();

        gameCam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(gameCam);
    }

    protected override void OnUpdate()
    {
        CheckTargetPosition();

        base.OnUpdate();  

        ShotTarget();
    }

    private void CheckTargetPosition()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        insideCamera = GeometryUtility.TestPlanesAABB(planes, BoxCollider.bounds);
    }

    protected override void ProcessMovement()
    {
        float distanceToTarget = Vector2.Distance(Target.transform.position, transform.position);
        Vector2 direction;

        if (distanceToTarget > rangedConfigSO.RetreatDistance &&
            distanceToTarget < rangedConfigSO.MaxDistance)
            return;

        if (distanceToTarget > rangedConfigSO.MaxDistance)
            direction = Target.transform.position - transform.position;
        else
            direction = transform.position - Target.transform.position;

        Animator.Play("walk");

        Vector2 destnation = Vector2.MoveTowards(transform.position, direction, 
            ConfigSO.MoveSpeed * Time.deltaTime);
        float clampedXPosition = Mathf.Clamp(destnation.x, -8.5f, 8.5f);
        float clampedYPosition = Mathf.Clamp(destnation.y, -4.5f, 4.5f);
        transform.position = new Vector2(clampedXPosition, clampedYPosition);
    }

    private void ShotTarget()
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
        projectile.Setup(transform.position, Quaternion.identity, Target);
        projectile.onImpactOrExpiration += EnqueueProjectile;

        timer = new Timer(gameObject.name + "attack_cooldown",
            rangedConfigSO.AttackCooldown, () =>
            {
                CanAttack = true;
            }, runOnce: true);
    }

    private void EnqueueProjectile(GameObject projectile)
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
