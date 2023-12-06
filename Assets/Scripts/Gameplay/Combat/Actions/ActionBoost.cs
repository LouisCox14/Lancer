using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Boost")]
public class ActionBoost : ActionBase
{
    [SerializeField] private ObjectPool hexHighlightPool;

    public override ActionType GetActionType() { return ActionType.QUICK; }

    public override async Task<bool> Execute(CombatManager combatManager, ActingEntity actor, object context)
    {
        actor.actionsRemaining[ActionType.MOVE] += actor.speed;
        actor.SpendActions(ActionType.QUICK, 1);

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

        hexHighlight.gameObject.SetActive(false);
    }
}
