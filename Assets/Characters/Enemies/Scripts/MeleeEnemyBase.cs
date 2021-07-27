using UnityEngine;

public class MeleeEnemyBase : EnemyBase
{
    protected override void GetMovementDirection()
    {
        MoveDestination = (Target.transform.position - transform.position).normalized;
    }
}
