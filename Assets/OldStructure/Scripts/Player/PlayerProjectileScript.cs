using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    [Header("Projectile Duration")]
    [SerializeField] float projectileDuration = 1f;

    [Header("Projectile Damage")]
    [SerializeField] float projectileDamage = 1f;

    [Header("Projectile Movement")]
    [SerializeField] float projectileSpeed = 1f;

    // Cached References
    BowController bowController;

    // Start is called before the first frame update
    void Start()
    {
        bowController = FindObjectOfType<BowController>();

        Destroy(gameObject, projectileDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<EnemyStandardScript>().DamageEnemy(projectileDamage);
        Destroy(gameObject);

        var currentProjectileName = bowController.GetCurrentProjectile().name;
        if (currentProjectileName == "Fire Arrow")
        {
        // Space reserved for any new properties.
        }
        else if (currentProjectileName == "Freeze Arrow")
        {
            collision.GetComponent<EnemyStandardScript>().FreezeEnemy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: delete this after testing
        collision.gameObject.GetComponent<EnemyBase>().TakeDamage(projectileDamage);
    }
}
