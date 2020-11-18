using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowController : MonoBehaviour
{
    [Header ("Weapon Movement and Rotation.")]
    [SerializeField] Transform player;
    [SerializeField] float offset = 1f; // transform the offset into the x offset the child has from the parent (bow from player).

    [Header("Fire projectile.")]
    [SerializeField] AudioClip projectileSound;
    [SerializeField] [Range(0, 1)] float projectileSoundVolume = 1f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform arrowTrigger;
    [SerializeField] float cooldownDuration = 1f;

    private bool canShoot = true;
    private GameObject projectileParent;
    private const string PROJECTILE_PARENT_NAME = "Projectiles";

    [Header("Ammunition.")]
    [SerializeField] GameObject regularProjectilePrefab;
    [SerializeField] Image uiProjectile;
    [SerializeField] Text xText;
    [SerializeField] Text uiAmmoText;
    [SerializeField] Sprite regularArrow;
    [SerializeField] Sprite fireArrow;
    [SerializeField] Sprite freezeArrow;

    private int currentAmmo;
    private Sprite currentProjectile;
    private GameObject testProjectilePrefab;
   
    // Start is called before the first frame update
    void Start()
    {
        CreateProjectileParent();
        GetCurrentProjectile();
        UpdateProjectileUI(currentProjectile);
        UpdateAmmoUI(currentAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        BowRotationAndMovement();
    }

    private void BowRotationAndMovement()
    {
        // Bow Rotation around its own pivot point.
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Bow Rotation around the player's pivot point.
        Vector3 playerToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        playerToMouseDir.z = 0;
        transform.position = player.position + (offset * playerToMouseDir.normalized);
    }

    private void CreateProjectileParent()
    {
        projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);
        if (!projectileParent)
        {
            projectileParent = new GameObject(PROJECTILE_PARENT_NAME);
        }
    }

    public void ShotProjectile()
    {
        if(canShoot)
        {
            AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileSoundVolume);
            if (currentProjectile == regularArrow)
            {
                GameObject arrow = Instantiate(projectilePrefab, arrowTrigger.position, arrowTrigger.rotation) as GameObject;
                arrow.transform.parent = projectileParent.transform;

                StartCoroutine(ApplyCooldown());
            }
            else if (currentAmmo > 0)
            {
                GameObject arrow = Instantiate(projectilePrefab, arrowTrigger.position, arrowTrigger.rotation) as GameObject;
                arrow.transform.parent = projectileParent.transform;

                StartCoroutine(ApplyCooldown());

                SpendAmmo();
  
            }
        }
    }

    public void SpendAmmo()
    {
        currentAmmo -= 1;

        UpdateAmmoUI(currentAmmo);

        if (currentAmmo <= 0)
        {
            currentProjectile = regularArrow;
            UpdateProjectileUI(currentProjectile);
            SwapProjectilePrefab(regularProjectilePrefab);
            UpdateAmmoUI(currentAmmo);
        }
    }

    IEnumerator ApplyCooldown()
    {
        canShoot = false;

        yield return new WaitForSeconds(cooldownDuration);

        canShoot = true;
    }

    public void UpdateCooldownDuration(float newCooldownDuration)
    {
        cooldownDuration = newCooldownDuration;
    }

    public Sprite GetCurrentProjectile()
    {
        string currentProjectileName = projectilePrefab.name;

        if (currentProjectileName == "Regular Arrow")
        {
            currentProjectile = regularArrow;
        }
        else if (currentProjectileName == "Fire Arrow")
        {
            currentProjectile = fireArrow;
        }
        else if (currentProjectileName == "Freeze Arrow")
        {
            currentProjectile = freezeArrow;
        }
        return currentProjectile;
    }

    public void UpdateProjectileUI(Sprite currentProjectile)
    {
        uiProjectile.sprite = currentProjectile;
    }

    public void SwapProjectilePrefab(GameObject newProjectilePrefab)
    {
        projectilePrefab = newProjectilePrefab;
        GetCurrentProjectile();
        UpdateProjectileUI(currentProjectile);

    }

    public void AddAmmo(int addAmmo)
    {
        UpdateAmmoCount(addAmmo);
        UpdateAmmoUI(currentAmmo);
    }

    public int UpdateAmmoCount(int addAmmo)
    {
        if (testProjectilePrefab == projectilePrefab)
        {
            currentAmmo += addAmmo;
        }
        else
        {
            currentAmmo = addAmmo;
        }
        return currentAmmo;
    }

    public GameObject SpecifyArrowPicked(GameObject newProjectilePrefab)
    {
        testProjectilePrefab = newProjectilePrefab;
        return testProjectilePrefab;
    }

    public void UpdateAmmoUI(int currentAmmo)
    {
        if (currentProjectile == regularArrow)
        {
            xText.GetComponent<Text>().enabled = false;
            uiAmmoText.GetComponent<Text>().enabled = false;
        }
        else if (currentProjectile == fireArrow)
        {
            xText.GetComponent<Text>().enabled = true;
            uiAmmoText.GetComponent<Text>().enabled = true;
            xText.GetComponent<Text>().color = Color.red;
            uiAmmoText.GetComponent<Text>().text = currentAmmo.ToString();
            uiAmmoText.GetComponent<Text>().fontSize = 50;
            uiAmmoText.GetComponent<Text>().color = Color.red;
        }
        else if (currentProjectile == freezeArrow)
        {
            xText.GetComponent<Text>().enabled = true;
            uiAmmoText.GetComponent<Text>().enabled = true;
            xText.GetComponent<Text>().color = Color.blue;
            uiAmmoText.GetComponent<Text>().text = currentAmmo.ToString();
            uiAmmoText.GetComponent<Text>().fontSize = 50;
            uiAmmoText.GetComponent<Text>().color = Color.blue;
        }
    }
}
