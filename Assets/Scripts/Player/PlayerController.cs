using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] float playerMoveSpeed = 1f;

    private bool facingRight = true;
    private Rigidbody2D playerRigidbody;
    private Vector2 movementDirection;

    [Header("Player Health")]
    [SerializeField] AudioClip damageSound;
    [SerializeField] [Range(0, 1)] float damageSoundVolume = 1f;
    [SerializeField] Image[] heartsImages;
    [SerializeField] Sprite[] heartsSprites;
    [SerializeField] int startingHeartAmount = 1; // Number of Hearts the player starts the game with. In this case, the player starts with access to all Hearts.
    [SerializeField] float playerRecoveryTime = 1f;

    private Color nativeColor;
    private bool inRecovery;
    private float recoveryCounter = 0f;
    private int playerCurrentHealth;
    private int playerMaxHealth; // Maximum amounts of Hearts available to the player. Ex) In Zelda games, new Hearts can be found and added to the maximum.
    private int maxHeartAmount = 5;
    private int healthPerHeart = 2;

    [Header("Player Damage")]
    private BowController bowController;

    [Header("Others")]
    [SerializeField] LevelManager levelManager;
    
    // Start is called before the first frame update
    void Start()
    {
        bowController = FindObjectOfType<BowController>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        nativeColor = GetComponent<SpriteRenderer>().color;

        SetUpPlayerHealth();
        CheckHealthAmount();
    }

    private void SetUpPlayerHealth()
    {
        playerCurrentHealth = startingHeartAmount * healthPerHeart;
        playerMaxHealth = maxHeartAmount * healthPerHeart;
    }

    // Update is called once per frame
    void Update()
    {
        MovementDetection();

        FireDetection();

        SetRecoveryTime();
    }

    private void SetRecoveryTime()
    {
        if (inRecovery)
        {
            GetComponent<SpriteRenderer>().color = Color.green;

            recoveryCounter += Time.deltaTime;

            if (recoveryCounter >= playerRecoveryTime)
            {
                inRecovery = false;
                recoveryCounter = 0;
                GetComponent<SpriteRenderer>().color = nativeColor;
            }
        }
    }

    private void FireDetection()
    {
        if (Input.GetButton("Fire1"))
        {
            bowController.ShotProjectile();
        }
    }

    public void MovementDetection()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");

        //Vector2 playerPosition = playerRigidbody.position;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }

        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        if (dir.x > 0 && !facingRight || dir.x < 0 && facingRight)
        {
            FlipPlayerSprite();
        }

    }

    private void FlipPlayerSprite()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + (movementDirection * playerMoveSpeed * Time.fixedDeltaTime));
    }

    public void DamagePlayer(int incomingDamage)
    {
        if(gameObject)
        {
            if (!inRecovery)
            {
                AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position, damageSoundVolume);
                inRecovery = true;
                playerCurrentHealth -= incomingDamage;
                playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth);
                //UpdateHealthUI(playerCurrentHealth);
                UpdateHealthUI();

                if (playerCurrentHealth <= 0)
                {
                    Destroy(gameObject);
                    levelManager.GameOver();
                }
            }
        }
    }

    public void UpdateHealthUI()
    {
        bool empty = false;
        int i = 0;

        foreach (Image image in heartsImages)
        {
            if (empty)
            {
                image.sprite = heartsSprites[0];
            }
            else
            {
                i++;
                if (playerCurrentHealth >= i * healthPerHeart)
                {
                    image.sprite = heartsSprites[heartsSprites.Length - 1];
                }
                else
                {
                    int currentHeartHealth = (int)(healthPerHeart - (healthPerHeart * i - playerCurrentHealth));
                    int healthPerImage = healthPerHeart / (heartsSprites.Length - 1);
                    int imageIndex = currentHeartHealth / healthPerImage;
                    image.sprite = heartsSprites[imageIndex];
                    empty = true;
                }
            }
        }
    }

    public void CheckHealthAmount() // Deactivate any Heart image that is not being used. Ex) Game allows up to 5 Hearts, but player has only 4 of them available.
    {
        for (int i = 0; i < maxHeartAmount; i++)
        {
            if (startingHeartAmount <= i)
            {
                heartsImages[i].enabled = false;
            }
            else
            {
                heartsImages[i].enabled = true;
            }
        }
        UpdateHealthUI();
    }

    public void RecoverHealth(int healthRecover)
    {
        playerCurrentHealth += healthRecover;
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth);
        //UpdateHealthUI(playerCurrentHealth);
        UpdateHealthUI();
    }


    // Code for pausing the game from: https://answers.unity.com/questions/728949/difference-between-inputgetaxis-and-getaxisraw.html#:~:text=2%20Replies&text=Yes%20%2D%20your%20understanding%20is%20correct,a%20keyboard%20or%20joystick%20button).
    /*
        if (Input.GetAxisRaw ("Pause")> 0) 
        { 
            if (Inp == 0) 
                { if (Pause == false) 
                    { 
                        Pause = true; 
                    } 
                    else 
                    { 
                        Pause = false; 
                    } 
                } 
            Inp = 1;
        } Else { Inp = 0; } Debug.log (InP);

        if (Pause) {

        Time.timeScale = 0; ObjCanvasPause.SetActive (true); // Inp = 0; } Else {
        ObjCanvasPause.SetActive (false); Time.timeScale = 1; }
        */
}
