using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    protected bool hasBeenCollected = false;

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        OnCollect();
    }

    protected virtual void OnCollect()
    {
        hasBeenCollected = true;
        Debug.Log("I have been collected");
    }
}
