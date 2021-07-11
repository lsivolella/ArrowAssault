using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        OnCollect(collider);
    }

    protected virtual void OnCollect(Collider2D collider) { }
}
