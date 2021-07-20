using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartLootController : Collectable
{
    [SerializeField] float amount = 1;

    protected override void OnCollect(Collider2D collider)
    {
        collider.GetComponent<HealthSystem>().Heal(amount);
        OnCollected();
        DeactivateItem();
    }
}
