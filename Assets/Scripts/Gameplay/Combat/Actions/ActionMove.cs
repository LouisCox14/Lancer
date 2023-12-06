using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Actions/Move")]
public class ActionMove : ActionBase
{
    [SerializeField] private ObjectPool pathPreviewPool;

    public override ActionType GetActionType() { return ActionType.MOVE; }

    public override async Task<bool> Execute(CombatManager combatManager, ActingEntity actor, object context)
    {
        Vector2Int targetPos = (Vector2Int)context;
        List<GameplayTileBase> path = Pathfinder.FindPath((GameplayTileBase)combatManager.terrain.GetObjectAtGridPos(actor.position), (GameplayTileBase)combatManager.terrain.GetObjectAtGridPos(targetPos));

        int moveSpeed = actor.actionsRemaining[ActionType.MOVE];
        if (path.Count > moveSpeed + 1)
        {
            path = path.GetRange(0, moveSpeed + 1);
        }

        await actor.FollowPath(path);
        actor.SpendActions(ActionType.MOVE, path.Count - 1);
        await base.Execute(combatManager, actor, context);

        return false;
    }

    public override async Task Preview(CombatManager combatManager, ActingEntity actor, object context, CancellationToken token)
    {
        PlayerTeam playerTeam = (PlayerTeam)context;
        Grid grid = (Grid)combatManager.gridObject.value;
        PathPreview pathPreview = pathPreviewPool.RequestFromPool().GetComponent<PathPreview>();
        pathPreview.transform.position = actor.transform.position;

        Vector2Int targetPos = new Vector2Int(999, 999);
        CancellationTokenSource pathDrawCancellationToken = new CancellationTokenSource();

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(25);

            if (targetPos == (Vector2Int)grid.WorldToCell(Camera.main.ScreenToWorldPoint(playerTeam.mousePos.ReadValue<Vector2>())))
            {           
                continue;     
            }

            targetPos = (Vector2Int)grid.WorldToCell(Camera.main.ScreenToWorldPoint(playerTeam.mousePos.ReadValue<Vector2>()));

            if (!combatManager.terrain.GetObjectAtGridPos(targetPos))
            {
                continue;
            }

            List<GameplayTileBase> path = Pathfinder.FindPath((GameplayTileBase)combatManager.terrain.GetObjectAtGridPos(actor.position), (GameplayTileBase)combatManager.terrain.GetObjectAtGridPos(targetPos));

            List<Vector2Int> points = new List<Vector2Int>();
            for (int i = 0; i <= Mathf.Min(actor.actionsRemaining[ActionType.MOVE], path.Count - 1); i++)
            {
                points.Add(path[i].position);
            }

            pathDrawCancellationToken.Cancel();
            pathDrawCancellationToken = new CancellationTokenSource();
            pathPreview.SetPath(points, pathDrawCancellationToken.Token);
        }

        pathPreview.gameObject.SetActive(false);
    }
}
