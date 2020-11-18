using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBowSprite : MonoBehaviour
{
    [SerializeField] float noobBowCooldownDuration = 1f;
    [SerializeField] float goodBowCooldownDuration = 1f;
    [SerializeField] float superBowCooldownDuration = 1f;
    // Cached References
    BowController bowController;

    private void Start()
    {
        bowController = FindObjectOfType<BowController>();
    }

    public void SwapSprites(Sprite newSprite)
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
        if (newSprite.name == "Super Bow")
        {
            bowController.UpdateCooldownDuration(superBowCooldownDuration);
        }
        else if (newSprite.name == "Good Bow")
        {
            bowController.UpdateCooldownDuration(goodBowCooldownDuration);
        }
        else if (newSprite.name == "Noob Bow")
        {
            bowController.UpdateCooldownDuration(noobBowCooldownDuration);
        }
    }
}
