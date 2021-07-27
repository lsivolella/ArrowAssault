using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Super Treant Configuration")]
public class SuperTreantConfigurationSO : ScriptableObject
{
    [SerializeField] int numberOfProjectiles = 8;
    [SerializeField] float rageModeTrigger = 0.3f;
    [SerializeField] float rageModeMoveSpeedFactor = 0.3f;
    [SerializeField] List<Vector2> movementWaypoints;

    public int NumberOfProjectiles { get { return numberOfProjectiles; } }
    public float RageModeTrigger { get { return rageModeTrigger; } }
    public float RangeModeMoveSpeedFactor { get { return rageModeMoveSpeedFactor; } }
    public List<Vector2> MovementWaypoints { get { return movementWaypoints; } }
}
