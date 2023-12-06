using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayActionsRemaining : DisplayActorResourcesBase
{
    [SerializeField] private Image fullActionImage;
    [SerializeField] private Image quickActionImage;

    [SerializeField] private float lerpRate;
    private float fullActionStartFill;
    private float quickActionStartFill;

    void Start()
    {
        fullActionStartFill = fullActionImage.fillAmount;
        quickActionStartFill = quickActionImage.fillAmount;
    }

    public override void UpdateUI(ActingEntity actor)
    {
        if (actor.IsActionAvaliable(ActionType.FULL, 1))
        {
            StartCoroutine(FadeFillTo(fullActionImage, fullActionStartFill));
            StartCoroutine(FadeFillTo(quickActionImage, quickActionStartFill));
        }
        else if (actor.IsActionAvaliable(ActionType.QUICK, 1))
        {
            StartCoroutine(FadeFillTo(fullActionImage, 0));
            StartCoroutine(FadeFillTo(quickActionImage, 0.5f));
        }
        else
        {
            StartCoroutine(FadeFillTo(fullActionImage, 0));
            StartCoroutine(FadeFillTo(quickActionImage, 0));
        }
    }

    private IEnumerator FadeFillTo(Image targetImage, float newFill)
    {
        float startFill = targetImage.fillAmount;
        float alpha = 0;
        float alphaTarget = Mathf.Max(startFill, newFill) - Mathf.Min(startFill, newFill);

        while (targetImage.fillAmount != newFill)
        {
            targetImage.fillAmount = Mathf.Lerp(startFill, newFill, Mathf.Min(alpha / alphaTarget, 1));
            alpha += lerpRate * Time.deltaTime;
            yield return null;
        }
    }
}
