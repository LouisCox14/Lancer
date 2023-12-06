using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Spend Action Test")]
public class SpendActionTest : ActionBase
{
    [SerializeField] ActionType actionType;

    public override ActionType GetActionType() { return actionType; }

    public override async Task<bool> Execute(CombatManager combatManager, ActingEntity actor, object context)
    {
        actor.SpendActions(actionType, 1);
        await base.Execute(combatManager, actor, context);

        return false;
    }

    public override async Task Preview(CombatManager combatManager, ActingEntity actor, object context, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(55);
        }

        token.ThrowIfCancellationRequested();
    }
}
