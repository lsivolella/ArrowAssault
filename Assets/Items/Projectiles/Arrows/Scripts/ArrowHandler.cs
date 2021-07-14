using System;
using UnityEngine;

public class ArrowHandler : Collidable
{
    [SerializeField] ArrowConfigurationSO configSO;

    public ArrowConfigurationSO ConfigSO { get { return configSO; } set { configSO = value; } }

    public event Action<GameObject> onImpactOrExpiration;

    private SpriteRenderer spriteRenderer;

    private Timer timer;

    public void Setup(Vector2 position, Quaternion rotation)
    {
        spriteRenderer.sprite = configSO.ArrowSprite;
        transform.position = position;
        transform.rotation = rotation;
        timer = new Timer("Arrow Life Expectancy",
            ConfigSO.LifeExpectancy, () =>
            {
                OnInpactOrExpiration(this.gameObject);
            }, runOnce: true);
    }

    protected override void Awake()
    {
        base.Awake();
        GetComponents();
    }

    private void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //protected override void OnStart()
    //{
    //    base.OnStart();

    //    spriteRenderer.sprite = ConfigSO.ArrowSprite;
    //}

    protected override void Update()
    {
        base.Update();

        transform.Translate(Vector2.right * ConfigSO.ArrowSpeed * Time.deltaTime);
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.CompareTag("Enemies")) return;
        //if (!collider.CompareTag("Enemies") || !collider.CompareTag("Collidables")) return;

        collider.GetComponent<EnemyBase>().TakeDamage(configSO.ArrowDamage);
        OnInpactOrExpiration(this.gameObject);
    }

    private void OnInpactOrExpiration(GameObject sender)
    {
        if (onImpactOrExpiration != null) { onImpactOrExpiration(sender); }

        if (!gameObject.activeInHierarchy) return;

        gameObject.SetActive(false);
    }
}
