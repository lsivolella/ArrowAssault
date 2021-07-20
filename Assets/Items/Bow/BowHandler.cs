using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHandler : MonoBehaviour
{
    [SerializeField] BowConfigurationSO configSO;

    public BowConfigurationSO ConfigSO { get { return configSO; } }

    private PlayerBase player;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        player = GetComponentInParent<PlayerBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = ConfigSO.BowSprite;
    }

    private void Update()
    {
        BowRotationAndMovement();
    }

    private void BowRotationAndMovement()
    {
        // Bow rotation around its own pivot point
        var directionToCamera = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var rotationAngle = Mathf.Atan2(directionToCamera.y, directionToCamera.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        // Bow rotation around player's pivot point
        Vector3 playerToMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        playerToMouseDirection.z = 0;
        transform.position = player.transform.position + (ConfigSO.OffsetFromPlayer * playerToMouseDirection.normalized);
    }
}
