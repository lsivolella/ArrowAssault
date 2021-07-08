using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    [SerializeField] AudioClip arrowPickUpSound;
    [SerializeField] [Range(0, 1)] float arrowPickUpVolume = 1f;
    [SerializeField] GameObject newProjectilePrefab;
    [SerializeField] int addAmmo = 1;

    private BowController bowController;

    // Start is called before the first frame update
    void Start()
    {
        bowController = FindObjectOfType<BowController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(arrowPickUpSound, Camera.main.transform.position, arrowPickUpVolume);
            bowController.SwapProjectilePrefab(newProjectilePrefab);
            bowController.AddAmmo(addAmmo);
            bowController.SpecifyArrowPicked(newProjectilePrefab);
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }


}
