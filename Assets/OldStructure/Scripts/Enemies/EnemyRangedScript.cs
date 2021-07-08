using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedScript : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] float enemyMoveSpeed = 1f;
    [SerializeField] float safetyDistance = 1f;
    [SerializeField] float retreatDistance = 1f;

    private bool canMove;

    [Header("Enemy Projectile Damage")]
    [SerializeField] AudioClip projectileSound;
    [SerializeField] [Range(0, 1)] float projectileSoundVolume = 1f;
    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] Transform targetPlayer;
    [SerializeField] float cooldownDuration = 1f;

    private bool insideCamera = false;
    private bool canShoot = true;
    private float cooldownTimer = 0;
    private Collider2D thisObjectCollider;
    private Camera gameCamera;
    private Plane[] planes;

    // Cached References
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
    }

    // Update is called once per frame
    void Update()
    {
        ControllEnemyMovementAndShooting();

        HandleShootingCooldownTime();

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

    private void HandleShootingCooldownTime()
    {
        if (cooldownTimer <= 0)
        {   
            canShoot = true;
        }
        else
        {
            canShoot = false;
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void ControllEnemyMovementAndShooting()
    {
        canMove = standardEnemyScript.CanEnemyMove();

        if (targetPlayer && canMove)
        {
            if (transform.position.y > 4.5)
            {
                Vector2 newPosition = new Vector2(transform.position.x, 4.5f);
                transform.position = Vector2.MoveTowards(transform.position, newPosition, enemyMoveSpeed * Time.deltaTime);
                
                if (canShoot && insideCamera)
                {
                    ShootProjectile();
                }
            }
            else if (transform.position.y < -4.5)
            {
                Vector2 newPosition = new Vector2(transform.position.x, -4.5f);
                transform.position = Vector2.MoveTowards(transform.position, newPosition, enemyMoveSpeed * Time.deltaTime);
                
                if (canShoot && insideCamera)
                {
                    ShootProjectile();
                }
            }
            else if (transform.position.x > 8.5)
            {
                Vector2 newPosition = new Vector2(8.5f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, newPosition, enemyMoveSpeed * Time.deltaTime);

                if (canShoot && insideCamera)
                {
                    ShootProjectile();
                }
            }
            else if (transform.position.x < -8.5)
            {
                Vector2 newPosition = new Vector2(-8.5f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, newPosition, enemyMoveSpeed * Time.deltaTime);

                if (canShoot && insideCamera)
                {
                    ShootProjectile();
                }
            }
            else if (Vector2.Distance(transform.position, targetPlayer.position) > safetyDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, enemyMoveSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, targetPlayer.position) < safetyDistance && Vector2.Distance(transform.position, targetPlayer.position) > retreatDistance)
            {
                transform.position = this.transform.position;

                if (canShoot && insideCamera)
                {
                    ShootProjectile();
                }
            }
            else if (Vector2.Distance(transform.position, targetPlayer.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, -enemyMoveSpeed * Time.deltaTime);

                if (canShoot && insideCamera)
                {
                    ShootProjectile();
                }
            }
        }
    }

    private void ShootProjectile()
    {
        if (canMove)
        {
            AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileSoundVolume);
            GameObject newProjectile = Instantiate(enemyProjectilePrefab, transform.position, Quaternion.identity) as GameObject;
            cooldownTimer = cooldownDuration;
        }
    }
}
