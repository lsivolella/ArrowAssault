using UnityEngine;

public abstract class CharacterBase : Collidable
{
    public bool IsAlive { get; set; } = true;
}
