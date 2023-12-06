using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleObjectToGrid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public ComponentObject gridObject;

    void Start()
    {
        Scale();
    }

    public void Scale()
    {
        Grid grid = (Grid)gridObject.value;

        float xScale = grid.GetBoundsLocal(Vector3Int.zero).extents.x / (spriteRenderer.bounds.extents.x / transform.localScale.x);
        float yScale = grid.GetBoundsLocal(Vector3Int.zero).extents.y / (spriteRenderer.bounds.extents.y / transform.localScale.y);
        
        transform.localScale = new Vector3(xScale, yScale, 0);
    }
}