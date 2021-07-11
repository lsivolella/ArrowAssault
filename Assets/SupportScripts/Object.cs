using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    protected virtual void OnAwake() { }
    protected virtual void OnEnableCall() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnDisableCall() { }

    protected virtual void Awake()
    {
        OnAwake();
    }
    protected virtual void OnEnable()
    {
        OnEnableCall();
    }

    protected virtual void Start()
    {
        OnStart();
    }

    protected virtual void Update()
    {
        OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void OnDisable()
    {
        OnDisableCall();
    }
}
