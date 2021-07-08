using System.Collections;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    [SerializeField] EnemiesConfigurationSO configSO;

    // Sprite Color Variables
    private Color originalColor;
    private Color freezeColor = Color.blue;

    // General Variables
    public bool IsFrozen { get; private set; } = false; // TODO: check real necessity
    public bool CanMove { get; private set; } = true;
    public Vector2 MoveDirection { get; set; }
    public GameObject Target { get; private set; }

    // General Cached References
    private HealthSystem healthSystem;
    private LootDrop lootDrop;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private BoxCollider2D boxCollider;
    private Animator animator;

    // move to given position
    // take damage (-life and damage blink)
    // finder?

    protected override void OnAwake()
    {
        GetComponents();

        originalColor = spriteRenderer.color;
    }

    private void GetComponents()
    {
        healthSystem = GetComponent<HealthSystem>();
        lootDrop = GetComponent<LootDrop>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void OnStart()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        // TODO: make pubject puller inform the enemy of the player object. test performance?
        healthSystem.Setup(configSO.Health);
    }

    public void TakeDamage(float amount)
    {
        audioSource?.PlayOneShot(configSO.OnDamageAudioClip, configSO.OnDamageClipVolume);
        
        healthSystem.TakeDamage(amount);

        if (healthSystem.CurentHealth > 0) return;

        KillEnemy();
    }

    private void KillEnemy()
    {
        IsAlive = false;
        lootDrop?.GetLootDrop();
        //TODO: implement enemy object pulling
        //TODO: implement a little puff smoke
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        //Destroy(gameObject);
    }

    protected override void OnUpdate()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        var pos = transform.position;

        var rayCastX = Physics2D.BoxCast(pos, boxCollider.size / 1.5f, 0, new Vector2(MoveDirection.x, 0),
            Mathf.Abs(MoveDirection.x * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));
        var rayCastY = Physics2D.BoxCast(pos, boxCollider.size / 1.5f, 0, new Vector2(0, MoveDirection.y),
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