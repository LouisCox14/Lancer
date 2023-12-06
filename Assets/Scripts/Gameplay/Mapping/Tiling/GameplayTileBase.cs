using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class GameplayTileBase : GridObject
{
    [HideInInspector] public List<GameplayTileBase> neighbours;
    [HideInInspector] public Entity standingEntity;

    public bool walkable;
    public float pathfindingCostModifier = 1f;
    public int movementCostModifier = 1;
    public int height;

    public virtual void OnEntityEnter(Entity target) 
    { 
        standingEntity = target; 
        walkable = false; 

        ActingEntity actingEntity = standingEntity as ActingEntity;
        if (actingEntity != null)
        {
            actingEntity.onStartTurn += OnStartTurn;
            actingEntity.onEndTurn += OnEndTurn;
        }
    }

    public virtual void OnEntityLeave(Entity target) 
    { 
        if (standingEntity == target) 
        { 
            ActingEntity actingEntity = standingEntity as ActingEntity;
            if (actingEntity != null)
            {
                actingEntity.onStartTurn -= OnStartTurn;
                actingEntity.onEndTurn -= OnEndTurn;
            }

            standingEntity = null; 
            walkable = true; 
        } 
    }

    public virtual void OnStartTurn() { }
    public virtual void OnEndTurn() { }
    public virtual void OnStartRound() { }

    public Vector2Int GetGridPosition()
    {
        Grid grid = (Grid)gridObject.value;
        return (Vector2Int)grid.WorldToCell(transform.position);
    }

    void Start()
    {
        StoreNeighbours();

        CombatManager.onNewRound += OnStartRound;
    }

    public void StoreNeighbours()
    {
        neighbours = gridSets[0].GetNeighbours(GetGridPosition()).ConvertAll(x => (GameplayTileBase)x);
    }
}