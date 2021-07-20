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
    public Vector2 MoveDirection { get; set; }
    public GameObject Target { get; private set; }

    // General Cached References
    private HealthSystem healthSystem;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private LootController lootController;
    private ObjectPooler lootPooler;

    public Animator Animator { get { return animator; } }

    // take damage (-life and damage blink)
    // finder?

    protected override void OnAwake()
    {
        base.OnAwake();

        GetComponents();

        originalColor = spriteRenderer.color;
        lootController = new LootController(this, lootConfigSO);

    }

    private void GetComponents()
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

        if (healthSystem.CurentHealth > 0) return;

        KillEnemy();
    }

    private void KillEnemy()
    {
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

        ProcessMovement();
    }

    protected virtual void ProcessMovement()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        var pos = transform.position;

        var rayCastX = Physics2D.BoxCast(pos, BoxCollider.size / 1.5f, 0, new Vector2(MoveDirection.x, 0),
            Mathf.Abs(MoveDirection.x * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));
        var rayCastY = Physics2D.BoxCast(pos, BoxCollider.size / 1.5f, 0, new Vector2(0, MoveDirection.y),
            Mathf.Abs(MoveDirection.y * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));

        if (rayCastX.collider == null)
        {
            transform.Translate(MoveDirection.x * configSO.MoveSpeed * Time.deltaTime, 0, 0);
            animator.Play("walk");
        }
        if (rayCastY.collider == null)
        {
            transform.Translate(0, MoveDirection.y * configSO.MoveSpeed * Time.deltaTime, 0);
            animator.Play("walk");
        }
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        base.OnCollideEnter(collider);

        if (!collider.gameObject.CompareTag("Player")) return;

        //TODO: remove int casting
        collider.gameObject.GetComponent<PlayerBase>().TakeDamage((int)configSO.MeleeDamage);
        StartCoroutine(FlinchAfterCollision());
    }

    IEnumerator FlinchAfterCollision()
    {
        CanMove = false;
        animator.Play("idle");

        yield return new WaitForSeconds(configSO.FlinchDuration);

        CanMove = true;
    }

    IEnumerator FreezeAfterCollision()
    {
        CanMove = false;
        IsFrozen = true;
        animator.Play("idle");
        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(configSO.FreezeDuration);

        CanMove = true;
        IsFrozen = false;
        spriteRenderer.color = originalColor;
    }
}
