using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHealthBarController : MonoBehaviour, IHealthBar
{
    [SerializeField] Transform fullHealthBar;
    [SerializeField] Transform residualHealthBar;
    [SerializeField] Transform emptyHealthBar;

    private CharacterBase characterBase;
    private Vector3 fullHealthBarMaxScale;
    private float baseHealth;
    private float residualHealth;
    private float currentHealth;

    private void Start()
    {
        fullHealthBarMaxScale = fullHealthBar.localScale;

        PositionBar();
    }

    private void PositionBar()
    {
        if(!transform.parent.TryGetComponent(out SpriteRenderer spriteRenderer))
            spriteRenderer = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
        var xPosition = -1 * transform.localScale.x / 2;
        var yPosition = spriteRenderer.bounds.size.y + 0.2f;

        transform.localPosition = new Vector2(xPosition, yPosition);
    }

    public void Setup(CharacterBase characterBase, float baseHealth)
    {
        this.characterBase = characterBase;
        this.baseHealth = baseHealth;
        currentHealth = baseHealth;
        residualHealth = currentHealth;

        characterBase.onDamageTaken += SetCurrentHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        var scale = fullHealthBar.localScale;
        var healthRatio = currentHealth / baseHealth;
        fullHealthBar.localScale = new Vector3(healthRatio * fullHealthBarMaxScale.x, scale.y, scale.z);

        if (currentHealth > 0) return;

        fullHealthBar.gameObject.SetActive(false);
        residualHealthBar.gameObject.SetActive(false);
        emptyHealthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (residualHealth <= currentHealth) return;

        Vector3 currentScale = residualHealthBar.localScale;
        Vector3 finalScale = fullHealthBar.localScale;

        residualHealthBar.localScale = Vector3.Lerp(currentScale, finalScale, 0.005f);
    }

    private void OnDisable()
    {
        characterBase.onDamageTaken -= SetCurrentHealth;
    }
}
