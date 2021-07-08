using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    [SerializeField] PlayerConfigurationSO configSO;
    [SerializeField] Transform arrowTrigger;

    // General Cached References
    private HealthSystem healthSystem;
    private LootDrop lootDrop;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private BowHandler bowHandler;

    private Vector2 movement;
    private ArrowConfigurationSO currentArrowType;

    protected override void OnAwake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        healthSystem = GetComponent<HealthSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        bowHandler = GetComponentInChildren<BowHandler>();
    }

    protected override void OnStart()
    {
        healthSystem.Setup(configSO.Health);

        currentArrowType = configSO.ArrowType[0];
    }

    protected override void OnUpdate()
    {
        ProcessMovementInput();
        ProcessFireInput();
    }

    private void ProcessMovementInput()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        var pos = transform.position;

        var rayCastX = Physics2D.BoxCast(pos, boxCollider.size / 1.5f, 0, new Vector2(movement.x, 0),
            Mathf.Abs(movement.x * configSO.MoveSpeed * Time.deltaTime), LayerMask.GetMask("Characters", "Interactables"));
        var rayCastY = Physics2D.BoxCast(pos, boxCollider.size / 1.5f, 0, new Vector2(0, movement.y),
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
        if (!Input.GetButtonDown("Fire1")) return;

        bowHandler.FireArrows(currentArrowType, arrowTrigger);
        Debug.Log("Pew pew!");

    }

    public void TakeDamage(float amount)
    {
        audioSource?.PlayOneShot(configSO.OnDamageAudioClip, configSO.OnDamageClipVolume);
        
        healthSystem.TakeDamage(amount);

        if (healthSystem.CurentHealth > 0) return;

        KillPlayer();
    }

    private void KillPlayer()
    {

    }
}
