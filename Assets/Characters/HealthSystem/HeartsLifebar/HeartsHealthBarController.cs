using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsHealthBarController : MonoBehaviour, IHealthBar
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite three_quarters_heart;
    [SerializeField] Sprite two_quarters_heart;
    [SerializeField] Sprite one_quarters_heart;
    [SerializeField] Sprite emptyHeart;

    public bool FourPartsSystem { get; set; } = false;

    private readonly List<GameObject> heartsList = new List<GameObject>();
    private CharacterBase characterBase;
    private float baseHealth;
    private float residualHealth;
    private float currentHealth;
    private float heartPieces;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Setup(CharacterBase characterBase, float baseHealth)
    {
        this.characterBase = characterBase;
        this.baseHealth = baseHealth;
        currentHealth = baseHealth;
        residualHealth = currentHealth;

        characterBase.onDamageTaken += SetCurrentHealth;
        InstantiateHearts();
    }

    private void InstantiateHearts()
    {
        if (FourPartsSystem)
            heartPieces = 4f;
        else
            heartPieces = 2f;

        float numberOfHearts = Mathf.CeilToInt(baseHealth / heartPieces);
        float remainingHealth = baseHealth;

        for (int i = 0; i < numberOfHearts; i++)
        {
            var newHeart = Instantiate(heartPrefab, transform.position, Quaternion.identity);
            heartsList.Add(newHeart);
            newHeart.transform.SetParent(this.gameObject.transform);
            newHeart.GetComponent<Image>().sprite = GetHeartSprite(remainingHealth);
            remainingHealth -= heartPieces;
            remainingHealth = Mathf.Clamp(remainingHealth, 0, remainingHealth);
        }
    }

    private Sprite GetHeartSprite(float remainingHealth)
    {
        Sprite spriteReturn;

        if (FourPartsSystem)
        {
            switch (remainingHealth)
            {
                case 0f:
                    spriteReturn = emptyHeart; break;
                case 1f:
                    spriteReturn = one_quarters_heart; break;
                case 2f:
                    spriteReturn = two_quarters_heart; break;
                case 3f:
                    spriteReturn = three_quarters_heart; break;
                default:
                    spriteReturn = fullHeart; break;
            }
        }
        else
        {
            switch (remainingHealth)
            {
                case 0f:
                    spriteReturn = emptyHeart; break;
                case 1f:
                    spriteReturn = two_quarters_heart; break;
                default:
                    spriteReturn = fullHeart; break;
            }
        }

        return spriteReturn;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float remainingHealth = currentHealth;

        foreach (GameObject heart in heartsList)
        {
            heart.GetComponent<Image>().sprite = GetHeartSprite(remainingHealth);
            remainingHealth -= heartPieces;
            remainingHealth = Mathf.Clamp(remainingHealth, 0, remainingHealth);
        }
    }

    private void OnDisable()
    {
        characterBase.onDamageTaken -= SetCurrentHealth;
    }
}
