using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMoveSpeed : DisplayActorResourcesBase
{
    [SerializeField] private TextMeshProUGUI moveRemainingText;

    public override void UpdateUI(ActingEntity actor)
    {
        moveRemainingText.text = actor.actionsRemaining[ActionType.MOVE].ToString();
    }
}
