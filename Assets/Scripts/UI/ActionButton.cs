using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButton : PoolableObject
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    private ActionBase currentAction;
    private PlayerTeam currentTeam;

    public void SetAction(ActionBase newAction, PlayerTeam newTeam)
    {
        currentAction = newAction;
        currentTeam = newTeam;

        iconImage.sprite = currentAction.icon;
        nameText.text = currentAction.name;
    }

    void OnEnable()
    {
        base.OnEnable();

        ActionBase.onActionTaken += OnActionTaken;
    }

    void OnDisable()
    {
        base.OnDisable();

        ActionBase.onActionTaken -= OnActionTaken;
    }

    public void OnButtonClicked()
    {
        currentTeam.SetSelectedAction(currentAction);
    }

    private void OnActionTaken(ActingEntity actor, ActionBase action)
    {
        if (!actor.IsActionAvaliable(currentAction.GetActionType(), 1, true))
        {
            gameObject.SetActive(false);
        }
    }
}
