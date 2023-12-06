using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class ActionOptionsDisplayer : MonoBehaviour
{
    [SerializeField] private ObjectPool actionButtonPool;

    void OnEnable()
    {
        PlayerTeam.onEntitySelected += OnEntitySelected;
        ActionBase.onActionTaken += OnActionTaken;
        CombatManager.onNewTurn += OnNewTurn;
    }

    void OnDisable()
    {
        PlayerTeam.onEntitySelected -= OnEntitySelected;
        ActionBase.onActionTaken -= OnActionTaken;
        CombatManager.onNewTurn -= OnNewTurn;
    }

    void OnEntitySelected(PlayerTeam team, ActingEntity actingEntity)
    {
        actionButtonPool.RecallAll();

        foreach (ActionBase action in actingEntity.actionsAvaliable)
        {
            GameObject actionButton = actionButtonPool.RequestFromPool();

            actionButton.GetComponent<RectTransform>().SetParent(this.transform);            
            actionButton.GetComponent<ActionButton>().SetAction(action, team);
        }
    }

    void OnActionTaken(ActingEntity actor, ActionBase action)
    {
        // Remove actions that are no longer possible.
    }

    void OnNewTurn(Team team)
    {
        actionButtonPool.RecallAll();
    }
}
