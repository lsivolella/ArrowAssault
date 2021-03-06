using System;
using UnityEngine;

public abstract class CharacterBase : Collidable
{
    public bool IsAlive { get; set; } = true;

    public event Action<float> onDamageTaken;
    public event Action onDeath;

    // make event onHeartSystemUpgrade (for when the player purchase the four hearts pieces system)

    protected virtual void OnDamageTaken(float currentHealth)
    {
        if (onDamageTaken != null) { onDamageTaken(currentHealth); }
    }
}
