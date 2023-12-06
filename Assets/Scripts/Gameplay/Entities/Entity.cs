using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ReturnEvents;

public abstract class Entity : GridObject
{
    public int size;

    [field: SerializeField] public int maxHp { get; private set; }
    public int hp { get; private set; }

    public int evasion;
    public int armour;

    [SerializeField] private AnimationCurve stepCurve;

    public delegate void damageTakenDelegateHandler(int maxHP, int newHP, Damage damageTaken);
    public damageTakenDelegateHandler onDamageTaken;

    public delegate void healthGainedDelegateHandler(int maxHP, int newHP, int healthGained);
    public healthGainedDelegateHandler onHealthGained;

    public delegate void entityDeathDelegateHandler(Entity entity);
    public static entityDeathDelegateHandler onEntityDeath;

    ReturnEvent<int> getDamageModifiers = new ReturnEvent<int>();

    void Awake()
    {
        hp = maxHp;
    }

    public void TakeDamage(Component causer, Damage damageData)
    {
        foreach (int returnValue in getDamageModifiers.Invoke(this, damageData))
        {
            damageData.amount = Mathf.Max(damageData.amount + returnValue, 0);
        }

        hp -= damageData.amount;
        onDamageTaken?.Invoke(maxHp, hp, damageData);

        if (hp <= 0)
        {
            // TODO: Add structure logic.
            onEntityDeath?.Invoke(this);
        }
    }

    public void Heal(Component causer, int healthGaned)
    {
        healthGaned = Mathf.Min(healthGaned, maxHp - hp);

        hp += healthGaned;
        onHealthGained?.Invoke(maxHp, hp, healthGaned);
    }

    public async Task FollowPath(List<GameplayTileBase> path)
    {
        GameplayTileBase lastTile = path[0];

        for (int i = 1; i < path.Count; i++)
        {
            lastTile.OnEntityLeave(this);

            await LerpPosition(path[i].transform.position);
            SetPosition(path[i].position);

            path[i].OnEntityEnter(this);
            lastTile = path[i];
        }
    }

    private async Task LerpPosition(Vector2 targetPos)
    {
        float timer = 0;
        Vector2 startPos = (Vector2)transform.position;

        while (stepCurve[stepCurve.length-1].time > timer)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, targetPos, stepCurve.Evaluate(timer));
            await Task.Yield();
        }

        transform.position = targetPos;
    }
}
