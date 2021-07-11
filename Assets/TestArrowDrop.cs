using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrowDrop : Collectable
{
    [SerializeField] ArrowConfigurationSO arrowConfig;
    [SerializeField] int amount = 10;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        spriteRenderer.sprite = arrowConfig.ArrowSprite;
    }

    protected override void OnCollect(Collider2D collider)
    {
        var player = collider.GetComponent<PlayerBase>();

        player.UpdateProjectileAndAmmo(arrowConfig, amount);

        Destroy(gameObject);
    }
}
