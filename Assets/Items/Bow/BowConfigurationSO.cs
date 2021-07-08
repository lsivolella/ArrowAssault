using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bow Configuration")]
public class BowConfigurationSO : ScriptableObject
{
    [Header("General Variables")]
    [SerializeField] Sprite bowSprite;
    [SerializeField] float offsetFromPlayer = 0.5f;
    [SerializeField] AudioClip arrowShotAudioClip;
    [SerializeField] [Range(0, 1)] float arrowShotClipVolume = 0.7f;
    [SerializeField] float shotCooldown = 1f;

    public Sprite BowSprite { get { return bowSprite; } }
    public float OffsetFromPlayer { get { return offsetFromPlayer; } }
    public AudioClip ArrowShotAudioClip { get { return arrowShotAudioClip; } }
    public float ArrowShotClipVolume { get { return arrowShotClipVolume; } }
    public float ShotCooldown { get { return shotCooldown; } }
}
