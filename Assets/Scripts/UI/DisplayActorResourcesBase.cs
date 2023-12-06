using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DisplayActorResourcesBase : MonoBehaviour
{
    public void OnEnable()
    {
        ActionBase.onActionTaken += OnActionTaken;
        PlayerTeam.onEntitySelected += OnEntitySelected;
    }

    public void OnDisable()
    {
        ActionBase.onActionTaken -= OnActionTaken;
        PlayerTeam.onEntitySelected -= OnEntitySelected;
    }

    public void OnActionTaken(ActingEntity actor, ActionBase action)
    {
        UpdateUI(actor);
    }

    public void OnEntitySelected(PlayerTeam team, ActingEntity actingEntity)
    {
        UpdateUI(actingEntity);
    }

    public abstract void UpdateUI(ActingEntity actor);
}
