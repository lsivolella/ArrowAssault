using System;
using UnityEngine;

public class ProjectileController : Collidable
{
    [SerializeField] ProjectileConfigurationSO configSO;

    public ProjectileConfigurationSO ConfigSO { get { return configSO; } }

    public event Action<GameObject> onImpactOrExpiration;

    private Timer timer;
    private GameObject target;
    private Vector2 targetInitialPosition; // used for non-seeker projectiles

    public void Setup(Vector2 position, Quaternion rotation, GameObject target)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.target = target;
        targetInitialPosition = target.transform.position;
        timer = new Timer(gameObject.name + "'s life expectancy",
            ConfigSO.LifeExpectancy, () =>
            {
                OnInpactOrExpiration(this.gameObject);
            }, runOnce: true);
    }

    public void OnInpactOrExpiration(GameObject sender)
    {
        if (onImpactOrExpiration != null) { onImpactOrExpiration(sender); }
    }

    protected override void Update()
    {
        base.Update();

        if (target == null)
            transform.position = Vector2.MoveTowards(transform.position, targetInitialPosition,
                ConfigSO.ProjectileSpeed * Time.deltaTime);
        else
        {
            transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
            transform.LookAt(target.transform.position, transform.up);
            transform.Rotate(new Vector3(0, 90, 0), Space.Self);

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 
                ConfigSO.ProjectileSpeed * Time.deltaTime);
        }
    }

    protected override void OnCollideEnter(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        OnInpactOrExpiration(this.gameObject);
    }
}
