using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekerProjectileScript : MonoBehaviour
{
    [Header("Projectile Damage")]
    [SerializeField] int projectileDamage = 1;

    [Header("Projectile Movement")]
    [SerializeField] float projectileSpeed = 1f;

    [Header("Projectile Duration")]
    [SerializeField] float projectileDuration = 1f;

    Transform targetPosition;
    Vector2 projectileDirection;

    void Start()
    {
        Destroy(gameObject, projectileDuration);
    }

    void Update()
    {
        FollowTargetPlayer();
    }

    private void FollowTargetPlayer()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        projectileDirection = new Vector2(targetPosition.position.x, targetPosition.position.y);
        transform.position = Vector2.MoveTowards(transform.position, projectileDirection, projectileSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
        transform.LookAt(targetPosition.position, transform.up);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);
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
