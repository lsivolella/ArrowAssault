using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPickup : MonoBehaviour
{
    [SerializeField] AudioClip bowPickUpSound;
    [SerializeField] [Range(0, 1)] float bowPickUpVolume = 1f;

    // Cached References
    ChangeBowSprite changeSprite;
    Sprite thisSprite;

    void Start()
    {
        changeSprite = FindObjectOfType<ChangeBowSprite>();
        thisSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(bowPickUpSound, Camera.main.transform.position, bowPickUpVolume);
            changeSprite.SwapSprites(thisSprite);
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
}
