using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartLoot : MonoBehaviour
{
    [SerializeField] AudioClip heartSound;
    [SerializeField] [Range(0, 1)] float heartSoundVolume = 1f;
    [SerializeField] int healthRecover = 1;

    // Cached References
    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(heartSound, Camera.main.transform.position, heartSoundVolume);
            playerController.RecoverHealth(healthRecover);
            Debug.Log("Heart used.");
            Destroy(gameObject);
        }
        else
        { 
            return;
        }
    }
}
