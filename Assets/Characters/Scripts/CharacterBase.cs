using UnityEngine;

public abstract class CharacterBase : Collidable
{
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }

    public bool IsAlive { get; set; } = true;

    protected override void Awake()
    {
        base.Awake();
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    protected override void Update()
    {
        base.Update();
        OnUpdate();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
}
