using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileUIController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] Image currentArrowSprite;
    [SerializeField] TextMeshProUGUI xText;
    [SerializeField] TextMeshProUGUI ammunitionText;

    private UIController uiController;
    private PlayerBase player;

    private ArrowType currentArrowType;

    private void Awake()
    {
        //uiController = GetComponent<UIController>();
        //player = uiController.Player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
        player.onProjectileUpdate += UpdateCurrentProjectile;
        player.onAmmoUpdate += UpdateAmmoCount;
    }

    public void UpdateCurrentProjectile(ArrowConfigurationSO newProjectile)
    {
        currentArrowSprite.sprite = newProjectile.ArrowSprite;
        xText.color = newProjectile.ThematicColor;
        ammunitionText.color = newProjectile.ThematicColor;
        currentArrowType = newProjectile.ArrowType;
    }

    public void UpdateAmmoCount(int amount)
    {
        if (currentArrowType.Equals(ArrowType.RegularArrow))
        {
            xText.enabled = false;
            ammunitionText.enabled = false;
        }
        else
        {
            xText.enabled = true;
            ammunitionText.enabled = true;
            ammunitionText.text = amount.ToString();
        }
            
    }

    private void OnDisable()
    {
        player.onProjectileUpdate -= UpdateCurrentProjectile;
        player.onAmmoUpdate -= UpdateAmmoCount;
    }
}
