using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    [SerializeField] PlayerConfigurationSO configSO;
    [SerializeField] Transform arrowTrigger;
    [SerializeField] PoolConfigurationSO arrowPoolConfigSO; 

    public event Action<ArrowConfigurationSO> onProjectileUpdate; 
    public event Action<int> onAmmoUpdate;

    // General Cached References
    private ObjectPooler projectilesPool;
    private HealthSystem healthSystem;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private BowHandler bowHandler;

    private Timer timer;

    public bool CanAttack { get; private set; } = true;

    private Vector2 movement;
    private int currentAmmunition;

    protected override void OnAwake()
    {
        base.OnAwake();

        GetComponents();

        projectilesPool = new ObjectPooler(this.gameObject, arrowPoolConfigSO);
    }

    private void GetComponents()
    {
        healthSystem = GetComponent<HealthSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bowHandler = GetComponentInChildren<BowHandler>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        healthSystem.Setup(configSO.Health);

        ArrowSetup();
    }

    private void ArrowSetup()
    {
        currentAmmunition = 0;
        configSO.CurrentProjectile = configSO.DefaultProjectile;
        UpdateProjectileEvents(configSO.CurrentProjectile, currentAmmunition);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        ProcessMovementInput();
        ProcessFireInput();
    }

    private void ProcessMovementInput()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        var pos = transform.position;

        var rayCastX = Physics2D.BoxCast(pos, BoxCollider.size / 1.5f, 0, new Vector2(movement.x, 0),
            Mathf.Abs(movement.x * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));
        var rayCastY = Physics2D.BoxCast(pos, BoxCollider.size / 1.5f, 0, new Vector2(0, movement.y),
            Mathf.Abs(movement.y * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));

        if (rayCastX.collider == null)
        {
            transform.Translate(movement.x * configSO.MoveSpeed * Time.deltaTime, 0, 0);
            animator.Play("walk");
        }
        if (rayCastY.collider == null)
        {
            transform.Translate(0, movement.y * configSO.MoveSpeed * Time.deltaTime, 0);
            animator.Play("walk");
        }
    }

    private void ProcessFireInput()
    {
        if (!Input.GetButton("Fire1")) return;

        if (!CanAttack) return;

        if (!configSO.CurrentProjectile.ArrowType.Equals(ArrowType.RegularArrow))
            SpendAmmo();

        CanAttack = false;

        var tag = projectilesPool.GenerateRandomTag();
        var projectile = projectilesPool.DequeueObject(tag).GetComponent<ArrowHandler>();
        projectile.ArrowConfigSO = configSO.CurrentProjectile;
        projectile.Setup(arrowTrigger.transform.position, arrowTrigger.rotation, null);
        projectile.onImpactOrExpiration += EnqueueProjectile;

        timer = new Timer(gameObject.name + "attack_cooldown",
            bowHandler.ConfigSO.ShotCooldown, () =>
            {
                CanAttack = true;
            }, runOnce: true);
    }

    private void EnqueueProjectile(GameObject projectile)
    {
        projectilesPool.EnqueueObject(arrowPoolConfigSO.Pools[0].Tag, projectile);
        projectile.GetComponent<ProjectileController>().onImpactOrExpiration -= EnqueueProjectile;
    }

    private void SpendAmmo()
    {
        if (currentAmmunition <= 0) return;

        currentAmmunition--;
        
        OnAmmoUpdate(currentAmmunition);

        if (currentAmmunition > 0) return;

        configSO.CurrentProjectile = configSO.DefaultProjectile;
        UpdateProjectileEvents(configSO.CurrentProjectile, currentAmmunition);
    }

    public override void TakeDamage(float amount)
    {
        audioSource?.PlayOneShot(configSO.OnDamageAudioClip, configSO.OnDamageClipVolume);
        
        healthSystem.TakeDamage(amount);
        OnDamageTaken(healthSystem.CurentHealth);

        if (healthSystem.CurentHealth > 0) return;

        KillPlayer();
    }

    private void KillPlayer()
    {

    }

    public void UpdateProjectileAndAmmo(ArrowConfigurationSO newProjectile, int amount)
    {
        if (configSO.CurrentProjectile == newProjectile)
            currentAmmunition += amount;
        else
        {
            currentAmmunition = amount;
            configSO.CurrentProjectile = newProjectile;
        }
        UpdateProjectileEvents(newProjectile, currentAmmunition);
    }

    public void UpdateProjectileEvents(ArrowConfigurationSO newProjectile, int currentAmmo)
    {
        OnProjectileUpdate(newProjectile);
        OnAmmoUpdate(currentAmmo);
    }

    private void OnProjectileUpdate(ArrowConfigurationSO newProjectile)
    {
        if (onProjectileUpdate != null) { onProjectileUpdate(newProjectile); }
    }

    private void OnAmmoUpdate(int currentAmmo)
    {
        if (onAmmoUpdate != null) { onAmmoUpdate(currentAmmo); }
    }
}
