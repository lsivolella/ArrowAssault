using System;
using UnityEngine;

public class TargetedProjectileController : ProjectileController
{
    protected Vector2 targetPosition;

    public override void Setup(Vector2 projectilePosition, Quaternion rotation, GameObject target)
    {
        transform.position = projectilePosition;
        targetPosition = target.transform.position;
        gameObject.SetActive(true);
        RotateRelativeTotarget();
        Timer.Stop();
    }

    private void RotateRelativeTotarget()
    {
        transform.LookAt(targetPosition, transform.up);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);
    }

    protected override void MoveProjectile()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition,
            ConfigSO.ProjectileSpeed * Time.deltaTime);

        if ((Vector2)transform.position == targetPosition)
            OnImpactOrExpiration(this.gameObject);
    }
}
