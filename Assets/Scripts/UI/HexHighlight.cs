using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class HexHighlight : PoolableObject
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float pulseSpeed;

    private CancellationTokenSource pulseCancellationToken;

    void OnEnable()
    {
        base.OnEnable();

        pulseCancellationToken = new CancellationTokenSource();
        Pulse(pulseCancellationToken.Token);
    }

    void OnDisable()
    {
        pulseCancellationToken.Cancel();

        base.OnDisable();
    }

    public async Task Pulse(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            float scale = Mathf.Lerp(minScale, maxScale, Mathf.Sin(Time.time * pulseSpeed) / 2 + 0.5f);
            transform.localScale = new Vector3(scale, scale, 1);
            await Task.Yield();
        }
    }
}
