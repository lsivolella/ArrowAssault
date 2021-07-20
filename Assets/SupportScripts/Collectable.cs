using System;
using UnityEngine;

public class Collectable : Collidable
{
    public event Action onCollected;

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        OnCollect(collider);
    }

    protected virtual void OnCollect(Collider2D collider) { }

    protected void OnCollected()
    {
        if (onCollected != null) { onCollected(); }
    }

    public void ActivateItem(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

    public void DeactivateItem()
    {
        gameObject.SetActive(false);
    }
}
