using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHandler : MonoBehaviour
{
    [System.Serializable]
    public class InstanceQuiver
    {
        public GameObject arrowPrefab;
        public int quiverSize;
    }

    [SerializeField] BowConfigurationSO configSO;
    [SerializeField] InstanceQuiver instanceQuiver;

    private readonly Queue<GameObject> arrowInstanceQuiver = new Queue<GameObject>();
    private const string ARROW_PARENT = "Arrows";
    private GameObject arrowParent;
    private PlayerBase player;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        player = GetComponentInParent<PlayerBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = configSO.BowSprite;

        CreateArrowParent();
        PopulateInstanceQuiver();
    }

    private void CreateArrowParent()
    {
        arrowParent = GameObject.Find(ARROW_PARENT);
        if (arrowParent) return;

        arrowParent = new GameObject(ARROW_PARENT);
    }

    private void PopulateInstanceQuiver()
    {
        for (int i = 0; i < instanceQuiver.quiverSize; i++)
        {
            GameObject arrow = Instantiate(instanceQuiver.arrowPrefab);
            arrow.transform.SetParent(arrowParent.transform);
            arrow.SetActive(false);
            arrowInstanceQuiver.Enqueue(arrow);
        }
    }

    private void Update()
    {
        BowRotationAndMovement();
    }

    private void BowRotationAndMovement()
    {
        // Bow rotation around its own pivot point
        var directionToCamera = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var rotationAngle = Mathf.Atan2(directionToCamera.y, directionToCamera.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        // Bow rotation around player's pivot point
        Vector3 playerToMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        playerToMouseDirection.z = 0;
        transform.position = player.transform.position + (configSO.OffsetFromPlayer * playerToMouseDirection.normalized);
    }

    public void FireArrows(ArrowConfigurationSO currentArrrowType, Transform arrowTrigger)
    {
        var arrowToFire = arrowInstanceQuiver.Dequeue().GetComponent<ArrowHandler>();
        arrowToFire.ConfigSO = currentArrrowType;
        arrowToFire.onImpact += EnqueueArrow;
        arrowToFire.Setup(arrowTrigger.position, arrowTrigger.rotation);
        arrowToFire.gameObject.SetActive(true);
        Debug.Log(arrowTrigger.rotation);
    }

    private void EnqueueArrow()
    {

    }
}
