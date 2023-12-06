using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [HideInInspector] public Vector2Int position { get; private set; }

    public ComponentObject gridObject;
    [Tooltip("Assign from most general to most specific. The first set is used to find neighbours.")]
    public List<GridLookupSet> gridSets = new List<GridLookupSet>();

    public void OnEnable()
    {
        Grid grid = (Grid)gridObject.value;
        position = (Vector2Int)grid.WorldToCell(transform.position);
        SubscribeAllSets();
    }

    public void OnDisable()
    {
        UnsubscribeAllSets();
    }

    public void SetPosition(Vector2Int newPosition)
    {
        UnsubscribeAllSets();
        position = newPosition;
        SubscribeAllSets();
    }

    void SubscribeAllSets()
    {
        foreach (GridLookupSet set in gridSets)
        {
            set.RegisterSetObject(this);
        }
    }
    
    void UnsubscribeAllSets()
    {
        foreach (GridLookupSet set in gridSets)
        {
            set.UnregisterObjectAt(position);
        }
    }
}
