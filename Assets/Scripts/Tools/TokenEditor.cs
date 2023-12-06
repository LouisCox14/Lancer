using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TokenEditor : MonoBehaviour
{
    [Header("Border Settings")]
    [SerializeField] [Range(0, 1)] private float borderThickness;
    [SerializeField] private Color borderColour;

    [Header("Image Settings")]
    [SerializeField] [Tooltip("Only functions if image background is transparent.")] private Color backgroundColour;
    [SerializeField] private Sprite spriteImage;
    [SerializeField] private float spriteScale;
    [SerializeField] private Vector2 spriteOffset;

    private GameObject backgroundObject;
    private SpriteRenderer backgroundRenderer;
    private GameObject spriteObject;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer border;

    void Start()
    {
        backgroundObject = transform.GetChild(0).gameObject;
        backgroundRenderer = backgroundObject.GetComponent<SpriteRenderer>();
        spriteObject = backgroundObject.transform.GetChild(0).gameObject;
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        border = transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        border.color = borderColour;

        backgroundRenderer.color = backgroundColour;
        backgroundObject.transform.localScale = new Vector3(1, 1, 1) * (1 - borderThickness);

        spriteRenderer.sprite = spriteImage;

        spriteObject.transform.localPosition = spriteOffset;
        spriteObject.transform.localScale = new Vector2(1, 1) * spriteScale;
    }
}
