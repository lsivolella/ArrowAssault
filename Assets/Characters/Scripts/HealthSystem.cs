using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float BaseHealth { get; private set; }
    public float CurentHealth { get; private set; }

    public void Setup(float baseHealth)
    {
        BaseHealth = baseHealth;
        CurentHealth = BaseHealth;
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
    }
}
