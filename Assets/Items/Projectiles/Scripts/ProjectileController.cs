using System;
using UnityEngine;

public class ProjectileController : Collidable
{
    [SerializeField] ProjectileConfigurationSO configSO;

    public ProjectileConfigurationSO ConfigSO { get { return configSO; } }

    public event Action<GameObject> onImpactOrExpiration;

    private Timer timer;

    public Timer Timer { get { return timer; } }

    protected override void OnAwake()
    {
        base.OnAwake();

        timer = new Timer(gameObject.name + "'s life expectancy",
            ConfigSO.LifeExpectancy, () =>
            {
                OnImpactOrExpiration(this.gameObject);
            },
            runOnRestart: true);
    }

    public virtual void Setup(Vector2 projectilePosition, Quaternion rotation, GameObject target)
    {
        transform.position = projectilePosition;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        timer.Restart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        MoveProjectile();
    }

    protected virtual void MoveProjectile()
    {
        transform.Translate(Vector2.right * ConfigSO.ProjectileSpeed * Time.deltaTime);
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        collider.GetComponent<CharacterBase>().TakeDamage(ConfigSO.ProjectileDamage);
        OnImpactOrExpiration(this.gameObject);
    }

    public void OnImpactOrExpiration(GameObject sender)
    {
        if (onImpactOrExpiration != null) { onImpactOrExpiration(sender); }

        gameObject.SetActive(false);
    }
}
