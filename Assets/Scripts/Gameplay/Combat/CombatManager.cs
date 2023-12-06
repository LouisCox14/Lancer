using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private List<Team> teams = new List<Team>();
    private int actingTeam;

    private int roundNumber;

    public GridLookupSet terrain;
    public ComponentObject gridObject;

    public GridLookupSet allEntities;

    public delegate void newRoundDelegateHandler();
    public static newRoundDelegateHandler onNewRound;

    public delegate void newTurnDelegateHandler(Team team);
    public static newTurnDelegateHandler onNewTurn;

    void Start()
    {
        foreach (Team team in teams)
        {
            team.BeginCombat();
        }
        
        actingTeam = 0;
        roundNumber = 0;

        onNewRound?.Invoke();
        onNewTurn?.Invoke(teams[actingTeam]);

        teams[actingTeam].StartTurn(this);
    }

    void OnEnable()
    {
        ActionBase.onActionTaken += OnActionTaken;
    }

    void OnDisable()
    {
        ActionBase.onActionTaken -= OnActionTaken;
    }

    private void OnActionTaken(ActingEntity actor, ActionBase action)
    {
        if (!actor.hasActed)
        {
            actor.hasActed = true;
        }
    }

    private void NewRound()
    {
        roundNumber += 1;
        onNewRound?.Invoke();

        actingTeam = roundNumber % teams.Count;
        Debug.Log("Round: " + roundNumber + "\nActing Team: " + actingTeam);
    }

    public void NewTurn()
    {
        teams[actingTeam].EndTurn();
        
        actingTeam = (actingTeam + 1) % teams.Count;
        onNewTurn?.Invoke(teams[actingTeam]);

        if (teams[actingTeam].turnsAvaliable.Count == 0)
        {
            NewRound();
        }

        teams[actingTeam].StartTurn(this);
    }

    public Team GetActingTeam()
    {
        return teams[actingTeam];
    }
}
