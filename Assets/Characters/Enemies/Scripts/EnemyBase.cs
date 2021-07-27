using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    [SerializeField] LootDropConfigurationSO lootConfigSO;
    [SerializeField] EnemiesConfigurationSO configSO;

    // Sprite Color Variables
    private Color originalColor;
    private Color freezeColor = Color.blue;

    // General Variables
    public EnemiesConfigurationSO ConfigSO { get { return configSO; } }
    public bool IsFrozen { get; private set; } = false; // TODO: check real necessity
    public bool CanMove { get; private set; } = true;
    public Vector2 MoveDestination { get; set; }
    public GameObject Target { get; private set; }

    // General Cached References
    protected HealthSystem healthSystem;
    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;
    protected Animator animator;
    protected LootController lootController;
    protected ObjectPooler lootPooler;


    // take damage (-life and damage blink)
    // finder?

    protected override void OnAwake()
    {
        base.OnAwake();

        GetComponents();

        originalColor = spriteRenderer.color;
        lootController = new LootController(this, lootConfigSO);

    }

    protected virtual void GetComponents()
    {
        healthSystem = GetComponent<HealthSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        Target = GameObject.FindGameObjectWithTag("Player");
        healthSystem.Setup(configSO.Health);
    }

    public override void TakeDamage(float amount)
    {
        //audioSource?.PlayOneShot(configSO.OnDamageAudioClip, configSO.OnDamageClipVolume);
        
        healthSystem.TakeDamage(amount);
        OnDamageTaken(healthSystem.CurentHealth);

        KillEnemy();
    }

    protected void KillEnemy()
    {
        if (healthSystem.CurentHealth > 0) return;

        OnDeath();
        IsAlive = false;
        //TODO: implement enemy object pulling
        //TODO: implement a little puff smoke
        // TODO: just deactivate enemy
        animator.enabled = false;
        spriteRenderer.enabled = false;
        BoxCollider.enabled = false;
        //Destroy(gameObject);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        GetMovementDirection();
        ProcessMovement();
    }

    protected virtual void GetMovementDirection() { }

    protected virtual void ProcessMovement()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        var pos = transform.position;

        var rayCastX = Physics2D.BoxCast(pos, BoxCollider.size / 1.5f, 0, new Vector2(MoveDestination.x, 0),
            Mathf.Abs(MoveDestination.x * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));
        var rayCastY = Physics2D.BoxCast(pos, BoxCollider.size / 1.5f, 0, new Vector2(0, MoveDestination.y),
            Mathf.Abs(MoveDestination.y * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));

        if (rayCastX.collider == null)
        {
            transform.Translate(MoveDestination.x * configSO.MoveSpeed * Time.deltaTime, 0, 0);
            PlayWalkAnimation();
        }
        if (rayCastY.collider == null)
        {
            transform.Translate(0, MoveDestination.y * configSO.MoveSpeed * Time.deltaTime, 0);
            PlayWalkAnimation();
        }
    }

    protected virtual void PlayIdleAnimation()
    {
        animator.Play("idle");
    }

    protected virtual void PlayWalkAnimation()
    {
        animator.Play("walk");
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        //TODO: remove int casting
        collider.gameObject.GetComponent<PlayerBase>().TakeDamage((int)ConfigSO.MeleeDamage);
        StartCoroutine(FlinchAfterCollision());
    }

    IEnumerator FlinchAfterCollision()
    {
        CanMove = false;
        PlayIdleAnimation();

        yield return new WaitForSeconds(ConfigSO.FlinchDuration);

        CanMove = true;
    }

    IEnumerator FreezeAfterCollision()
    {
        CanMove = false;
        IsFrozen = true;
        PlayIdleAnimation();
        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(ConfigSO.FreezeDuration);

        CanMove = true;
        IsFrozen = false;
        spriteRenderer.color = originalColor;
    }
}
