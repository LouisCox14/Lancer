using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Stabilise")]
public class ActionStabilise : ActionBase
{
    [SerializeField] private ObjectPool hexHighlightPool;

    public override ActionType GetActionType() { return ActionType.FULL; }

    public override async Task<bool> Execute(CombatManager combatManager, ActingEntity actor, object context)
    {
        actor.Heal(actor, actor.maxHp);
        actor.SpendActions(ActionType.FULL, 1);

        await base.Execute(combatManager, actor, context);
        return true;
    }

    public override async Task Preview(CombatManager combatManager, ActingEntity actor, object context, CancellationToken token)
    {
        HexHighlight hexHighlight = hexHighlightPool.RequestFromPool().GetComponent<HexHighlight>();
        hexHighlight.transform.position = actor.transform.position;

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(25);
        }

        token.ThrowIfCancellationRequested();
    }
}
