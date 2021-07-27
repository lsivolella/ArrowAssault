using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTreantEnemyBase : EnemyBase
{
    [SerializeField] SuperTreantConfigurationSO superTreantConfigSO;
    [SerializeField] PoolConfigurationSO projectilePoolConfigSO;

    private Camera gameCam;
    private ObjectPooler projectilesPool;
    private Transform projectileTrigger;
    private Plane[] planes;

    private bool insideCamera = false;
    private float currentMoveSpeed;

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

        currentMoveSpeed = ConfigSO.MoveSpeed;

        GetNextWaypoint();
    }

    protected override void OnUpdate()
    {
        CheckPosition();

        base.OnUpdate();
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

        if ((Vector2)transform.position != MoveDestination)
        {
            transform.position = Vector2.MoveTowards(transform.position, MoveDestination,
                currentMoveSpeed * Time.deltaTime);
            return;
        }
        ShotProjectiles();
        GetNextWaypoint();
    }

    private void GetNextWaypoint()
    {
        int randomWaypoint = Random.Range(0, superTreantConfigSO.MovementWaypoints.Count);
        MoveDestination = superTreantConfigSO.MovementWaypoints[randomWaypoint];
    }

    protected override void ProcessMovement()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        PlayWalkAnimation();

        transform.position = Vector2.MoveTowards(transform.position, MoveDestination,
            ConfigSO.MoveSpeed * Time.deltaTime);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        ActivateRageMode();
    }

    private void ActivateRageMode()
    {
        if (healthSystem.CurentHealth > superTreantConfigSO.RageModeTrigger * healthSystem.BaseHealth) return;

        currentMoveSpeed = (1 + superTreantConfigSO.RangeModeMoveSpeedFactor) * ConfigSO.MoveSpeed;
    }

    private void ShotProjectiles()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!insideCamera) return;

        float angleStep = 360f / superTreantConfigSO.NumberOfProjectiles;
        float currentAngle = 0f;
        Debug.Log("Angle Step: " + angleStep);
        // audioSource.Play();

        for (int i = 0; i < superTreantConfigSO.NumberOfProjectiles; i++)
        {
            var tag = projectilesPool.GetDictionaryTag(0);
            var projectile = projectilesPool.DequeueObject(tag).GetComponent<ProjectileController>();

            var projectileRotation = Quaternion.Euler(0f, 0f, 180 - currentAngle);
            projectile.transform.localScale =
                new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            projectile.Setup(projectileTrigger.position, projectileRotation, Target);
            projectile.onImpactOrExpiration += EnqueueProjectile;
            Debug.Log("Current Angle: " + currentAngle);
            currentAngle += angleStep;
        }
    }

    protected void EnqueueProjectile(GameObject projectile)
    {
        projectilesPool.EnqueueObject(projectilePoolConfigSO.Pools[0].Tag, projectile);
        projectile.GetComponent<ProjectileController>().onImpactOrExpiration -= EnqueueProjectile;
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        //TODO: remove int casting
        collider.gameObject.GetComponent<PlayerBase>().TakeDamage((int)ConfigSO.MeleeDamage);
    }
}
