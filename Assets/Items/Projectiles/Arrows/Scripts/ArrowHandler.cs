using System;
using UnityEngine;

public class ArrowHandler : ProjectileController
{
    [SerializeField] ArrowConfigurationSO arrowConfigSO;

    public ArrowConfigurationSO ArrowConfigSO { get { return arrowConfigSO; } set { arrowConfigSO = value; } }

    private SpriteRenderer spriteRenderer;

    protected override void OnAwake()
    {
        base.OnAwake();

        GetComponents();
    }

    private void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Setup(Vector2 projectilePosition, Quaternion rotation, GameObject target)
    {
        base.Setup(projectilePosition, rotation, target);
        spriteRenderer.sprite = arrowConfigSO.ArrowSprite;
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.CompareTag("Enemies")) return;
        //if (!collider.CompareTag("Enemies") || !collider.CompareTag("Collidables")) return;
        // TODO: filter for enemies and collidables

        collider.GetComponent<CharacterBase>().TakeDamage(ConfigSO.ProjectileDamage);
        OnImpactOrExpiration(this.gameObject);
    }
}
