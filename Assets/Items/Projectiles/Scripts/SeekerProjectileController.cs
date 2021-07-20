using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerProjectileController : ProjectileController
{
    protected GameObject targetObject;

    public override void Setup(Vector2 projectilePosition, Quaternion rotation, GameObject target)
    {
        transform.position = projectilePosition;
        targetObject = target;
        gameObject.SetActive(true);
        Timer.Restart();
    }

    protected override void MoveProjectile()
    {
        //transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
        transform.LookAt(targetObject.transform.position, transform.up);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);

        transform.position = Vector2.MoveTowards(transform.position, targetObject.transform.position,
            ConfigSO.ProjectileSpeed * Time.deltaTime);
    }
}
