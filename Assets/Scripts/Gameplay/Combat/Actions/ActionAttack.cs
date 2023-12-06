using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Actions/Attack")]
public class ActionAttack : ActionBase
{
    [SerializeField] private ObjectPool hexHighlightPool;

    public override ActionType GetActionType() { return ActionType.QUICK; }

    //public virtual async Task<bool> Execute(CombatManager combatManager, ActingEntity actor, object context)
    //{
    //    Entity target = (Entity)context;
//
    //    base.Execute(combatManager, actor, context);
    //}

    public virtual async Task Preview(CombatManager combatManager, ActingEntity actor, object context, CancellationToken token)
    {
        List<Entity> potentialTargets = combatManager.allEntities.objects.Values.Cast<Entity>().ToList();
        List<Entity> allies = combatManager.GetActingTeam().teamEntities.objects.Values.Cast<Entity>().ToList();
        potentialTargets = potentialTargets.Except(allies).ToList();
    }
}
