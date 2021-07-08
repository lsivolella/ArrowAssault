using UnityEngine;

public class MeleeEnemyBase : EnemyBase
{
    protected override void OnUpdate()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        MoveDirection = (Target.transform.position - transform.position).normalized;
        base.OnUpdate();
    }
}
