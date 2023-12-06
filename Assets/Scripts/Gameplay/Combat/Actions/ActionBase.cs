using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum ActionType
{
    QUICK,
    FULL,
    MOVE
}

public abstract class ActionBase : ScriptableObject
{
    public string name;
    public Sprite icon;

    public delegate void actionTakenDelegateHandler(ActingEntity actor, ActionBase action);
    public static actionTakenDelegateHandler onActionTaken;

    public abstract ActionType GetActionType();
    public virtual bool IsActionPossible(CombatManager combatManager, ActingEntity actor, object context) { return true; }
    public virtual bool IsActionAvaliable(CombatManager combatManager, ActingEntity actor) { return true; }

    #pragma warning disable 1998
    public virtual async Task<bool> Execute(CombatManager combatManager, ActingEntity actor, object context) { onActionTaken?.Invoke(actor, this); return true; }
    public virtual async Task Preview(CombatManager combatManager, ActingEntity actor, object context, CancellationToken token) { }
    #pragma warning restore 1998
}
