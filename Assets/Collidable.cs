using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collidable : MonoBehaviour
{
    protected ContactFilter2D filter;
    private BoxCollider2D test;
    private Collider2D[] hits = new Collider2D[10];
    private Collider2D[] hitsMemory = new Collider2D[10];
    private RaycastHit2D[] results = new RaycastHit2D[10];

    protected virtual void OnCollideEnter(Collider2D collider) { }
    protected virtual void OnCollideStay(Collider2D collider) { }
    protected virtual void OnCollideExit(Collider2D collider) { }

    protected virtual void Awake() 
    {
        test = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        OnCollide();
    }

    private void OnCollide()
    {
        test.OverlapCollider(filter, hits);
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
        Debug.Log("Collision Stay");
        OnCollideStay(collider);
    }

    private void OnEnter(Collider2D collider, int index)
    {
        if (collider == hitsMemory[index]) return;

        Debug.Log("Collision Enter");
        hitsMemory[index] = collider;
        OnCollideEnter(collider);
    }

    private void OnExit(Collider2D collider, int index)
    {
        if (hitsMemory[index] == null) return;

        GameObject gameObject = hitsMemory[index].gameObject;

        Vector2 direction = gameObject.transform.position - transform.position;

        if (this.test.Raycast(direction, filter, results, 1f) > 0)
        {
            foreach (var objectFound in results)
            {
                if (objectFound.collider == null) continue;

                if (objectFound.collider.gameObject == gameObject)
                {
                    if (collider != null) return;

                    hitsMemory[index] = null;
                    Debug.Log("Collision Exit");
                    OnCollideExit(collider);
                }
                    
            }
        }
    }
}
