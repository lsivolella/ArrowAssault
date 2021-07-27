using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collidable : Object
{
    protected ContactFilter2D filter;
    protected BoxCollider2D boxCollider;
    private readonly Collider2D[] hits = new Collider2D[10];
    private readonly Collider2D[] hitsMemory = new Collider2D[10];
    private readonly RaycastHit2D[] results = new RaycastHit2D[10];

    public BoxCollider2D BoxCollider { get { return boxCollider; } }

    protected virtual void OnCollideEnter(Collider2D collider) { }
    protected virtual void OnCollideStay(Collider2D collider) { }
    protected virtual void OnCollideExit(Collider2D collider) { }

    protected override void OnAwake() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void OnUpdate()
    {
        OnCollide();
    }

    private void OnCollide()
    {
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            OnExit(hits[i], i);
            if (hits[i] == null) continue;

            OnStay(hits[i], i);
            OnEnter(hits[i], i);

            hits[i] = null;
        }
    }

    private void OnStay(Collider2D collider, int index)
    {
        //Debug.Log("Collision Stay");
        OnCollideStay(collider);
    }

    private void OnEnter(Collider2D collider, int index)
    {
        if (collider == hitsMemory[index]) return;

        //Debug.Log("Collision Enter");
        hitsMemory[index] = collider;
        OnCollideEnter(collider);
    }

    private void OnExit(Collider2D collider, int index)
    {
        if (hitsMemory[index] == null) return;

        GameObject gameObject = hitsMemory[index].gameObject;

        Vector2 direction = gameObject.transform.position - transform.position;

        if (this.boxCollider.Raycast(direction, filter, results, 1f) > 0)
        {
            foreach (var objectFound in results)
            {
                if (objectFound.collider == null) continue;

                if (objectFound.collider.gameObject == gameObject)
                {
                    if (collider != null) return;

                    hitsMemory[index] = null;
                    //Debug.Log("Collision Exit");
                    OnCollideExit(collider);
                }
                    
            }
        }
    }
}
