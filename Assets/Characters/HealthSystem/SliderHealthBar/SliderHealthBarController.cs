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

        var xPosition = -1 * transform.localScale.x / 2;

        transform.localPosition = new Vector3(xPosition, transform.localPosition.y, transform.localPosition.z);
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
