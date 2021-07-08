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
    [SerializeField] float arrowSpeed;
    [SerializeField] float arrowDamage;

    // TODO; create particles for fire and cold arrow trail

    public ArrowType ArrowType { get { return arrowType; } }
    public Sprite ArrowSprite { get { return arrowSprite; } }
    public float ArrowSpeed { get { return arrowSpeed; } }
    public float ArrowDamage { get { return arrowDamage; } }
}
