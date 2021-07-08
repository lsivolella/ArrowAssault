using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandardScript : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] bool canMove = true;
    [SerializeField] float freezeMovementTime = 1f;
    [SerializeField] float freezeEnemyTime = 1f;

    private Color nativeColor;
    private Color freezeColor;
    private bool arrowFrozen;

    [Header("Enemy Health")]
    [SerializeField] AudioClip damageSound;
    [SerializeField] [Range(0, 1)] float damageSoundVolume = 1f;
    [SerializeField] float enemyHealth = 1f;

    [Header("Enemy Damage on Contact")]
    [SerializeField] Transform targetPlayer;
    [SerializeField] int enemyDamage = 1;

    // General Cached References
    LootDrop lootDrop;

    void Start()
    {
        lootDrop = GetComponent<LootDrop>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        nativeColor = GetComponent<SpriteRenderer>().color;
        freezeColor = Color.blue;
    }

    public void DamageEnemy(float incomingDamage)
    {
        // refactor done
        AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position, damageSoundVolume);
        enemyHealth -= incomingDamage;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
            lootDrop.GetLootDrop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !arrowFrozen)
        {
            Debug.Log("Enter Trigger");
            targetPlayer.GetComponent<PlayerController>().DamagePlayer(enemyDamage);
            StartCoroutine(FreezeMovement());
        }
        else if (collision.CompareTag("Player"))
        {
            targetPlayer.GetComponent<PlayerController>().DamagePlayer(enemyDamage);
        }
    }

    IEnumerator FreezeMovement()
    {
        canMove = false;

        yield return new WaitForSeconds(freezeMovementTime);

        canMove = true;
    }

    public void FreezeEnemy()
    {
        StartCoroutine(FreezeEnemyCoroutine());
    }

    IEnumerator FreezeEnemyCoroutine()
    {
        this.arrowFrozen = true;
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = false;
        }
        GetComponent<SpriteRenderer>().color = freezeColor;
        this.canMove = false;
        yield return new WaitForSeconds(freezeEnemyTime);
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = true;
        }
        this.canMove = true;
        GetComponent<SpriteRenderer>().color = nativeColor;
        this.arrowFrozen = false;
    }

    public bool CanEnemyMove()
    {
        return canMove;
    }

    public float GetEnemyHealth()
    {
        return enemyHealth;
    }
}
