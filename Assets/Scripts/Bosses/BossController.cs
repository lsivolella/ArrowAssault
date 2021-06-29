using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] Vector2[] movementWaypoints;
    [SerializeField] float enemyMoveSpeed = 1f;

    private bool canMove;
    private Vector2 targetSpot;

    [Header("Enemy Damage")]
    [SerializeField] AudioClip projectileSound;
    [SerializeField] [Range(0, 1)] float projectileSoundVolume = 1f;
    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] Transform targetPlayer;
    [SerializeField] int numberOfProjectiles = 1;
    [SerializeField] float cooldownDuration = 1f;

    private bool insideCamera = false;
    private float cooldownTimer = 0;
    private const string PROJECTILE_PARENT_NAME = "Boss";
    private GameObject projectileParent;
    private Collider2D thisObjectCollider;
    private Camera gameCamera;
    private Plane[] planes;

    [Header("Enemy Rage Mode")]
    [SerializeField] float ragePercent = 1f;
    [SerializeField] bool rageMode = false;

    private float enemyHealthRage;

    // General Cached References
    EnemyStandardScript standardEnemyScript;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        standardEnemyScript = GetComponent<EnemyStandardScript>();
        cooldownTimer = cooldownDuration;
        gameCamera = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(gameCamera);
        thisObjectCollider = GetComponent<Collider2D>();

        GetNextWaypoint();
        DefineBossRageMode();
        CreateProjectileParent();
    }

    private void DefineBossRageMode()
    {
        float enemyHealth = standardEnemyScript.GetEnemyHealth();
        enemyHealthRage = enemyHealth * ragePercent / 100;
    }

    void Update()
    {
        BossMovementAndShooting();

        TestEnemyPosition();
    }

    private void TestEnemyPosition()
    {
        if (GeometryUtility.TestPlanesAABB(planes, thisObjectCollider.bounds))
        {
            insideCamera = true;
        }
        else
        {
            insideCamera = false;
        }
    }

    private void CreateProjectileParent()
    {
        projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);
        if (!projectileParent)
        {
            projectileParent = new GameObject(PROJECTILE_PARENT_NAME);
        }
    }

    private void BossMovementAndShooting()
    {
        canMove = standardEnemyScript.CanEnemyMove();

        if (targetPlayer && canMove)
        {
            if (transform.position.x != targetSpot.x && transform.position.y != targetSpot.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetSpot, enemyMoveSpeed * Time.deltaTime);
            }
            else
            {
                GetNextWaypoint();
            }

            if (cooldownTimer <= 0)
            {
                ShootProjectile();
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }
        float enemyHealth = standardEnemyScript.GetEnemyHealth();

        if (enemyHealth <= enemyHealthRage && !rageMode)
        {
            TurnOnRageMode();
        }
    }

    private void TurnOnRageMode()
    {
        rageMode = true;
        enemyMoveSpeed *= 1.3f;
        cooldownDuration *= 0.75f;
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    private void ShootProjectile()
    {
        cooldownTimer = cooldownDuration;
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        if (insideCamera)
        {
            AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileSoundVolume);
            for (int i = 0; i <= numberOfProjectiles - 1; i++)
            {
                float xPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 360;
                float yPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * 360;

                Vector2 projectileDirection = new Vector2(xPosition, yPosition);

                GameObject projectile = Instantiate(enemyProjectilePrefab, transform.position, Quaternion.identity) as GameObject;
                projectile.GetComponent<BossProjectileController>().SetDirection(projectileDirection * 3);
                projectile.transform.parent = projectileParent.transform;

                angle += angleStep;
            }
        }
    }

    private void GetNextWaypoint()
    {
        int randomWaypoint = Random.Range(0, movementWaypoints.Length);
        targetSpot = movementWaypoints[randomWaypoint];
    }
}
