using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] bool destroyBarUponDeath = false;

    public float BaseHealth { get; private set; }
    public float CurentHealth { get; private set; }
    public CharacterBase CharacterBase { get; private set; }

    private IHealthBar iHealthBar;

    private void Awake()
    {
        CharacterBase = GetComponent<CharacterBase>();
    }

    public void Setup(float baseHealth)
    {
        BaseHealth = baseHealth;
        CurentHealth = BaseHealth;

        if (!healthBar) return;

        GetHealthBarReference();
        iHealthBar.Setup(CharacterBase, baseHealth);
    }

    private void GetHealthBarReference()
    {
        if (iHealthBar == null && healthBar != null)
            iHealthBar = healthBar.GetComponent<IHealthBar>();
    }

    /// <summary>
    /// Removes health from an object instance.
    /// </summary>
    /// <param name="amount">
    /// The value of health to be removed from the object instance.
    /// Can be passed both as a negative or a positive value.
    /// </param>
    public void TakeDamage(float amount)
    {
        var newAmount = -1 * Mathf.Abs(amount);

        CurentHealth += newAmount;

        CurentHealth = Mathf.Clamp(CurentHealth, 0, BaseHealth);

        if (CurentHealth == 0 && destroyBarUponDeath)
            Destroy(healthBar);

    }
}
