using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNonSeekerProjectileScript : MonoBehaviour
{
    [Header("Projectile Damage")]
    [SerializeField] int projectileDamage = 1;

    [Header("Projectile Movement")]
    [SerializeField] float projectileSpeed = 1f;

    Transform targetPosition;
    Vector2 projectileDirection;


    // Start is called before the first frame update
    void Start()
    {
        LockOnTargetPosition();
    }

    private void LockOnTargetPosition()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        projectileDirection = new Vector2(targetPosition.position.x, targetPosition.position.y);
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
        transform.LookAt(targetPosition.position, transform.up);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        // The projectile flies to the direction the target was when it was fired.
        transform.position = Vector2.MoveTowards(transform.position, projectileDirection, projectileSpeed * Time.deltaTime);
        // If the projectile gets to the target position it was aiming, it is destroyed.
        if (transform.position.x == projectileDirection.x && transform.position.y == projectileDirection.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.GetComponent<PlayerController>().DamagePlayer(projectileDamage);
        }
        else
        {
            return;
        }
    }
}
