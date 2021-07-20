using System;
using UnityEngine;

public class Timer
{
    private class TimerMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool runOnce;

        [SerializeField]
        private float timerMax;

        [SerializeField]
        private float timer;
        public float Timer
        {
            get { return timer; }
            set { timer = value; }
        }
        private Action callback;

        private bool canRun;

        public void Setup(float amount, Action callback, bool runOnce, bool canRun = true)
        {
            timer = 0f;
            timerMax = amount;
            this.callback = callback;
            this.runOnce = runOnce;
            this.canRun = canRun;
        }

        private void Update()
        {
            if (!canRun) return;

            timer += Time.deltaTime;

            if (timer >= timerMax)
            {
                timer = 0f;
                try
                {
                    callback();
                    if (runOnce) Close();
                }
                catch (MissingReferenceException)
                {
                    Close();
                }
            }
        }

        public void Resolve()
        {
            timer = timerMax;
        }

        public void Close()
        {
            if (gameObject == null) return;
            Destroy(gameObject);
        }
    }

    private TimerMonoBehaviour timerBehaviour;
    private readonly string name;
    private float amount;
    private readonly bool runOnce;
    private readonly Action callback;
    private bool runOnRestart;

    public float Completion { get { return GetCurrentTime() / amount * 100f; } }

    public Timer(string name, float amount, Action callback, bool runOnce = false, bool runOnRestart = false)
    {
        this.name = name;
        this.amount = amount;
        this.callback = callback;
        this.runOnce = runOnce;
        this.runOnRestart = runOnRestart;

        InstantiateTimer();
    }

    public void Restart()
    {
        runOnRestart = false;
        if (timerBehaviour != null)
        {
            timerBehaviour.gameObject.SetActive(true);
            timerBehaviour.Setup(amount, callback, runOnce);
            return;
        }

        InstantiateTimer();
    }

    public void Restart(float amountTime)
    {
        amount = amountTime;
        Restart();
    }

    public void Resolve()
    {
        timerBehaviour.Resolve();
    }

    public void Stop()
    {
        timerBehaviour.gameObject.SetActive(false);
    }

    private void InstantiateTimer()
    {
        // TODO: melhorar esse código para não ficar pesquisando toda
        // a vez que um timer é instanciado,
        // o objeto ** Timers já deve ficar em cache depois da primeira vez que foi criado
        var timersRef = GameObject.Find("** Timers");
        if (timersRef == null)
        {
            timersRef = new GameObject("** Timers");
        }

        timerBehaviour = new GameObject(name).AddComponent<TimerMonoBehaviour>();
        timerBehaviour.transform.SetParent(timersRef.transform);
        timerBehaviour.Setup(amount, callback, runOnce, !runOnRestart);
    }

    public void Close()
    {
        if (timerBehaviour == null) return;
        timerBehaviour.Close();
    }

    public float GetCurrentTime()
    {
        return amount - timerBehaviour.Timer;
    }

}