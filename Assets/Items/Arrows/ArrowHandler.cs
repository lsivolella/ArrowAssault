using System;
using UnityEngine;

public class ArrowHandler : Collidable
{
    [SerializeField] ArrowConfigurationSO configSO;

    public ArrowConfigurationSO ConfigSO { get { return configSO; } set { configSO = value; } }

    public event Action onImpact;

    private SpriteRenderer spriteRenderer;

    private Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        GetComponents();
    }

    public void Setup(Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    private void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = ConfigSO.ArrowSprite;
    }

    protected override void Update()
    {
        base.Update();

        transform.Translate(Vector2.right * ConfigSO.ArrowSpeed * Time.deltaTime);
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        OnInpact();
    }

    private void OnInpact()
    {
        if (onImpact != null) { onImpact(); }
    }
}
