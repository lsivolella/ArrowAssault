using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player Configuration")]
public class PlayerConfigurationSO : ScriptableObject
{
    [Header("OnDamageVariables")]
    [SerializeField] float health = 5f;
    [SerializeField] AudioClip onDamageAudioClip;
    [SerializeField] [Range(0, 1)]  float onDamageClipVolume = 0.7f;
    [SerializeField] float flinchDuration = 0.6f;
    [SerializeField] float freezeDuration = 0.6f;
    [Header("Movement Variables")]
    [SerializeField] float moveSpeed = 5f;
    [Header("Arrow Types")]
    [SerializeField] ArrowConfigurationSO[] arrowTypes;

    public float Health { get { return health; } }
    public AudioClip OnDamageAudioClip { get { return onDamageAudioClip; } }
    public float OnDamageClipVolume { get { return onDamageClipVolume; } }
    public float FlinchDuration { get { return flinchDuration; } }
    public float FreezeDuration { get { return freezeDuration; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public ArrowConfigurationSO[] ArrowType { get { return arrowTypes; } }
}
