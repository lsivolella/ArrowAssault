using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    [Header("Projectile Duration")]
    [SerializeField] float projectileDuration = 1f;

    [Header("Projectile Damage")]
    [SerializeField] int projectileDamage = 1;

    [Header("Projectile Movement")]
    [SerializeField] float projectileSpeed = 1f;
    
    Vector2 movementDirection;
    bool hasDirection;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, projectileDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDirection)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementDirection, projectileSpeed * Time.deltaTime);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        hasDirection = true;
        movementDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().DamagePlayer(projectileDamage);
            Destroy(gameObject);
        }
    }
}
