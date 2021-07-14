using UnityEngine;

public class RangedEnemyBase : EnemyBase
{
    [SerializeField] RangedEnemyConfigurationSO rangedConfigSO;

    private ProjectilePoolController projectilesPool;
    private Camera gameCam;
    private Timer timer;
    private Plane[] planes;
    private bool insideCamera = false;

    public bool CanAttack { get; private set; } = true;

    protected override void OnAwake()
    {
        base.OnAwake();

        projectilesPool = GetComponent<ProjectilePoolController>();
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

        GameObject projectile = projectilesPool.DequeueProjectile();
        projectile.GetComponent<ProjectileController>().Setup(transform.position, Quaternion.identity, Target);

        timer = new Timer(gameObject.name + "attack cooldown",
            rangedConfigSO.AttackCooldown, () =>
            {
                CanAttack = true;
            }, runOnce: true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangedConfigSO.MaxDistance);
        Gizmos.DrawWireSphere(transform.position, rangedConfigSO.RetreatDistance);
    }
}
