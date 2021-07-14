using UnityEngine;

public class MeleeEnemyBase : EnemyBase
{
    protected override void OnUpdate()
    {
        if (!IsAlive) return;

        if (!CanMove) return;

        if (IsFrozen) return;

        if (!Target) return;

        GetMovementDirection();
        base.OnUpdate();
        //TODO: try to move base to the top
    }

    private void GetMovementDirection()
    {
        MoveDirection = (Target.transform.position - transform.position).normalized;
    }
}
