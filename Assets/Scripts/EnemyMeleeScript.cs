using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] float enemyMoveSpeed = 1f;

    private bool canMove;

    [Header("Enemy Damage")]
    [SerializeField] Transform targetPlayer;

    // Cached References
    EnemyStandardScript standardEnemyScript;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        standardEnemyScript = GetComponent<EnemyStandardScript>();
    }

    
    // Update is called once per frame
    void Update()
    {
        ControllEnemyMovement();
    }

    private void ControllEnemyMovement()
    {
        canMove = standardEnemyScript.CanEnemyMove();

        if (targetPlayer && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, enemyMoveSpeed * Time.deltaTime);
        }
    }
}
