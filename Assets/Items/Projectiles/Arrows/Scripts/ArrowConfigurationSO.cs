using UnityEngine;

public enum ArrowType
{
    RegularArrow,
    FireArrow,
    FreezeArrow
}

[CreateAssetMenu(menuName = "Items/Arrow Configuration")]
public class ArrowConfigurationSO : ScriptableObject
{
    [Header("General Variables")]
    [SerializeField] ArrowType arrowType;
    [SerializeField] Sprite arrowSprite;
    [SerializeField] float arrowSpeed = 3f;
    [SerializeField] float arrowDamage = 1f;
    [SerializeField] float lifeExpectancy = 2f;
    [SerializeField] Color thematicColor;

    // TODO; create particles for fire and cold arrow trail

    public ArrowType ArrowType { get { return arrowType; } }
    public Sprite ArrowSprite { get { return arrowSprite; } }
    public float ArrowSpeed { get { return arrowSpeed; } }
    public float ArrowDamage { get { return arrowDamage; } }
    public float LifeExpectancy { get { return lifeExpectancy; } }
    public Color ThematicColor { get { return thematicColor; } }
}
