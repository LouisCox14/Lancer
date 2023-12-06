using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActingEntity : Entity
{
    public Dictionary<ActionType, int> actionsRemaining = new Dictionary<ActionType, int>
    {
        { ActionType.FULL, 0 },
        { ActionType.QUICK, 0 },
        { ActionType.MOVE, 0 }
    };

    public int speed;

    public List<ActionBase> actionsAvaliable;
    [HideInInspector] public bool hasActed;

    public delegate void turnDelegateHandler();
    public turnDelegateHandler onStartTurn;
    public turnDelegateHandler onEndTurn;

    void OnEnable()
    {
        base.OnEnable();

        CombatManager.onNewRound += OnRoundStart;
    }

    void OnDisable()
    {
        base.OnDisable();
        
        CombatManager.onNewRound -= OnRoundStart;
    }

    void OnRoundStart()
    {
        actionsRemaining[ActionType.FULL] = 1;
        actionsRemaining[ActionType.MOVE] = speed;

        hasActed = false;
    }

    public void StartTurn()
    {
        onStartTurn?.Invoke();
    }

    public void EndTurn()
    {
        onEndTurn?.Invoke();
    }

    public bool SpendActions(ActionType actionType, int amount)
    {
        if (!IsActionAvaliable(actionType, amount))
        {
            if (IsActionAvaliable(actionType, amount, true))
            {
                actionsRemaining[ActionType.FULL] -= 1;
                actionsRemaining[ActionType.QUICK] += 2;
            }
            else
            {
                return false;
            }
        }

        actionsRemaining[actionType] -= amount;
        return true;
    }

    public bool IsActionAvaliable(ActionType actionType, int amount, bool quickCanUseFull = false)
    {
        if (!actionsRemaining.ContainsKey(actionType))
        {
            return false;
        }

        if (actionsRemaining[actionType] < amount)
        {
            if (quickCanUseFull && actionType == ActionType.QUICK && IsActionAvaliable(ActionType.FULL, 1))
            {
                return true;
            }

            return false;
        }

        return true;
    }
}
