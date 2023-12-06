using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Team : ScriptableObject
{
    public string teamName;
    public GridLookupSet teamEntities;

    public CombatManager combatManager { get; private set; }

    [HideInInspector] public ActingEntity actingEntity;
    public List<ActingEntity> turnsAvaliable { get; private set; }

    public void BeginCombat()
    {
        CombatManager.onNewRound += NewRound;
    }

    public virtual void NewRound()
    {
        turnsAvaliable = teamEntities.objects.Values.Cast<ActingEntity>().ToList();
    } 

    public virtual void StartTurn(CombatManager _combatManager)
    {
        combatManager = _combatManager;
    }

    public virtual void EndTurn()
    {
        actingEntity.EndTurn();
        turnsAvaliable.Remove(actingEntity);
        actingEntity = null;
    }

    private bool GetHasActed(GridObject gridObj)
    {
        ActingEntity actor = gridObj as ActingEntity;

        if (actor)
        {
            return actor.hasActed;
        }

        return false;
    }
}
